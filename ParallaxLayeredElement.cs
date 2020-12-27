using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoukaiFox.Math;

namespace YoukaiFox.Parallax
{
    public abstract class ParallaxLayeredElement : ParallaxElement
    {
        #region Serialized fields

        [SerializeField] [LeftToggle] [BeginGroup("Constraints")]
        private bool _preventHorizontalMovement = false;

        [SerializeField] [LeftToggle] 
        private bool _preventVerticalMovement = false;

        [SerializeField] [LeftToggle] [EndGroup]
        private bool _preventMovementOnZAxis = true;

        [SerializeField] [BeginGroup("Values")] [EndGroup]
        private Plane _plane;

        [SerializeField] [BeginGroup("Debug")] [LeftToggle] [EndGroup]
        private bool _debugMode = false;

        [SerializeField] [ShowIf(nameof(_debugMode), true)] [LeftToggle]
        private bool _updateSpeedInPlayMode;
        #endregion

        #region Non-serialized fields

        [ReadOnlyField] [ShowIf(nameof(_debugMode), true)] [Label("Parallax speed")]
        private float _parallaxSpeed = 1f;

        #endregion

        #region Properties

        public float ParallaxSpeed => _parallaxSpeed;
        public Plane ElementPlane => _plane;
        
        #endregion

        #region Custom structures

        public enum Plane
        {
            Background, Foreground
        }

        #endregion

        #region Unity events
        #endregion

        #region Public methods
        #endregion

        #region Protected methods

        #region Overridden methods

        protected override void Initialize()
        {
            base.Initialize();
            _parallaxSpeed = CalculateSpeed();
        }

        protected override void Move(Vector3 nextPosition)
        {
            if (_preventHorizontalMovement)
                nextPosition.x = base.Transform.position.x;

            if (_preventVerticalMovement)
                nextPosition.y = base.Transform.position.y;

            if (_preventMovementOnZAxis)
                nextPosition.z = base.Transform.position.z;

            base.Move(nextPosition);
        }

        protected override void OnUpdateEnter()
        {
            base.OnUpdateEnter();
            RunDebug();
        }

        #endregion
        
        #endregion

        #region Private methods

        private float CalculateSpeed()
        {
            switch (_plane)
            {
                case Plane.Background:
                    return YoukaiMath.Abs(1f / base.Transform.position.z);
                case Plane.Foreground:
                    float lowestZavailable = ParallaxManager.Instance.GetLowestZAxisValueAvailable();
                    return YoukaiMath.Abs((base.Transform.position.z) / lowestZavailable);
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        private void RunDebug()
        {
            if (!_debugMode)
                return;

            if (!_updateSpeedInPlayMode)
                _parallaxSpeed = CalculateSpeed();
        }

        #endregion
    }

}