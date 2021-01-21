using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoukaiFox.Parallax
{
    public class ParallaxStaticElement : ParallaxElement
    {
        #region Protected methods

        #region Overriden methods
        protected override Vector3 CalculateNextPosition()
        {
            if (!ParallaxManager.Instance)
                return Vector3.zero;
                
            return base.Transform.position + ParallaxManager.Instance.CurrentCameraDisplacement;
        }
        
        #endregion

        #endregion  
    }

}