using UnityEngine;
using UnityEngine.XR;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// First playable movement slice: forward auto-run, controller-trigger
    /// jump, desktop Space fallback, obstacle collision, and finish detection.
    /// This is a prototype controller, not a complete Geometry Dash physics model.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class VRRunnerController : MonoBehaviour
    {
        [SerializeField] private Transform headCamera;
        [SerializeField, Min(0f)] private float forwardSpeed = 7f;
        [SerializeField, Min(0f)] private float jumpHeight = 2.1f;
        [SerializeField, Min(0f)] private float gravity = 22f;
        [SerializeField] private float laneLimit = 1.85f;
        [SerializeField] private float laneSpeed = 5f;

        private CharacterController character;
        private float verticalSpeed;
        private bool finished;
        private static VRRunnerController active;

        private void Awake()
        {
            character = GetComponent<CharacterController>();
            active = this;
        }

        private void Update()
        {
            if (finished)
                return;

            if (JumpPressed() && character.isGrounded)
                verticalSpeed = Mathf.Sqrt(jumpHeight * 2f * gravity);

            if (character.isGrounded && verticalSpeed < 0f)
                verticalSpeed = -1f;

            verticalSpeed -= gravity * Time.deltaTime;

            float steer = ReadSteering();
            Vector3 velocity = new Vector3(steer * laneSpeed, verticalSpeed, forwardSpeed);
            character.Move(velocity * Time.deltaTime);

            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, -laneLimit, laneLimit);
            transform.position = position;

            if (position.z >= 87f)
            {
                finished = true;
                Debug.Log("[Geometry Dash VR] Prototype course complete!");
            }
        }

        private bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return true;

            return ControllerButtonPressed(XRNode.LeftHand, CommonUsages.triggerButton) ||
                   ControllerButtonPressed(XRNode.RightHand, CommonUsages.triggerButton);
        }

        private float ReadSteering()
        {
            float keyboard = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(keyboard) > 0.01f)
                return keyboard;

            InputDevice left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (left.isValid && left.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
                return Mathf.Clamp(axis.x, -1f, 1f);

            return 0f;
        }

        private static bool ControllerButtonPressed(XRNode node, InputFeatureUsage<bool> button)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            return device.isValid && device.TryGetFeatureValue(button, out bool pressed) && pressed;
        }

        public static void NotifyHit()
        {
            if (active == null || active.finished)
                return;

            active.finished = true;
            Debug.Log("[Geometry Dash VR] Hit an obstacle. Restart the scene to try again.");
        }

        private void OnDestroy()
        {
            if (active == this)
                active = null;
        }
    }
}
