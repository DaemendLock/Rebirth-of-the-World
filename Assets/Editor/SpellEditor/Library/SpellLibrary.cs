using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.DataTypes;

namespace Editor.SpellEditor.Windows
{
    public static class SpellLibrary
    {

    }

    public class SpellLibraryWindow : EditorWindow
    {
        private static readonly string _path = "Assets/Editor/SpellEditor/Library/SpellLibrary.uxml";

        private static VisualTreeAsset _library;
        private ToolbarSearchField _searchField;
        private VisualElement _createButton;

        [MenuItem("Window/Spell Library/Library")]
        private static void ShowLibrary()
        {
            SpellLibraryWindow window = GetWindow<SpellLibraryWindow>();
            window.titleContent = new GUIContent("Spell Library");
        }

        private void CreateGUI()
        {
            _library = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path);

            if (_library == null)
            {
                return;
            }

            _library.CloneTree(rootVisualElement);

            _searchField = rootVisualElement.Q<ToolbarSearchField>();
            _searchField.RegisterValueChangedCallback(FilterBySearch);
            _createButton = rootVisualElement.Q<VisualElement>("CreateSpell");
            //_createButton.RegisterCallback<ClickEvent>();
        }

        private void OnDisable()
        {
            _searchField.RegisterValueChangedCallback(FilterBySearch);
        }

        private void ShowSpells()
        {

        }

        private void CreateSpellCard(SpellId id)
        {

        }

        private void FilterBySearch(ChangeEvent<string> @event)
        {

        }
    }
}