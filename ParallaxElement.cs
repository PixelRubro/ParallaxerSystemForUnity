using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YoukaiFox.Parallax
{
    public class ParallaxElement : MonoBehaviour
    {
        #region Serialized Fields

        [BeginGroup("Additional values")]
        [SerializeField] [LeftToggle]
        private bool _hardFollow = false;

        [SerializeField] [LeftToggle] [HideIf(nameof(_hardFollow), true)]
        private bool _lockHorizontalMovement = false;

        [SerializeField] [LeftToggle] [HideIf(nameof(_hardFollow), true)]
        private bool _lockVerticalMovement = true;

        [SerializeField] [ShowIf(nameof(_lockVerticalMovement), false)] [LeftToggle]
        private bool _useInitialPositionAsLowestY = true;

        [BeginGroup("Debug")]
        [SerializeField] [LeftToggle]
        private bool _debugMode;

        [SerializeField] [LeftToggle]
        private bool _maintainZPosition = true;

        #endregion

        #region Non-Serialized Fields
        [SerializeField] [ShowIf(nameof(_debugMode), true)] [LeftToggle]
        private bool _updateSpeedInPlayMode;

        [SerializeField] [ReadOnlyField] [ShowIf(nameof(_debugMode), true)] [Label("Parallax speed")]
        private float _speed = 1f;

        [SerializeField] [ReadOnlyField] [ShowIf(nameof(_debugMode), true)] [Label("Initial position")]
        private Vector3 _initialPosition;
        [EndGroup]
        private float _previousZValue;
        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        public ParallaxManager _parallaxManager;
        #endregion

        #region Properties
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public float Speed => _speed;
        public Transform Transform => _transform;
        public ParallaxManager ParallaxManager => _parallaxManager;
        public Vector3 InitialPosition => _initialPosition;
        #endregion

        #region Unity Methods

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
            _previousZValue = _transform.position.z;
        }

        private void Start() 
        {
            _initialPosition = _transform.position;
            CalculateSpeed();
            Initialize();
        }

        private void LateUpdate() 
        {
            LateLoop();
        }

        #endregion

        #region Virtual methods

        protected virtual void Initialize()
        {
            if (!_parallaxManager)
            {
                _parallaxManager = FindObjectOfType<ParallaxManager>();

                if (!_parallaxManager)
                {
                    Debug.LogError($"Error! Element {gameObject} is not assigned to a parallax manager!");
                    return;
                }
            }

            if (_hardFollow)
            {
                _lockHorizontalMovement = false;
                _lockVerticalMovement = false;
                _useInitialPositionAsLowestY = true;
            }
        }

        protected virtual void LateLoop()
        {
            UpdateSpeed();
            MoveParallax();
        }

        #endregion

        #region Public Methods

        public void SetManager(ParallaxManager parallaxManager)
        {
            _parallaxManager = parallaxManager;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            if (!_spriteRenderer) return;
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public Transform GetTransform() { return _transform; }

        #endregion

        #region Protected methods

        protected void Move(Vector3 nextPosition)
        {
            if (_lockHorizontalMovement)
                nextPosition.x = _transform.position.x;

            if (_lockVerticalMovement)
            {
                nextPosition.y = _transform.position.y;
            }
            else
            {
                if ((_useInitialPositionAsLowestY) && (nextPosition.y < _initialPosition.y))
                    nextPosition.y = _initialPosition.y;
            }

            if (_maintainZPosition)
                nextPosition.z = _transform.position.z;

            _transform.position = nextPosition;
        }

        #endregion

        #region Private Methods

        private void CalculateSpeed()
        {
            if (_transform.position.z == 0)
            {
                print($"The {gameObject} has position of zero in the Z axis, please change this value.");
                return;
            }

            _speed = Mathf.Abs(1f / _transform.position.z);
        }

        private void UpdateSpeed()
        {
            if ((!_updateSpeedInPlayMode) || (!_debugMode))
                return;

            if (_transform.position.z == _previousZValue)
                return;

            CalculateSpeed();
            _previousZValue = _transform.position.z;
        }

        private void MoveParallax()
        {
            if (_hardFollow)
            {
                Move(_transform.position + _parallaxManager.CurrentCameraDisplacement);
            }
            else
            {
                Move(_transform.position - _parallaxManager.CurrentCameraDisplacement * _speed);
            }
        }

        #endregion
    }

}
