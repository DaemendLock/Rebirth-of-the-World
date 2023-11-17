using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Core.Combat.Abilities.SpellEffects;
using System.Reflection;

namespace Editor.SpellEditor.Editor
{
    public class SpellEffectEditor : EditorWindow
    {
        private VisualElement _propertiesRoot;
        private EnumField _effect;

        [MenuItem("Window/SpellLibrary/SpellEffectEditor")]
        public static void ShowExample()
        {
            SpellEffectEditor wnd = GetWindow<SpellEffectEditor>();
            wnd.titleContent = new GUIContent("Spell Effect Editor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/SpellEditor/Editor/SpellEffectEditor.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            _propertiesRoot = root.Q<VisualElement>("PropertyContainer");
            _effect = root.Q<EnumField>("Effect");

            LoadEffectProperties(_effect.value);

            _effect.RegisterValueChangedCallback((@event) => LoadEffectProperties(@event.newValue));
        }

        public void OnDestroy()
        {
            _effect.UnregisterValueChangedCallback((@event) => LoadEffectProperties(@event.newValue));
        }

        private void LoadEffectProperties(Enum value)
        {
            Type type = Type.GetType($"{typeof(SpellEffect).Namespace}.{value}, {typeof(SpellEffect).Assembly}");

            if (type == null)
            {
                Debug.LogError("Can't load spell effect type: " + value);
                return;
            }

            _propertiesRoot.Clear();

            FieldInfo[] args = GetFileds(type);

            if (args == null)
            {
                Debug.LogError("Can't load spell effect type: " + value + ". No constructor found");
                return;
            }

            foreach (FieldInfo arg in args)
            {
                
            }
        }

        private FieldInfo[] GetFileds(Type type)
        {
            return type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }

    

    public enum SpellEffectType
    {
        Dummy,
        ApplyAura,
        GiveResource,
        Heal,
        ReduceCooldown,
        SchoolDamage,
        TriggerSpell
    }
}