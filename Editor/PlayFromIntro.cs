using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayFromIntro : UnityEditor.Editor
{
    [MenuItem(EditorValues.ToolsTabName + "/Play from intro", false, 11)]
    private static void Play()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/#Scenes/IntroScene.unity");
        EditorApplication.isPlaying = true;
    }
}

