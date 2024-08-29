using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit.ToolkitEditor
{
    [CustomPropertyDrawer(typeof(ValueSetting<>), true)]
    public class ScriptableSettingDrawer : PropertyDrawer
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            PropertyField valueField = new PropertyField(property.FindPropertyRelative("_value"));
            valueField.label = property.name;
            container.Add(valueField);

            return (container);
        }
    }

    [CustomPropertyDrawer(typeof(ObjectSetting<>), true)]
    public class ScriptableObjectSettingDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            PropertyField valueField = new PropertyField(property.FindPropertyRelative("_typeValue"));
            valueField.label = property.name;
            container.Add(valueField);

            return (container);
        }
    }

    [CustomPropertyDrawer(typeof(EnumSetting<>), true)]
    public class ScriptableEnumSettingDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            PropertyField valueField = new PropertyField(property.FindPropertyRelative("_typeValue"));
            valueField.label = property.name;
            container.Add(valueField);

            return (container);
        }
    }
}



