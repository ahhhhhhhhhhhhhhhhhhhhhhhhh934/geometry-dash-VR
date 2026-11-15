#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeometryDashVR.VR.Editor
{
    public static class VRPrototypeSceneSetup
    {
        [MenuItem("Geometry Dash VR/Create Prototype Scene")]
        public static void CreateScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var world = new GameObject("VR Prototype World");
            world.AddComponent<VRPrototypeBootstrap>();

            var player = new GameObject("VR Player");
            player.transform.position = new Vector3(0f, 1.1f, 0f);
            var character = player.AddComponent<CharacterController>();
            character.height = 1.8f;
            character.radius = 0.28f;
            character.center = new Vector3(0f, 0f, 0f);

            var cameraObject = new GameObject("Player Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();

            var runner = player.AddComponent<VRRunnerController>();
            var runnerCamera = typeof(VRRunnerController).GetField(
                "headCamera", System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            runnerCamera?.SetValue(runner, cameraObject.transform);

            player.AddComponent<VRStartupCheck>();

            const string folder = "Assets/Scenes";
            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets", "Scenes");

            const string path = folder + "/VRPrototype.unity";
            EditorSceneManager.SaveScene(scene, path);

            var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);
            scenes.RemoveAll(item => item.path == path);
            scenes.Insert(0, new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();

            AssetDatabase.Refresh();
            Debug.Log("[Geometry Dash VR] Created Assets/Scenes/VRPrototype.unity. " +
                      "Enable Android OpenXR in Project Settings before Quest testing.");
        }
    }
}
#endif
