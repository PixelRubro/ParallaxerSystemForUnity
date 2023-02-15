using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using PixelSpark.Parallaxer.InspectorAttributes;
#endif
using PixelSpark.Parallaxer.Helpers;

namespace PixelSpark.Parallaxer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ParallaxElement : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        [Tooltip("Prevents the object from moving below its initial position.")]
#if UNITY_EDITOR
        [LeftToggle]
#endif
        private bool _preventMovingBelowInitialPosition = true;

        [SerializeField]
        [Tooltip("The parallax moving speed depends on the plane that it is placed in.")]
        private ParallaxPlane _plane;

        #endregion

        #region Non-Serialized Fields

        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private Vector3 _initialPosition;
        private float _previousZValue;
        private ParallaxManager _manager;

        #endregion

        #region Properties

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;
        protected Transform Transform => _transform;
        protected Vector3 InitialPosition => _initialPosition;
        protected ParallaxPlane Plane => _plane;
        protected ParallaxManager Manager => _manager;
        protected bool PreventMovingBelowInitialPos => _preventMovingBelowInitialPosition;

        #endregion

        #region Unity Methods

        private void Awake() 
        {
            ReferenceComponents();
        }

        private void Start() 
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void ChangeInitialPosition(Vector3 newPosition)
        {
            _initialPosition = newPosition;
        }

        #endregion

        #region Internal methods

        internal abstract void Move(Vector2 displacement, EDirection direction);

        #endregion

        #region Protected methods

        #region Virtual methods

        protected virtual void Initialize()
        {
            Setup();
        }

        protected virtual void ReferenceComponents()
        {
            _manager = gameObject.GetComponentInAncestors<ParallaxManager>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
        }

        protected virtual void Setup()
        {
            if (!_transform.parent.GetComponent<ParallaxManager>())
            {
                string errorMsg = "Place the object containing this elements as a child of a Parallax Manager.";
                Debug.LogError($"Error! Element {gameObject} is not assigned to a parallax manager! {errorMsg}");
                throw new Exception();
            }

            if (!_spriteRenderer.sprite)
            {
                string errorMsg = "A parallax element needs a sprite to function properly.";
                Debug.LogError($"Error! The Sprite Renderer of {gameObject} is missing the Sprite! {errorMsg}");
                throw new Exception();
            }

            _initialPosition = _transform.position;
        }

        #endregion

        #endregion

        #region Private methods
        #endregion
    }
}
