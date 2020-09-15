using UnityEditor;
using UnityEngine;

namespace FazAppFramework.Development.Editor
{
    public class PlayerPrefsClear : EditorWindow
    {
        [MenuItem(EditorValues.ToolsTabName + "/Clear PlayerPrefs", false, 12)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

    }
}
