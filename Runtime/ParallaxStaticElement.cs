using UnityEngine;
using PixelRouge.Parallaxer.Helpers;

namespace PixelRouge.Parallaxer
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