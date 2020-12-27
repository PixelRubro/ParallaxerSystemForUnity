using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoukaiFox.Parallax
{
    public class ParallaxStaticElement : ParallaxElement
    {
        #region Serialized fields
        #endregion

        #region Non-serialized fields
        #endregion

        #region Unity events
        #endregion

        #region Public methods
        #endregion

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

        #region Private methods
        #endregion    
    }

}