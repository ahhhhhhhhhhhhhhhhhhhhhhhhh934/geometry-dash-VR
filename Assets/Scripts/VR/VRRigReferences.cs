using UnityEngine;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// References the tracked headset and controller transforms in the XR Origin.
    /// Assign these in the Unity Inspector.
    /// </summary>
    public sealed class VRRigReferences : MonoBehaviour
    {
        [field: SerializeField] public Camera HeadCamera { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }

        public bool IsConfigured =>
            HeadCamera != null && LeftHand != null && RightHand != null;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (HeadCamera == null)
                HeadCamera = GetComponentInChildren<Camera>();
        }
#endif
    }
}
