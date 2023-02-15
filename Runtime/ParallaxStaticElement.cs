using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftBoiledGames.Parallaxer.Helpers;

namespace SoftBoiledGames.Parallaxer
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