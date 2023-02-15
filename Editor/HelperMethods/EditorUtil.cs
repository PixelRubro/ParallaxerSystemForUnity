#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using PixelSpark.Parallaxer.Extensions;

namespace PixelSpark.Parallaxer.InspectorAttributes.Utilities
{
    public static class EditorUtil
    {
        public static bool IsButtonVisible(UnityEngine.Object target, MethodInfo method)
        {
            var showIfAttribute = Attribute.GetCustomAttribute(method, typeof(ShowIfAttribute)) as ShowIfAttribute;

            if (showIfAttribute == null)
            {
                return true;
            }

            if (showIfAttribute != null)
            {
                bool showingCondition = true;

                if (showIfAttribute.TargetConditionValue != null)
                {
                    var hasCondition = showIfAttribute.TargetConditionValue.ToBool(out bool conditionValue);
                    showingCondition = !hasCondition || conditionValue;
                }

                var fieldValue = (bool) target.GetField(showIfAttribute.PropertyName).GetValue(target);

                if (showingCondition == fieldValue)
                    return true;
            }

            return false;
        }

        public static List<bool> GetConditionValues(object target, string[] conditions)
        {
            List<bool> conditionValues = new List<bool>();

            foreach (var condition in conditions)
            {
                FieldInfo conditionField = target.GetField(condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    conditionValues.Add((bool)conditionField.GetValue(target));
                }

                PropertyInfo conditionProperty = target.GetProperty(condition);
                if (conditionProperty != null &&
                    conditionProperty.PropertyType == typeof(bool))
                {
                    conditionValues.Add((bool)conditionProperty.GetValue(target));
                }

                MethodInfo conditionMethod = target.GetMethod(condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
                }
            }

            return conditionValues;
        }

        public static bool GetConditionsFlag(List<bool> conditionValues)
        {
            bool flag = false;

            foreach (var value in conditionValues)
            {
                flag = flag && value;
            }

            return flag;
        }
    }
}
#endif