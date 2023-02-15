using System;

namespace PixelSpark.Parallaxer.InspectorAttributes
{
    /// <summary>
    /// Base class for comparison attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ConditionalAttribute : CustomAttribute
    {
        public string PropertyName = null;
        public System.Object TargetConditionValue = null;

        /// <summary>
        /// Comparison is true if <paramref name="comparedPropertyName"/>
        /// is true.
        /// </summary>
        public ConditionalAttribute(string comparedPropertyName)
        {
            PropertyName = comparedPropertyName;
            TargetConditionValue = null;
        }

        /// <summary>
        /// Comparison is true if <paramref name="comparedPropertyName"/>
        /// has a value of <paramref name="targetConditionValue"/>.
        /// </summary>
        public ConditionalAttribute(string comparedPropertyName, System.Object targetConditionValue)
        {
            PropertyName = comparedPropertyName;
            TargetConditionValue = targetConditionValue;
        }
    }
}