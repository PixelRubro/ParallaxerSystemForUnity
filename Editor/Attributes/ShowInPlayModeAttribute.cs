using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace PixelSpark.Parallaxer.InspectorAttributes
{
    /// <summary>
    /// Show the field only if it is in Play Mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class ShowInPlayModeAttribute : CustomAttribute
    {
    }
}
