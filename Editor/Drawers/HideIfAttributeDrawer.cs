using UnityEditor;
using UnityEngine;

namespace PixelRouge.Parallaxer.InspectorAttributes
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