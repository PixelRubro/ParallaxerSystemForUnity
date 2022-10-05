using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftBoiledGames.Parallaxer.Helpers;

namespace SoftBoiledGames.Parallaxer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxMovingElement : ParallaxElement
    {
        #region Serialized fields

        [SerializeField]
        private float _movementSpeed = 0.5f;

        [SerializeField] 
        private Vector2 _movementDirection;

        [SerializeField]
        [InspectorAttributes.LeftToggle]
        private bool _respawnsWhenOutOfScreen = true;

        #endregion

        #region Non-serialized fields
        #endregion

        #region Unity events

        private void OnBecameInvisible()
        {
            Respawn();
        }

        #endregion

        #region Public methods

        public override void Move(Vector2 displacement, EDirection direction)
        {
            var movement = _movementSpeed * Time.deltaTime * _movementDirection;
            var aggregatedDisplacement = movement + displacement;
            base.Transform.position += (Vector3) aggregatedDisplacement;
        }

        #endregion

        #region Protected methods

        #region Overridden methods

        protected override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #endregion

        #region Private methods

        private void Respawn()
        {
            if (_respawnsWhenOutOfScreen == false)
            {
                return;
            }

            if (Manager == null)
            {
                return;
            }

            var spriteWidth = base.SpriteRenderer.bounds.size.x;
            
            if (_movementDirection.x > 0f)
            {
                var leftmostPoint = Manager.CameraLeftmostHorizontalPoint;
                var newPosition = new Vector2(leftmostPoint + -spriteWidth, base.Transform.position.y);
                base.Transform.position = newPosition;
            }
            else if (_movementDirection.x < 0f)
            {
                var rightmostPoint = Manager.CameraRightmostHorizontalPoint;
                var newPosition = new Vector2(rightmostPoint + spriteWidth, base.Transform.position.y);
                base.Transform.position = newPosition;
            }
        }

        #endregion
    }
}