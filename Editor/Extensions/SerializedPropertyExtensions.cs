using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using PixelSpark.Parallaxer.InspectorAttributes.Utilities;

#if UNITY_EDITOR

namespace PixelSpark.Parallaxer.Extensions
{
    public static class SerializedPropertyExtensions
    {
        private delegate FieldInfo GetFieldInfoAndStaticTypeFromProperty(SerializedProperty aProperty, out Type aType);

        private static GetFieldInfoAndStaticTypeFromProperty m_GetFieldInfoAndStaticTypeFromProperty;

        #region Extensions

        // Author: github.com/arimger
        public static SerializedProperty GetSibiling(this SerializedProperty self, string propertyPath)
        {
            return self.depth == 0 || self.GetParent() == null
                ? self.serializedObject.FindProperty(propertyPath)
                : self.GetParent().FindPropertyRelative(propertyPath);
        }

        // Author: github.com/arimger
        public static SerializedProperty GetParent(this SerializedProperty self)
        {
            if (self.depth == 0)
            {
                return null;
            }

            var path = self.propertyPath.Replace(".Array.data[", "[");
            var elements = path.Split('.');

            SerializedProperty parent = null;

            for (var i = 0; i < elements.Length - 1; i++)
            {
                var element = elements[i];
                var index = -1;
                if (element.Contains("["))
                {
                    index = Convert.ToInt32(element
                        .Substring(element.IndexOf("[", StringComparison.Ordinal))
                        .Replace("[", "").Replace("]", ""));
                    element = element
                        .Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                }

                parent = i == 0 ? self.serializedObject.FindProperty(element) : parent.FindPropertyRelative(element);

                if (index >= 0) parent = parent.GetArrayElementAtIndex(index);
            }

            return parent;
        }

        // Author: github.com/lordofduct
        /// <summary>
        /// Gets the object that the property is a member of
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static object GetTargetObjectWithProperty(this SerializedProperty self)
        {
            string path = self.propertyPath.Replace(".Array.data[", "[");
            object obj = self.serializedObject.targetObject;
            string[] elements = path.Split('.');

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = HelperMethods.GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = HelperMethods.GetValue_Imp(obj, element);
                }
            }

            return obj;
        }

        // Author: github.com/lordofduct
        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(this SerializedProperty self)
        {
            if (self == null)
            {
                return null;
            }

            string path = self.propertyPath.Replace(".Array.data[", "[");
            object obj = self.serializedObject.targetObject;
            string[] elements = path.Split('.');

            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = HelperMethods.GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = HelperMethods.GetValue_Imp(obj, element);
                }
            }

            return obj;
        }

        public static Type GetPropertyType(this SerializedProperty self)
        {
            Type parentType = self.GetTargetObjectWithProperty().GetType();
            FieldInfo fieldInfo = parentType.GetField(self.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return fieldInfo.FieldType;
        }

        // Origin: https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/
        public static FieldInfo GetFieldInfoAndStaticType(this SerializedProperty self, out Type type)
        {
            if (m_GetFieldInfoAndStaticTypeFromProperty == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assembly.GetTypes())
                    {
                        if (t.Name == "ScriptAttributeUtility")
                        {
                            MethodInfo mi = t.GetMethod("GetFieldInfoAndStaticTypeFromProperty", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                            m_GetFieldInfoAndStaticTypeFromProperty = (GetFieldInfoAndStaticTypeFromProperty)Delegate.CreateDelegate(typeof(GetFieldInfoAndStaticTypeFromProperty), mi);
                            break;
                        }
                    }
                    if (m_GetFieldInfoAndStaticTypeFromProperty != null) break;
                }
                if (m_GetFieldInfoAndStaticTypeFromProperty == null)
                {
                    UnityEngine.Debug.LogError("GetFieldInfoAndStaticType::Reflection failed!");
                    type = null;
                    return null;
                }
            }
            return m_GetFieldInfoAndStaticTypeFromProperty(self, out type);
        }

        public static T GetAttribute<T>(this SerializedProperty self) where T : class
        {
			return self.GetAttributes<T>()?.FirstOrDefault();
        }

        public static T[] GetAttributes<T>(this SerializedProperty self) where T : class
        {
            var targetObj = self.GetTargetObjectWithProperty();
			FieldInfo targetField = targetObj.GetField(self.name);

            if (targetField == null)
                return null;

			return (T[])targetField.GetCustomAttributes(typeof(T), true);
        }

        #endregion

        #region Helper methods

        #endregion
    }
}
#endif