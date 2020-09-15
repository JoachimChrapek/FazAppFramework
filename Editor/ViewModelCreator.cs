using System.IO;
using UnityEditor;
using UnityEngine;

namespace FazAppFramework.Editor
{
    public class ViewModelCreator : EditorWindow
    {
        private static string path;
        private static string className;

        private float labelWidth = 200f;

        private static ViewModelCreator window;

        [MenuItem("Assets/Create/Scripts/ViewViewModel")]
        private static void CreateViewViewModelMenuItem()
        {
            path = AssetDatabase.GetAssetPath (Selection.activeObject);
            className = "";
            
            window = (ViewModelCreator) EditorWindow.GetWindow(typeof(ViewModelCreator), true,
                "View-ViewModel Creator", true);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = labelWidth;

            className = EditorGUILayout.TextField("Classes name", className);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("View class name", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField($"{className}View.cs");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ViewModel class name", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField($"{className}ViewModel.cs");
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                if (string.IsNullOrEmpty(className))
                {
                    Debug.LogError("Name of View-ViewModel classes cannot be empty");
                    return;
                }

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError("Path is empty");
                    return;
                }

                CreateFiles();
                window.Close();
            }
        }

        private void CreateFiles()
        {
            using (StreamWriter writer = new StreamWriter(path + $"/{className}View.cs"))
            {
                writer.WriteLine("using FazAppFramework.UI.MVVM;");
                writer.WriteLine("");
                writer.WriteLine($"public class {className}View : View<{className}ViewModel>");
                writer.WriteLine("{");
                writer.WriteLine("  ");
                writer.WriteLine("}");
            }

            using (StreamWriter writer = new StreamWriter(path + $"/{className}ViewModel.cs"))
            {
                writer.WriteLine("using FazAppFramework.UI.MVVM;");
                writer.WriteLine("");
                writer.WriteLine($"public class {className}ViewModel : ViewModel");
                writer.WriteLine("{");
                writer.WriteLine("  ");
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }
    }
}

