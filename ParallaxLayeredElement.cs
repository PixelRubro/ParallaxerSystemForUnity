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

        [SerializeField] [BeginGroup("Debug")] [LeftToggle]
        private bool _debugMode = false;

        [SerializeField] [ShowIf(nameof(_debugMode), true)] [LeftToggle] 
        private bool _updateSpeedInPlayMode;

        #endregion

        #region Non-serialized fields

        [ReadOnlyField] [ShowIf(nameof(_debugMode), true)] [SerializeField] [EndGroup]
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

        public void SetMovementConstraints(bool horizontal, bool vertical, bool zAxis)
        {
            _preventHorizontalMovement = horizontal;
            _preventVerticalMovement = vertical;
            _preventMovementOnZAxis = zAxis;
        }

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
                    return GetBackgroundSpeed();
                case Plane.Foreground:
                    return GetForegroundSpeed();
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        private void RunDebug()
        {
            if (!_debugMode)
                return;

            if (_updateSpeedInPlayMode)
                _parallaxSpeed = CalculateSpeed();
        }

        private float GetBackgroundSpeed()
        {
            return YoukaiMath.Abs(1f / base.Transform.position.z);
        }

        private float GetForegroundSpeed()
        {
            float lowestZavailable = ParallaxManager.Instance.GetLowestZAxisValueAvailable();

            if (lowestZavailable == 0)
                lowestZavailable = 1;

            return YoukaiMath.Abs((base.Transform.position.z) / lowestZavailable);
        }

        #endregion
    }

}