using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftBoiledGames.Parallaxer.Helpers;

namespace SoftBoiledGames.Parallaxer
{
    public class ParallaxStaticElement : ParallaxElement
    {
        #region Protected methods

        #region Overriden methods

        public override void Move(Vector2 displacement, EDirection direction)
        {
            base.Transform.position += (Vector3) displacement;
        }
        
        #endregion

        #endregion  
    }

}