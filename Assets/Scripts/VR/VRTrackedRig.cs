using UnityEngine;
using UnityEngine.XR;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// Applies OpenXR-tracked headset and controller poses to transforms under
    /// the player rig. Requires an active XR loader and tracked devices.
    /// </summary>
    public sealed class VRTrackedRig : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        public void Configure(Transform headTransform, Transform left, Transform right)
        {
            head = headTransform;
            leftHand = left;
            rightHand = right;
        }

        private void Update()
        {
            ApplyPose(XRNode.CenterEye, head);
            ApplyPose(XRNode.LeftHand, leftHand);
            ApplyPose(XRNode.RightHand, rightHand);
        }

        private static void ApplyPose(XRNode node, Transform target)
        {
            if (target == null)
                return;

            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            if (!device.isValid)
                return;

            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
                target.localPosition = position;

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
                target.localRotation = rotation;
        }
    }
}