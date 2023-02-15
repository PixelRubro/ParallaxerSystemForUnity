using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSpark.Parallaxer.Helpers
{
    public static class Extensions
    {
        // Author: Phillip Pierce (Adapted)
        /// <summary>
        /// Returns true if game object's layer is present in the given layer mask.
        /// </summary>
        public static bool IsInLayerMask(this GameObject self, LayerMask mask) 
        {
            return ((mask.value & (1 << self.layer)) > 0);
        }

        /// <summary>
        /// Looks for a component of given type in objects directly
        /// higher up in the hierarchy.
        /// </summary>
        /// <typeparam name="T">Unity engine component.</typeparam>
        /// <returns>Component of given type if found, null if otherwise.</returns>
        public static T GetComponentInAncestors<T>(this GameObject self) where T : Component
        {
            Transform currentAncestor = self.transform.parent;

            while (currentAncestor != null)
            {
                var targetComponent = currentAncestor.gameObject.GetComponent<T>();

                if (targetComponent != null)
                {
                    return targetComponent;
                }

                currentAncestor = currentAncestor.parent;
            }

            return null;
        }
    }
}
