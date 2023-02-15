using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PixelSpark.Parallaxer.InspectorAttributes.Utilities
{
    public class HelperMethods 
    {
        // Author: github.com/lordofduct
        public static object GetValue_Imp(object obj, string name)
        {
            if (obj == null)
                return null;
            var type = obj.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(obj);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(obj, null);

                type = type.BaseType;
            }
            return null;
        }

        // Author: github.com/lordofduct
        public static object GetValue_Imp(object obj, string name, int index)
        {
            var enumerable = GetValue_Imp(obj, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }

        public static Type GetListElementType(Type listType)
        {
            if (listType.IsGenericType)
            {
                return listType.GetGenericArguments()[0];
            }
            else
            {
                return listType.GetElementType();
            }
        }
    }
}