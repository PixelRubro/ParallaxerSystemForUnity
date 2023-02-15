using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using PixelSpark.Parallaxer.Extensions;

namespace PixelSpark.Parallaxer.InspectorAttributes
{
    public abstract class BasePropertyDrawer: PropertyDrawer 
    {
        public virtual float GetHelpBoxHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2.0f;
        }

        protected void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            // Property is hidden, no need to draw.
            if (IsHidden(property))
                return;

            // If has Label and/or Icon attributes, return label value with them added in.
            label = AddLabelAttributes(property, label);

            // Disable property if it should.
            if (HasDisablingAttribute(property))
                EditorGUI.BeginDisabledGroup(true);

            // Check for special cases for drawing, if none are met, draw field as simple as possible.
            if (property.GetAttribute<LeftToggleAttribute>() != null)
                DrawFieldWithToggleOnTheLeft(position, property, label);
            else
                DrawPropertySimple(position, property, label);

            // Restore GUI status if field was disabled.
            if (HasDisablingAttribute(property))
                EditorGUI.EndDisabledGroup();
        }

        protected void DrawErrorMessage(Rect position, string errorMessage)
        {
            var padding = EditorGUIUtility.standardVerticalSpacing;

            var highlightRect = new Rect(position.x - padding, position.y - padding,
                position.width + (padding * 2), position.height + (padding * 2));

            EditorGUI.DrawRect(highlightRect, Color.red);

            var contentColor = GUI.contentColor;
            GUI.contentColor = Color.white;
            EditorGUI.LabelField(position, errorMessage);
            GUI.contentColor = contentColor;
        }

        protected void DrawFieldWithToggleOnTheLeft(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                var message = "ERROR! Not a boolean field.";
                DrawErrorMessage(position, message);
                return;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.ToggleLeft(position, label, property.boolValue);

            if (EditorGUI.EndChangeCheck())
                property.boolValue = value;

            EditorGUI.EndProperty();
        }

        protected Texture2D FindIcon(string iconPath)
        {
            var path = Path.Combine(Application.dataPath, iconPath);
            var image = File.ReadAllBytes(path);

            if (image == null)
                return null;

            var iconTexture = new Texture2D(1, 1);
            iconTexture.LoadImage(image);
            iconTexture.name = "PrefabIcon";
            iconTexture.Apply();
            return iconTexture;
        }

        protected void DrawPropertySimple(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }

        protected void DrawEnumFlagsField(Rect position, SerializedProperty property, GUIContent label)
        {
            Enum targetEnum = property.GetTargetObjectOfProperty() as Enum;

            if (targetEnum == null)
            {
                var message = "ERROR! EnumFlags attribute can only be used on enums.";
                DrawErrorMessage(position, message);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            Enum updatedEnum = EditorGUI.EnumFlagsField(position, label.text, targetEnum);
            property.intValue = (int) Convert.ChangeType(updatedEnum, targetEnum.GetType());
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        protected void ShowNullFieldWarning(SerializedProperty property, string fieldName)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return;

            if (property.objectReferenceValue != null)
                return;

            EditorGUILayout.HelpBox($"The value of the field \"{fieldName}\" cannot be null.", MessageType.Error);
        }

        private bool HasDisablingAttribute(SerializedProperty property)
        {
            var readonlyAttribute = property.GetAttribute<ReadOnlyAttribute>();

            if (readonlyAttribute != null)
                return true;

            var disablePlayModeAttribute = property.GetAttribute<DisableInPlayModeAttribute>();

            if ((disablePlayModeAttribute != null) && (Application.isPlaying))
                return true;

            return false;
        }

        private bool IsHidden(SerializedProperty property)
        {
            var hideInPlayModeAttribute = property.GetAttribute<HideInPlayModeAttribute>();

            if ((hideInPlayModeAttribute != null) && (Application.isPlaying))
                return true;

            return false;
        }

        private GUIContent AddLabelAttributes(SerializedProperty property, GUIContent label)
        {
            return label;
        }
    }
}