using System;

namespace VermillionVanguard.Parallaxer.InspectorAttributes
{
    /// <summary>
    /// Make the field read-only when the editor is in play mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class DisableInPlayModeAttribute : CustomAttribute
    {
    }
}
