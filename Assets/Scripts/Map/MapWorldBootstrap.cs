using System.Collections.Generic;
using UnityEngine;

namespace GeometryDashVR.Map
{
    /// <summary>
    /// Original VR world-map inspired by the announced map concepts:
    /// player platformer stages, Demon Towers, unlocks, and hidden secrets.
    /// All geometry and labels are generated at runtime.
    /// </summary>
    public sealed class MapWorldBootstrap : MonoBehaviour
    {
        private readonly Color ice = new Color(0.22f, 0.78f, 1f);
        private readonly Color gold = new Color(1f, 0.72f, 0.12f);
        private readonly Color violet = new Color(0.62f, 0.34f, 1f);

        private void Start()
        {
            BuildMap();
        }

        private void BuildMap()
        {
            RenderSettings.ambientLight = new Color(0.42f, 0.52f, 0.72f);
            CreateLight();
            CreatePrimitive("Map island", PrimitiveType.Cylinder,
                new Vector3(0f, -0.8f, 18f), new Vector3(27f, 0.8f, 20f),
                new Color(0.12f, 0.3f, 0.55f));

            CreatePrimitive("Snowfield", PrimitiveType.Cylinder,
                new Vector3(0f, -0.25f, 18f), new Vector3(25f, 0.2f, 18f),
                new Color(0.72f, 0.9f, 1f));

            AddLabel("THE MAP", new Vector3(0f, 5.5f, 2f), 2.2f, gold);
            AddLabel("PLATFORMER LEVELS", new Vector3(-8f, 2.2f, 12f), 0.65f, ice);
            AddLabel("DEMON TOWERS", new Vector3(8f, 2.2f, 24f), 0.65f, violet);
            AddLabel("SECRETS & UNLOCKS", new Vector3(0f, 2.2f, 34f), 0.65f, gold);

            BuildPath();
            BuildPlatformerStages();
            BuildDemonTowers();
            BuildUnlockShrine();
            BuildSecrets();

            var start = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            start.name = "Map entrance";
            start.transform.position = new Vector3(0f, 0.1f, 2f);
            start.transform.localScale = new Vector3(2.5f, 0.15f, 2.5f);
            SetColor(start, new Color(0.15f, 1f, 0.65f));
            AddLabel("START", new Vector3(0f, 1.3f, 2f), 0.55f, Color.white);
        }

        private void BuildPath()
        {
            Vector3[] points =
            {
                new Vector3(0f, 0.15f, 3f),
                new Vector3(-3f, 0.25f, 7f),
                new Vector3(-8f, 0.25f, 12f),
                new Vector3(-4f, 0.25f, 17f),
                new Vector3(2f, 0.25f, 20f),
                new Vector3(8f, 0.25f, 24f),
                new Vector3(4f, 0.25f, 29f),
                new Vector3(0f, 0.25f, 34f)
            };

            for (int i = 0; i < points.Length; i++)
            {
                var tile = CreatePrimitive("Map path tile " + (i + 1), PrimitiveType.Cube,
                    points[i], new Vector3(2.2f, 0.3f, 2.2f),
                    new Color(0.18f, 0.55f, 0.82f));
                tile.transform.rotation = Quaternion.Euler(0f, (i % 2 == 0) ? 12f : -12f, 0f);
            }
        }

        private void BuildPlatformerStages()
        {
            CreateNode("FROSTBOUND", "Platformer Level 1", new Vector3(-8f, 0.5f, 12f), ice, true);
            CreateNode("CRYSTAL CAVERN", "Platformer Level 2", new Vector3(-4f, 0.5f, 17f), ice, false);
            CreateNode("GLACIER RUN", "Platformer Level 3", new Vector3(2f, 0.5f, 20f), ice, false);
        }

        private void BuildDemonTowers()
        {
            CreateTower("TOWER OF TRIALS", new Vector3(8f, 0.5f, 24f), 4.5f, violet, true);
            CreateTower("TOWER OF SHADOWS", new Vector3(4f, 0.5f, 29f), 6f, violet, false);
        }

        private void BuildUnlockShrine()
        {
            CreateNode("UNLOCK SHRINE", "Collectible rewards and unlocks", new Vector3(0f, 0.5f, 34f), gold, false);
            for (int i = 0; i < 5; i++)
            {
                float angle = i * Mathf.PI * 2f / 5f;
                Vector3 p = new Vector3(Mathf.Cos(angle) * 3f, 1.2f, 34f + Mathf.Sin(angle) * 3f);
                var shard = CreatePrimitive("Unlock crystal " + (i + 1), PrimitiveType.Octahedron,
                    p, Vector3.one * 0.65f, gold);
                shard.transform.rotation = Quaternion.Euler(20f, i * 25f, 15f);
            }
        }

        private void BuildSecrets()
        {
            var secret = CreatePrimitive("Hidden secret crystal", PrimitiveType.Octahedron,
                new Vector3(10f, 0.8f, 10f), Vector3.one * 0.9f, new Color(1f, 0.2f, 0.55f));
            secret.AddComponent<MapSecret>();
            AddLabel("???", new Vector3(10f, 2.2f, 10f), 0.8f, new Color(1f, 0.55f, 0.8f));

            var secret2 = CreatePrimitive("Hidden cave entrance", PrimitiveType.Cube,
                new Vector3(-11f, 1.2f, 29f), new Vector3(2.8f, 2.4f, 0.7f),
                new Color(0.08f, 0.12f, 0.24f));
            secret2.AddComponent<MapSecret>();
        }

        private void CreateTower(string title, Vector3 basePosition, float height, Color color, bool unlocked)
        {
            CreateNode(title, "Demon Tower", basePosition, color, unlocked);
            for (int i = 0; i < 4; i++)
            {
                float y = 1.2f + i * height * 0.22f;
                var floor = CreatePrimitive(title + " floor " + i, PrimitiveType.Cube,
                    basePosition + new Vector3(0f, y, 0f),
                    new Vector3(2.3f - i * 0.25f, 0.3f, 2.3f - i * 0.25f), color);
                floor.transform.rotation = Quaternion.Euler(0f, i * 12f, 0f);
            }
            CreatePrimitive(title + " spire", PrimitiveType.Cylinder,
                basePosition + new Vector3(0f, height + 1.2f, 0f),
                new Vector3(0.35f, 1.1f, 0.35f), color);
        }

        private void CreateNode(string title, string subtitle, Vector3 position, Color color, bool unlocked)
        {
            var node = CreatePrimitive(title, PrimitiveType.Sphere,
                position, Vector3.one * 1.45f, unlocked ? color : new Color(0.22f, 0.27f, 0.34f));
            node.AddComponent<MapNode>().Initialize(title, subtitle, unlocked);
            AddLabel(title, position + Vector3.up * 1.5f, 0.42f, Color.white);
        }

        private static GameObject CreatePrimitive(string title, PrimitiveType type,
            Vector3 position, Vector3 scale, Color color)
        {
            var obj = GameObject.CreatePrimitive(type);
            obj.name = title;
            obj.transform.position = position;
            obj.transform.localScale = scale;
            SetColor(obj, color);
            return obj;
        }

        private static void SetColor(GameObject obj, Color color)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            var material = new Material(shader) { color = color };
            renderer.material = material;
        }

        private static void AddLabel(string value, Vector3 position, float size, Color color)
        {
            var label = new GameObject("Map label - " + value);
            label.transform.position = position;
            label.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            var text = label.AddComponent<TextMesh>();
            text.text = value;
            text.fontSize = 64;
            text.characterSize = size * 0.025f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = color;
        }

        private static void CreateLight()
        {
            var lightObject = new GameObject("Map directional light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.25f;
            lightObject.transform.rotation = Quaternion.Euler(48f, -25f, 0f);
        }
    }

    public sealed class MapNode : MonoBehaviour
    {
        private string title;
        private string subtitle;
        private bool unlocked;
        private bool visited;

        public void Initialize(string nodeTitle, string nodeSubtitle, bool startsUnlocked)
        {
            title = nodeTitle;
            subtitle = nodeSubtitle;
            unlocked = startsUnlocked;
            var trigger = gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.8f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<CharacterController>() == null)
                return;

            if (!unlocked)
            {
                Debug.Log("[The Map] " + title + " is locked. Explore other areas to unlock it.");
                return;
            }

            if (!visited)
            {
                visited = true;
                Debug.Log("[The Map] Discovered " + title + " — " + subtitle);
            }
        }
    }

    public sealed class MapSecret : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<CharacterController>() != null)
                Debug.Log("[The Map] SECRET FOUND! A hidden discovery has been added to your collection.");
        }
    }
}
