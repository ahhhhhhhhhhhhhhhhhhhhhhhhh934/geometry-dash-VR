using UnityEngine;

namespace GeometryDashVR.VR
{
    /// <summary>
    /// Builds a small original obstacle-course scene at runtime so the first
    /// prototype can be tested without importing proprietary game assets.
    /// Attach this component to an empty scene object.
    /// </summary>
    public sealed class VRPrototypeBootstrap : MonoBehaviour
    {
        [SerializeField] private float trackLength = 90f;
        [SerializeField] private float trackWidth = 5f;

        private void Start()
        {
            BuildWorld();
        }

        private void BuildWorld()
        {
            CreateBlock("Track", new Vector3(0f, -0.25f, trackLength * 0.5f),
                new Vector3(trackWidth, 0.5f, trackLength), new Color(0.12f, 0.14f, 0.2f));

            CreateBlock("Left rail", new Vector3(-trackWidth * 0.5f, 0.25f, trackLength * 0.5f),
                new Vector3(0.18f, 0.5f, trackLength), new Color(0.1f, 0.75f, 1f));
            CreateBlock("Right rail", new Vector3(trackWidth * 0.5f, 0.25f, trackLength * 0.5f),
                new Vector3(0.18f, 0.5f, trackLength), new Color(0.1f, 0.75f, 1f));

            for (int i = 0; i < 18; i++)
            {
                float z = 9f + i * 4.2f;
                float lane = (i % 3 - 1) * 1.35f;
                float height = i % 4 == 0 ? 2.2f : 1.1f;
                Vector3 size = i % 4 == 0
                    ? new Vector3(1.15f, 0.55f, 0.9f)
                    : new Vector3(1.1f, height, 0.9f);

                CreateBlock("Obstacle " + (i + 1),
                    new Vector3(lane, height * 0.5f + 0.05f, z),
                    size, new Color(1f, 0.28f, 0.32f), true);
            }

            CreateBlock("Finish marker", new Vector3(0f, 2f, trackLength - 2f),
                new Vector3(trackWidth, 0.15f, 0.3f), new Color(0.2f, 1f, 0.55f));

            var lightObject = new GameObject("Prototype directional light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            RenderSettings.ambientLight = new Color(0.38f, 0.4f, 0.48f);
        }

        private static GameObject CreateBlock(
            string objectName, Vector3 position, Vector3 scale, Color color,
            bool obstacle = false)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = objectName;
            block.transform.position = position;
            block.transform.localScale = scale;

            var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if (material.shader == null)
                material.shader = Shader.Find("Standard");
            material.color = color;
            block.GetComponent<Renderer>().material = material;

            if (obstacle)
            {
                var hit = block.AddComponent<VRObstacle>();
                hit.enabled = true;
            }

            return block;
        }
    }

    public sealed class VRObstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<VRRunnerController>() != null)
                VRRunnerController.NotifyHit();
        }
    }
}
