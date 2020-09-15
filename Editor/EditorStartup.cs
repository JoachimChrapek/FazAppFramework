using UnityEditor;

namespace FazAppFramework.Development.Editor
{
    [InitializeOnLoad]
    public static class EditorStartup
    {
        static EditorStartup()
        {
            PersonalSettings.OnStartup();
        }
    }
}
