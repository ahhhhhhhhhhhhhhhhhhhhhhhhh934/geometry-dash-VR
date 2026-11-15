using UnityEngine;
using UnityEngine.XR.Management;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// Reports whether Unity's XR loader initialized. This does not configure
    /// OpenXR automatically; enable the appropriate provider in Project Settings.
    /// </summary>
    public sealed class VRStartupCheck : MonoBehaviour
    {
        private void Start()
        {
            var manager = XRGeneralSettings.Instance != null
                ? XRGeneralSettings.Instance.Manager
                : null;

            if (manager == null || manager.activeLoader == null)
            {
                Debug.LogWarning(
                    "[Geometry Dash VR] No active XR loader. Configure OpenXR " +
                    "in Project Settings > XR Plug-in Management before testing on Quest.");
                return;
            }

            Debug.Log(
                $"[Geometry Dash VR] XR loader initialized: {manager.activeLoader.name}");
        }
    }
}
