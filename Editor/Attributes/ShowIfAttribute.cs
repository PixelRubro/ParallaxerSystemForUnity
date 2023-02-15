namespace PixelSpark.Parallaxer.InspectorAttributes
{
    /// <summary>
    /// Shows property if provided condition is met.
    /// </summary>
    public sealed class ShowIfAttribute : ConditionalAttribute
    {
        /// <summary>
        /// Shows field if <paramref name="comparedPropertyName"/> is true.
        /// </summary>
        public ShowIfAttribute(string comparedPropertyName) : base(comparedPropertyName)
        {
        }

        /// <summary>
        /// Shows field if <paramref name="comparedPropertyName"/>
        /// has a value of <paramref name="targetConditionValue"/>.
        /// </summary>
        public ShowIfAttribute(string comparedPropertyName, object targetConditionValue) : base(comparedPropertyName, targetConditionValue)
        {
        }
    }
}