using UnityEditor;
using UnityEngine;

namespace PixelSpark.Parallaxer.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
    public class DisableInPlayModeAttributeDrawer : BasePropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawProperty(position, property, label);
        }
    }
}
