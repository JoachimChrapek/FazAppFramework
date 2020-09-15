using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FazAppFramework.Managers;
using GoogleMobileAds.Editor;
using Unity.Notifications;
using UnityEditor; 
using UnityEditor.Compilation;
using UnityEngine;
using File = System.IO.File;
 
namespace FazAppFramework.Editor
{
    internal class FrameworkConfiguration : EditorWindow
    {
        private const float labelWidth = 200f;

        private bool isLoaded;
        private GUIStyle sectionTitleStyle;
        private GUIStyle warningTextStyle;
        private GUIStyle xButtonStyle;
        private GUIStyle redBoxStyle;
        private GUIStyle redTextStyle;

        private FrameworkValues frameworkValues;
        private string path;

        private Vector2 scrollPos;
        private string newPackageName;

        private const string ActivityCheckKey = "NotificationsAutoActivityCheck";

        [MenuItem(EditorValues.ToolsTabName + "/Configure Framework", false, 0)]
        private static void Init()
        {
            var window = (FrameworkConfiguration) EditorWindow.GetWindow(typeof(FrameworkConfiguration), false,
                "Framework Configuration", true);
            window.Show();
        }

        private void Load()
        {
            if (frameworkValues == null)
            {
                path = FrameworkConstantValues.FrameworkValuesFilePath;

                try
                {
                    string serialized;

                    if (!File.Exists(path))
                    {
                        var tmp = new FrameworkValues();
                        serialized = JsonUtility.ToJson(tmp, true);
                        File.WriteAllText(path, serialized);
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        serialized = File.ReadAllText(path);
                    }

                    frameworkValues = JsonUtility.FromJson<FrameworkValues>(serialized);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            if (sectionTitleStyle == null)
            {
                sectionTitleStyle = new GUIStyle();
                sectionTitleStyle.alignment = TextAnchor.MiddleCenter;
                sectionTitleStyle.fontSize = 16;
            }

            if (warningTextStyle == null)
            {
                warningTextStyle = new GUIStyle();
                warningTextStyle.alignment = TextAnchor.UpperRight;
                warningTextStyle.fontSize = 16;
                warningTextStyle.normal.textColor = Color.red;
                warningTextStyle.hover.textColor = Color.red;
                warningTextStyle.active.textColor = Color.red;
                warningTextStyle.focused.textColor = Color.red;
            }

            if (xButtonStyle == null)
            {
                xButtonStyle = new GUIStyle(GUI.skin.button);
                xButtonStyle.normal.textColor = Color.red;
                xButtonStyle.hover.textColor = Color.red;
                xButtonStyle.active.textColor = Color.red;
                xButtonStyle.focused.textColor = Color.red;
                xButtonStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (redBoxStyle == null)
            {
                redBoxStyle = new GUIStyle();
                redBoxStyle.normal.background = Resources.Load<Texture2D>("EditorBoxStyle");
                redBoxStyle.border = new RectOffset(3, 3, 3, 3);
                redBoxStyle.padding = new RectOffset(10, 10, 10, 10);
            }

            if (redTextStyle == null)
            {
                redTextStyle = new GUIStyle();
                redTextStyle.fontSize = 14;
                redTextStyle.alignment = TextAnchor.MiddleCenter;
                redTextStyle.normal.textColor = Color.red;
                redTextStyle.hover.textColor = Color.red;
                redTextStyle.active.textColor = Color.red;
                redTextStyle.focused.textColor = Color.red;
            }

            OnAdsGuiChange();

            autoActivityCheck = PlayerPrefs.GetInt(ActivityCheckKey, 0) == 1;

            isLoaded = true;
        }

        private void SaveConfiguration()
        {
            try
            {
                var serialized = JsonUtility.ToJson(frameworkValues, true);
                File.WriteAllText(path, serialized);
                AssetDatabase.Refresh();

                OnAdsGuiChange();

                SaveDrawableResources();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void OnGUI()
        {
            if (!isLoaded)
            {
                Load();
            }

            EditorGUIUtility.labelWidth = labelWidth;

            EditorGUILayout.LabelField("Make sure to save before closing!", warningTextStyle);
            EditorGUILayout.Space(24);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            DrawGeneralWindow();

            //DrawGooglePlayServicesWindow();

            DrawAdsWindow();

            DrawFirebaseWindow();

            DrawLocalNotificationsWindow();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(24);
            if (GUILayout.Button("\nSAVE CONFIGURATION\n"))
            {
                SaveConfiguration();
            }
        }
        
        private void DrawGeneralWindow()
        {
            EditorGUILayout.BeginVertical("box");

            DrawSectionTitle("General");
            
            PlayerSettings.bundleVersion = EditorGUILayout.TextField("Version", PlayerSettings.bundleVersion);
            PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("Bundle version", PlayerSettings.Android.bundleVersionCode);
            
            GUILayout.Space(6);

            PlayerSettings.productName = EditorGUILayout.TextField("Product name", PlayerSettings.productName);
            PlayerSettings.companyName = EditorGUILayout.TextField("Company name", PlayerSettings.companyName);

            GUILayout.Space(6);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Package name:", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(PlayerSettings.applicationIdentifier);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("New package name:", GUILayout.Width(labelWidth));
            newPackageName = EditorGUILayout.TextField(newPackageName);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("CHANGE PACKAGE NAME"))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, newPackageName);
                newPackageName = "";
            }

            GUILayout.Space(6);
            
            PlayerSettings.Android.targetArchitectures = (AndroidArchitecture)EditorGUILayout.EnumPopup("Target architecture",
                PlayerSettings.Android.targetArchitectures);

            PlayerSettings.defaultInterfaceOrientation =
                (UIOrientation)EditorGUILayout.EnumPopup("Orientation", PlayerSettings.defaultInterfaceOrientation);

            GUILayout.Space(10);

            frameworkValues.MORE_GAMES_LINK =
                EditorGUILayout.TextField("More Games link", frameworkValues.MORE_GAMES_LINK);

            GUILayout.Space(10);

            frameworkValues.SERVICE_WAITING_TIME = EditorGUILayout.FloatField("Max wait time for services loading",
                frameworkValues.SERVICE_WAITING_TIME);
            
            EditorGUILayout.EndVertical();
        }

        //private void DrawGooglePlayServicesWindow()
        //{
        //    EditorGUILayout.BeginVertical("box");

        //    DrawSectionTitle("Google Play Services");

        //    EditorGUILayout.LabelField("TODO: Do not configure if you are not using services");
        //    if (GUILayout.Button("Configure GPS"))
        //    {
        //        GPGSAndroidSetupUI.MenuItemFileGPGSAndroidSetup();
        //    }

        //    EditorGUILayout.EndVertical();
        //}

        private void DrawAdsWindow()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box");

            DrawSectionTitle("ADS");

            frameworkValues.UseAdmob = EditorGUILayout.ToggleLeft("Use ADMOB", frameworkValues.UseAdmob);
            if (frameworkValues.UseAdmob)
            {
                GUILayout.Space(10);
                frameworkValues.TestAds = EditorGUILayout.Toggle("Test ads", frameworkValues.TestAds);
                frameworkValues.AdmobAppID = EditorGUILayout.TextField("App ID", frameworkValues.AdmobAppID);
                frameworkValues.InterstitialID =
                    EditorGUILayout.TextField("Interstitial ID", frameworkValues.InterstitialID);
                frameworkValues.RewardedVideoID =
                    EditorGUILayout.TextField("Rewarded Video ID", frameworkValues.RewardedVideoID);
                frameworkValues.PreloadedRewardedVideosCount = EditorGUILayout.IntField("Preloaded rewarded videos",
                    frameworkValues.PreloadedRewardedVideosCount);
                frameworkValues.TimeAfterAdFailedToLoad =
                    EditorGUILayout.FloatField("Time to retry after ad failed to load",
                        frameworkValues.TimeAfterAdFailedToLoad);

                if (frameworkValues.TestDevices == null)
                {
                    frameworkValues.TestDevices = new List<string>();
                }

                GUILayout.Space(12);
                EditorGUILayout.LabelField("Test devices");
                for (int i = 0; i < frameworkValues.TestDevices.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    frameworkValues.TestDevices[i] = EditorGUILayout.TextField(frameworkValues.TestDevices[i]);
                    if (DrawXButton())
                    {
                        frameworkValues.TestDevices.RemoveAt(i);
                        return;
                    }

                    EditorGUILayout.EndVertical();
                }

                if (GUILayout.Button("Add test device"))
                {
                    frameworkValues.TestDevices.Add("");
                }

                GUILayout.Space(12);
            }

            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                OnAdsGuiChange();
            }
        }

        private void OnAdsGuiChange()
        {
            GoogleMobileAdsSettings.Instance.IsAdMobEnabled = frameworkValues.UseAdmob;
            if (frameworkValues.UseAdmob)
            {
                GoogleMobileAdsSettings.Instance.AdMobAndroidAppId = frameworkValues.AdmobAppID;
            }

            EditorUtility.SetDirty(GoogleMobileAdsSettings.Instance);
            GoogleMobileAdsSettings.Instance.WriteSettingsToFile();
        }

        private void DrawFirebaseWindow()
        {
            EditorGUILayout.BeginVertical("box");

            DrawSectionTitle("Firebase");

            frameworkValues.UseFirebase = EditorGUILayout.ToggleLeft("Use Firebase", frameworkValues.UseFirebase);
            if (frameworkValues.UseFirebase)
            {
                var configFileExists = File.Exists(Application.dataPath + "/google-services.json");
                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleLeft;
                style.normal.textColor = configFileExists
                    ? Color.green
                    : Color.red;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Firebase config file status:", GUILayout.Width(labelWidth));
                EditorGUILayout.LabelField(configFileExists ? "OK" : "MISSING", style);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(12);

                frameworkValues.UseOwnIntertital = EditorGUILayout.ToggleLeft("Use own interstitial", frameworkValues.UseOwnIntertital);
                if (frameworkValues.UseOwnIntertital)
                {
                    EditorGUILayout.LabelField("Own interstitial value keys");
                    frameworkValues.FIREBASE_REMOTE_CONFIG_SHOW_OWN_INTERSTITIAL_KEY_B        = EditorGUILayout.TextField("Show own interstitial key",frameworkValues.FIREBASE_REMOTE_CONFIG_SHOW_OWN_INTERSTITIAL_KEY_B);
                    frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_IMAGE_URL_KEY_S   = EditorGUILayout.TextField("Image URL key",frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_IMAGE_URL_KEY_S);
                    frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_STORE_URL_KEY_S   = EditorGUILayout.TextField("Store URL key",frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_STORE_URL_KEY_S);
                    frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_TITLE_KEY_S       = EditorGUILayout.TextField("Title text key",frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_TITLE_KEY_S);
                    frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_DESCRIPTION_KEY_S = EditorGUILayout.TextField("Description text key",frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_DESCRIPTION_KEY_S);
                }
            }

            GUILayout.Space(12);

            EditorGUILayout.EndVertical();
        }

        private const string DefaultActivityName = "com.unity3d.player.UnityPlayerActivity";
        private string currentActivityName;
        private bool autoActivityCheck;

        private DrawableResourceDataContainer drawableContainer;
        private string notificationSettingsPath =
            Directory.GetCurrentDirectory() + "/ProjectSettings/NotificationsSettings.asset";

        private void DrawLocalNotificationsWindow()
        {
            EditorGUILayout.BeginVertical("box");

            DrawSectionTitle("Local Notifications");

            frameworkValues.UseLocalNotifications =
                EditorGUILayout.ToggleLeft("Use local notifications", frameworkValues.UseLocalNotifications);
            if (frameworkValues.UseLocalNotifications)
            {
                if (!NotificationSettings.AndroidSettings.RescheduleOnDeviceRestart)
                {
                    NotificationSettings.AndroidSettings.RescheduleOnDeviceRestart = true;
                }

                GUILayout.Space(12);

                if (autoActivityCheck && string.IsNullOrEmpty(currentActivityName))
                {
                    CheckActivityName();
                }

                string statusText;
                Color statusColor;
                var isActivityOk = false;

                if (string.IsNullOrEmpty(currentActivityName))
                {
                    statusText = "CHECK REQUIRED";
                    statusColor = Color.yellow;
                }
                else if (!NotificationSettings.AndroidSettings.UseCustomActivity &&
                         currentActivityName == DefaultActivityName ||
                         NotificationSettings.AndroidSettings.UseCustomActivity && currentActivityName ==
                         NotificationSettings.AndroidSettings.CustomActivityString)
                {
                    statusText = "OK";
                    statusColor = Color.green;
                    isActivityOk = true;
                }
                else
                {
                    statusText = "WRONG";
                    statusColor = Color.red;
                }
                
                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleLeft;
                style.normal.textColor = statusColor;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Activity name status:", GUILayout.Width(labelWidth));
                EditorGUILayout.LabelField(statusText, style);
                EditorGUILayout.EndHorizontal();

                var tmp = EditorGUILayout.ToggleLeft("Auto Check&Fix Activity", autoActivityCheck);
                if (tmp != autoActivityCheck)
                {
                    autoActivityCheck = tmp;
                    PlayerPrefs.SetInt(ActivityCheckKey, autoActivityCheck ? 1 : 0);
                }

                if (autoActivityCheck && !isActivityOk)
                {
                    FixActivityName();
                }
                else if(!autoActivityCheck)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("CHECK"))
                    {
                        CheckActivityName();
                    }

                    if (GUILayout.Button("FIX") && !string.IsNullOrEmpty(currentActivityName))
                    {
                        FixActivityName();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                void CheckActivityName()
                {
                    var manifestText = File.ReadAllText(Application.dataPath + "/Plugins/Android/AndroidManifest.xml");
                    var searchString = "activity android:name=\"";
                    var startIndex = manifestText.IndexOf(searchString) + searchString.Length;
                    var endIndex = manifestText.IndexOf('"', startIndex);
                    currentActivityName = manifestText.Substring(startIndex, endIndex - startIndex);
                    Debug.Log("Current activity name (in Manifest): " + currentActivityName);
                }

                void FixActivityName()
                {
                    if (currentActivityName == DefaultActivityName)
                    {
                        NotificationSettings.AndroidSettings.UseCustomActivity = false;
                    }
                    else
                    {
                        
                        NotificationSettings.AndroidSettings.UseCustomActivity = true;
                        NotificationSettings.AndroidSettings.CustomActivityString = currentActivityName;
                    }
                }

                if (frameworkValues.NotificationsData == null)
                {
                    var allTypes = (LocalNotificationType[]) Enum.GetValues(typeof(LocalNotificationType));

                    frameworkValues.NotificationsData = new LocalNotificationData[allTypes.Length];

                    for (int i = 0; i < allTypes.Length; i++)
                    {
                        var data = new LocalNotificationData {type = allTypes[i]};
                        frameworkValues.NotificationsData[i] = data;
                    }
                }
                GUILayout.Space(12);
                
                if (drawableContainer == null)
                {
                    drawableContainer = new DrawableResourceDataContainer();
                    EditorJsonUtility.FromJsonOverwrite(GetDrawableResourcesDataContainerFromNotificationsSettings(), drawableContainer);
                }

                EditorGUILayout.LabelField("NOTE: Small Icons will be always white on transparent background (it will be only white shape)");
                EditorGUILayout.LabelField("Large icons can have Colors - preview for icons is in Project Settings -> Mobile Notifications");

                var wrongIconImageFlag = false;
                foreach (var data in drawableContainer.DrawableResources)
                {
                    EditorGUILayout.BeginVertical(redBoxStyle);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    data.Id = EditorGUILayout.TextField("Identifier", data.Id);
                    data.Type = (NotificationIconType) EditorGUILayout.EnumPopup("Type", data.Type);
                    EditorGUILayout.EndVertical();
                    data.Asset = (Texture2D) EditorGUILayout.ObjectField("Icon", data.Asset, typeof(Texture2D), false);
                    if (DrawXButton())
                    {
                        drawableContainer.DrawableResources.Remove(data);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (string.IsNullOrEmpty(data.Id))
                    {
                        wrongIconImageFlag = true;
                        EditorGUILayout.LabelField("Id cannot be empty", redTextStyle);
                    }

                    if (data.Asset == null)
                    {
                        wrongIconImageFlag = true;
                        EditorGUILayout.LabelField("Image asset cannot be null", redTextStyle);
                    }
                    else if (data.Asset != null)
                    {
                        if (!data.Asset.isReadable)
                        {
                            wrongIconImageFlag = true;
                            EditorGUILayout.LabelField("Image asset must have Read/Write enabled", redTextStyle);
                        }

                        if (data.Asset.width != data.Asset.height)
                        {
                            wrongIconImageFlag = true;
                            EditorGUILayout.LabelField("Icon must have same width and height (square image)", redTextStyle);
                        }

                        if (data.Type == NotificationIconType.Small &&
                            (data.Asset.width < 48 || data.Asset.height < 48))
                        {
                            wrongIconImageFlag = true;
                            EditorGUILayout.LabelField("Small icons must be at least 48x48px", redTextStyle);
                        }

                        if (data.Type == NotificationIconType.Large &&
                            (data.Asset.width < 192 || data.Asset.height < 192))
                        {
                            wrongIconImageFlag = true;
                            EditorGUILayout.LabelField("Small icons must be at least 48x48px", redTextStyle);
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
                
                if (GUILayout.Button("Add icon"))
                {
                    drawableContainer.DrawableResources.Add(new DrawableResourceData());
                }

                if (wrongIconImageFlag)
                {
                    EditorGUILayout.BeginVertical(redBoxStyle);
                    GUILayout.Space(2);
                    EditorGUILayout.LabelField("Cant save icons! At least in one icon record is something wrong", redTextStyle);
                    GUILayout.Space(2);
                    EditorGUILayout.EndVertical();
                }
                else if (GUILayout.Button("SAVE ICONS (takes some time, recompile etc)", GUILayout.Height(40)))
                {
                    SaveDrawableResources();
                }
                GUILayout.Space(12);

                

                GUILayout.Space(6);
                for (int i = 0; i < frameworkValues.NotificationsData.Length; i++)
                {
                    GUILayout.Space(12);
                    EditorGUILayout.LabelField("Notification " + frameworkValues.NotificationsData[i].type, EditorStyles.boldLabel);
                    frameworkValues.NotificationsData[i].title = EditorGUILayout.TextField("Title", frameworkValues.NotificationsData[i].title);
                    frameworkValues.NotificationsData[i].message = EditorGUILayout.TextField("Message", frameworkValues.NotificationsData[i].message);
                    frameworkValues.NotificationsData[i].callback = EditorGUILayout.TextField("Callback", frameworkValues.NotificationsData[i].callback);
                    frameworkValues.NotificationsData[i].smallIconId = EditorGUILayout.TextField("Small icon ID", frameworkValues.NotificationsData[i].smallIconId);
                    frameworkValues.NotificationsData[i].bigIconId = EditorGUILayout.TextField("Large icon ID", frameworkValues.NotificationsData[i].bigIconId);
                }
            }

            GUILayout.Space(12);
            EditorGUILayout.EndVertical();
        }

        private string GetDrawableResourcesDataContainerFromNotificationsSettings()
        {
            var text = File.ReadAllText(notificationSettingsPath);
            var searchText = "\"DrawableResources\": [";
            var index = text.IndexOf(searchText) + searchText.Length;
            var lastIndex = text.IndexOf(']', index);
            return "{" + searchText + text.Substring(index, lastIndex - index) + "]}";
        }

        private void SaveDrawableResources()
        {
            var text = File.ReadAllText(notificationSettingsPath);
            var searchText = "\"DrawableResources\": [";
            var serialized = EditorJsonUtility.ToJson(drawableContainer, true);
            serialized = serialized.Remove(serialized.IndexOf("{"), 1);
            serialized = serialized.Remove(serialized.LastIndexOf("}"), 1);

            var startIndex = text.IndexOf(searchText);
            var lastIndex = text.IndexOf("]", startIndex);
            var toRemove = text.Substring(startIndex, lastIndex - startIndex + 1);
            text = text.Replace(toRemove, serialized);

            text = Regex.Replace(text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            File.WriteAllText(notificationSettingsPath, text);

            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
        }
        
        private void DrawSectionTitle(string title)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(title, sectionTitleStyle);
            EditorGUILayout.Space(10);
        }

        private bool DrawXButton()
        {
            return GUILayout.Button("X", xButtonStyle, GUILayout.Width(20));
        }
    }

    public enum NotificationIconType
    {
        Small,
        Large
    }

    [Serializable]
    public class DrawableResourceDataContainer
    {
        public List<DrawableResourceData> DrawableResources;

        public DrawableResourceDataContainer()
        {
        }

        public DrawableResourceDataContainer(List<DrawableResourceData> drawableResource)
        {
            DrawableResources = drawableResource;
        }
    }

    [Serializable]
    public class DrawableResourceData
    {
        public string Id;
        public NotificationIconType Type;
        public Texture2D Asset;
        
        public DrawableResourceData()
        { }

        public DrawableResourceData(string id, NotificationIconType type)
        {
            Id = id;
            Type = type;
        }
    }
}