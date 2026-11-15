using UnityEngine;
using UnityEngine.XR;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// Experimental arm-based locomotion prototype.
    /// While a controller grip is held against a surface, tracked hand motion
    /// moves the CharacterController in the opposite direction.
    ///
    /// This is a small original prototype, not a port of Gorilla Tag's code.
    /// It requires a CharacterController on this object and assigned hand transforms.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class HandPushLocomotion : MonoBehaviour
    {
        [Header("Tracked hands")]
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        [Header("Contact")]
        [SerializeField, Min(0.01f)] private float handRadius = 0.09f;
        [SerializeField, Min(0.01f)] private float castDistance = 0.14f;
        [SerializeField] private LayerMask grippableLayers = ~0;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float movementScale = 1f;
        [SerializeField, Min(0f)] private float maxSpeed = 5.5f;
        [SerializeField, Min(0f)] private float smoothing = 14f;
        [SerializeField] private bool allowVerticalMovement = true;

        private CharacterController characterController;
        private Vector3 previousLeft;
        private Vector3 previousRight;
        private Vector3 smoothedVelocity;
        private bool initialized;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            initialized = false;
            smoothedVelocity = Vector3.zero;
        }

        private void Update()
        {
            if (leftHand == null || rightHand == null || !characterController.enabled)
                return;

            if (!initialized)
            {
                previousLeft = leftHand.position;
                previousRight = rightHand.position;
                initialized = true;
                return;
            }

            Vector3 desiredVelocity = Vector3.zero;
            float deltaTime = Mathf.Max(Time.deltaTime, 0.0001f);

            if (GripIsHeld(XRNode.LeftHand))
                desiredVelocity += GetPushVelocity(leftHand, ref previousLeft, deltaTime);
            else
                previousLeft = leftHand.position;

            if (GripIsHeld(XRNode.RightHand))
                desiredVelocity += GetPushVelocity(rightHand, ref previousRight, deltaTime);
            else
                previousRight = rightHand.position;

            if (!allowVerticalMovement)
                desiredVelocity.y = 0f;

            desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);
            smoothedVelocity = Vector3.Lerp(
                smoothedVelocity,
                desiredVelocity,
                1f - Mathf.Exp(-smoothing * deltaTime));

            characterController.Move(smoothedVelocity * deltaTime);
        }

        private Vector3 GetPushVelocity(
            Transform hand,
            ref Vector3 previousPosition,
            float deltaTime)
        {
            Vector3 currentPosition = hand.position;
            Vector3 handDelta = currentPosition - previousPosition;
            previousPosition = currentPosition;

            if (handDelta.sqrMagnitude < 0.000001f)
                return Vector3.zero;

            Vector3 castDirection = handDelta.normalized;
            float castLength = Mathf.Min(handDelta.magnitude + castDistance, 0.5f);

            if (!Physics.SphereCast(
                    previousPosition - handDelta,
                    handRadius,
                    castDirection,
                    out _,
                    castLength,
                    grippableLayers,
                    QueryTriggerInteraction.Ignore))
            {
                return Vector3.zero;
            }

            // Moving the hand into a surface should move the player away from it.
            Vector3 push = -handDelta / deltaTime * movementScale;
            return push;
        }

        private static bool GripIsHeld(XRNode node)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            return device.isValid &&
                   device.TryGetFeatureValue(CommonUsages.gripButton, out bool pressed) &&
                   pressed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            if (leftHand != null)
                Gizmos.DrawWireSphere(leftHand.position, handRadius);
            if (rightHand != null)
                Gizmos.DrawWireSphere(rightHand.position, handRadius);
        }
    }
}
