using UnityEngine;
using VermillionVanguard.Parallaxer.Helpers;

namespace VermillionVanguard.Parallaxer
{
    public class ParallaxStaticElement : ParallaxElement
    {
        #region Internal methods

        internal override void Move(Vector2 displacement, EDirection direction)
        {
            Transform.position += (Vector3) displacement;
        }
        
        #endregion  
    }

}