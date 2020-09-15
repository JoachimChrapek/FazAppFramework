using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FazAppFramework.Development.Editor
{
    public class PersonalSettings : EditorWindow
    {
        private static bool loaded;
    
        private static bool alwaysStartFromIntro;

        public static void OnStartup()
        {
            LoadSettings();
        }

        [MenuItem(EditorValues.ToolsTabName + "/Personal settings", false, 31)]
        private static void Init()
        {
            LoadSettings();
            loaded = true;
            var window = (PersonalSettings)EditorWindow.GetWindow(typeof(PersonalSettings));
            window.Show();
        }

        private void OnGUI()
        {
            if (!loaded)
            {
                LoadSettings();
                loaded = true;
            }
        
            GUILayout.BeginVertical("box");
            alwaysStartFromIntro = EditorGUILayout.Toggle("Start from intro scene", alwaysStartFromIntro);
            GUILayout.EndVertical();

            GUILayout.Space(100);

            if (GUILayout.Button("\nSAVE\n"))
            {
                SaveSettings();
            }
        }

        private static void LoadSettings()
        {
            alwaysStartFromIntro = PlayerPrefs.GetInt(EditorValues.AlwaysStartFromIntroKey, 0) == 1;

            EditorSceneManager.playModeStartScene = alwaysStartFromIntro ? AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/#Scenes/IntroScene.unity") : null;
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetInt(EditorValues.AlwaysStartFromIntroKey, alwaysStartFromIntro ? 1 : 0);
            PlayerPrefs.Save();

            EditorSceneManager.playModeStartScene = alwaysStartFromIntro ? AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/#Scenes/IntroScene.unity") : null;
        }
    }
}



