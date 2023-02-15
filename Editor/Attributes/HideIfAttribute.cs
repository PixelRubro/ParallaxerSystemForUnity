namespace PixelSpark.Parallaxer.InspectorAttributes
{
    /// <summary>
    /// Hides property if provided condition is met.
    /// </summary>
    public sealed class HideIfAttribute : ConditionalAttribute
    {
        /// <summary>
        /// Hides field if <paramref name="comparedPropertyName"/> is true.
        /// </summary>
        public HideIfAttribute(string comparedPropertyName) : base(comparedPropertyName)
        {
        }

        /// <summary>
        /// Hides field if <paramref name="comparedPropertyName"/>
        /// has a value of <paramref name="targetConditionValue"/>.
        /// </summary>
        public HideIfAttribute(string comparedPropertyName, object targetConditionValue) : base(comparedPropertyName, targetConditionValue)
        {
        }
    }
}