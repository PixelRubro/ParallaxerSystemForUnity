using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PixelSpark.Parallaxer.Extensions
{
    public static class ObjectReflectionExtensions 
    {
        private static BindingFlags StandardFlags = BindingFlags.Instance | 
                                                    BindingFlags.Static | 
                                                    BindingFlags.NonPublic | 
                                                    BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetAllFields(this object self, Func<FieldInfo, bool> predicate)
        {
            if (self == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            List<Type> types = new List<Type>()
            {
                self.GetType()
            };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<FieldInfo> fieldInfos = types[i]
                    .GetFields(StandardFlags)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this object self, Func<PropertyInfo, bool> predicate)
        {
            if (self == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            List<Type> types = new List<Type>()
            {
                self.GetType()
            };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<PropertyInfo> propertyInfos = types[i]
                    .GetProperties(StandardFlags)
                    .Where(predicate);

                foreach (var propertyInfo in propertyInfos)
                {
                    yield return propertyInfo;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(this object self)
        {
            if (self == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            IEnumerable<MethodInfo> methodInfos = self.GetType()
                .GetMethods(StandardFlags);

            return methodInfos;
        }

        public static IEnumerable<MethodInfo> GetAllMethods(this object self, Func<MethodInfo, bool> predicate)
        {
            if (self == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            IEnumerable<MethodInfo> methodInfos = self.GetType()
                .GetMethods(StandardFlags)
                .Where(predicate);

            return methodInfos;
        }

        public static FieldInfo GetField(this object self, string fieldName)
        {
            return GetAllFields(self, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public static PropertyInfo GetProperty(this object self, string propertyName)
        {
            return GetAllProperties(self, p => p.Name.Equals(propertyName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public static MethodInfo GetMethod(this object self, string methodName)
        {
            return GetAllMethods(self, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public static bool GetConditionValue(this object self, string validationPropertyName)
        {
            var conditionField = self.GetField(validationPropertyName);

            if ((conditionField != null) && (conditionField.FieldType == typeof(bool)))
            {
                return (bool) conditionField.GetValue(self);
            }

            var conditionProperty = self.GetProperty(validationPropertyName);

            if ((conditionProperty != null) && (conditionProperty.PropertyType == typeof(bool)))
            {
                return (bool) conditionProperty.GetValue(self);
            }

            var conditionMethod = self.GetMethod(validationPropertyName);

            if ((conditionMethod != null) && (conditionMethod.ReturnType == typeof(bool)) &&
                (conditionMethod.GetParameters().Length == 0))
            {
                return (bool) conditionMethod.Invoke(self, null);
            }

            return false;
        }
    }
}