using UnityEditor;
using UnityEngine;

namespace PixelSpark.Parallaxer.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfAttributeDrawer : ConditionalAttributeDrawer
    {
        protected override PropertyDrawing GetPropertyDrawing()
        {
            return PropertyDrawing.Hide;
        }
    }
}