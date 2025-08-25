using System.IO;

using UnityEngine;

namespace UnityEditor.Tooltips
{
    [CustomEditor(typeof(DefaultAsset), isFallback = true)]
    public class FolderDescription : Editor
    {
        public const string DescriptionFileExtensions = ".desc";

        private string _description;
        private bool _isFolder;

        private void OnEnable()
        {
            var path = AssetDatabase.GetAssetPath(target);

            if (Directory.Exists(path))
            {
                _isFolder = true;
                var descriptionPath = Path.Combine(path, DescriptionFileExtensions);

                try
                {
                    _description = File.ReadAllText(descriptionPath);
                }
                catch (IOException) { }
            }
        }

        public override void OnInspectorGUI()
        {
            if (_isFolder == false)
            {
                DrawDefaultInspector();
                return;
            }

            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();

            var descriptionContent = new GUIContent(_description);

            _description = EditorGUILayout.TextArea(_description, Styles.invisibleTextEditor, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));

            if (string.IsNullOrEmpty(_description) && GUIUtility.keyboardControl == 0)
            {
                var rect = GUILayoutUtility.GetLastRect();

                GUI.Label(rect, "Click to add a folder description", EditorStyles.centeredGreyMiniLabel);
            }

            if (EditorGUI.EndChangeCheck() == false)
            {
                return;
            }
            // we get the asset path again instead of caching it in case the folder has moved since OnEnable was called
            var descriptionPath = Path.Combine(AssetDatabase.GetAssetPath(target), DescriptionFileExtensions);

            if (!string.IsNullOrEmpty(_description))
            {
                try
                {
                    File.Delete(descriptionPath);
                }
                catch (IOException) { }

                return;
            }

            try
            {
                File.SetAttributes(descriptionPath, FileAttributes.Normal);
            }
            catch (IOException) { }

            File.WriteAllText(descriptionPath, _description);

            try
            {
                File.SetAttributes(descriptionPath, FileAttributes.Hidden);
            }
            catch (IOException) { }
        }

        private static class Styles
        {
            public static readonly GUIStyle invisibleTextEditor = new GUIStyle("TextArea");

            static Styles()
            {
                var label = EditorStyles.label;
                invisibleTextEditor.normal = new GUIStyleState();
                invisibleTextEditor.normal.background = label.normal.background;
                invisibleTextEditor.normal.textColor = label.normal.textColor;
            }
        }
    }
}