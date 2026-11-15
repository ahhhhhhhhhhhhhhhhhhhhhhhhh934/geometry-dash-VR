#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeometryDashVR.Map.Editor
{
    public static class MapWorldSceneSetup
    {
        [MenuItem("Geometry Dash VR/Create The Map Scene")]
        public static void CreateScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var world = new GameObject("The Map - 2.21 Inspired World");
            world.AddComponent<MapWorldBootstrap>();

            var player = new GameObject("Map Explorer");
            player.transform.position = new Vector3(0f, 0.1f, 0f);
            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.28f;
            controller.center = new Vector3(0f, 0.9f, 0f);

            var cameraObject = new GameObject("Map Explorer Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = Vector3.zero;
            cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();

            var lightObject = new GameObject("Map Fill Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 35f;
            light.intensity = 1.5f;
            light.transform.position = new Vector3(0f, 12f, 18f);

            const string folder = "Assets/Scenes";
            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets", "Scenes");

            const string path = folder + "/TheMap.unity";
            EditorSceneManager.SaveScene(scene, path);
            var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.RemoveAll(item => item.path == path);
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.Refresh();
            Debug.Log("[Geometry Dash VR] Created The Map scene. Explore the platformer islands, towers, unlock shrine and secrets.");
        }
    }
}
#endif
