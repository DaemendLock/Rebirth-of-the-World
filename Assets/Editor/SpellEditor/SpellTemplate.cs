using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.SpellEditor
{
    [CustomPropertyDrawer(typeof(SpellEffect), true)]
    public class SpellEffect_Inspector : UnityEditor.PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            // Create drawer UI using C#
            //UnityEngine.UIElements.PopupWindow popup = new UnityEngine.UIElements.PopupWindow();
            //popup.text = "Tire Details";

            Debug.Log(property.type);

            //popup.Add(new PropertyField(property.FindPropertyRelative("m_AirPressure"), "Air Pressure (psi)"));
            //popup.Add(new PropertyField(property.FindPropertyRelative("m_ProfileDepth"), "Profile Depth (mm)"));
            //container.Add(popup);

            // Return the finished UI
            return container;
        }
    }

    /*
    [CustomPropertyDrawer(typeof(SpellData), true)]
    public class SpellData_Inspector : UnityEditor.PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();
            return container;
            FieldInfo[] fields = typeof(SpellData).GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                container.Add(new PropertyField(property.FindPropertyRelative(field.Name), field.Name));
            }

            return container;
        }
    }*/
}
