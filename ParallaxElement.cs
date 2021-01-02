using System;
using System.Collections.Generic;
using UnityEngine;
using YoukaiFox.Math;

namespace YoukaiFox.Parallax
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ParallaxElement : MonoBehaviour, IComparable<ParallaxElement>
    {
        #region Serialized Fields

        [SerializeField] [LeftToggle] [BeginGroup("Constraints")] [EndGroup]
        private bool _preventMovingBelowInitialPos = true;

        #endregion

        #region Non-Serialized Fields

        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private Vector3 _initialPosition;
        private float _previousZValue;

        #endregion

        #region Properties

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => _transform;
        public Vector3 InitialPosition => _initialPosition;

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

        private void Update() 
        {
            OnUpdateEnter();
        }

        private void LateUpdate() 
        {
            OnLateUpdateEnter();
        }

        #endregion

        #region Public Methods

        public ParallaxLayeredElement.Plane GetPlane()
        {
            if (GetComponent<ParallaxStaticElement>())
                return ParallaxLayeredElement.Plane.Background;

            var layeredElement = GetComponent<ParallaxLayeredElement>();

            if (!layeredElement)
                throw new System.Exception();

            return layeredElement.ElementPlane;
        }

        public void ChangeInitialPosition(Vector3 newPosition)
        {
            _initialPosition = newPosition;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            if (!_spriteRenderer) return;
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public int CompareTo(ParallaxElement other)
        {
            if (_transform.position.z > other.transform.position.z)
                return -1;
            else if (_transform.position.z < other.transform.position.z)
                return 1;
            else
                return 0;
        }

        #endregion

        #region Protected methods

        #region Abstract methods

        protected abstract Vector3 CalculateNextPosition();

        #endregion 

        #region Virtual methods

        protected virtual void Initialize()
        {
            Setup();
        }

        protected virtual void Move(Vector3 nextPosition)
        {
            if ((_preventMovingBelowInitialPos) && (nextPosition.y < _initialPosition.y))
                nextPosition.y = _initialPosition.y;

            _transform.position = nextPosition;
        }

        protected virtual void OnUpdateEnter() {}

        protected virtual void ReferenceComponents()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
        }

        protected virtual void Setup()
        {
            if (!_transform.parent.GetComponent<ParallaxManager>())
            {
                string errorMsg = "Place the object containing this elements as a child of a Parallax Manager.";
                Debug.LogError($"Error! Element {gameObject} is not assigned to a parallax manager! {errorMsg}");
                throw new System.Exception();
            }

            if (!_spriteRenderer.sprite)
            {
                string errorMsg = "A parallax element needs a sprite to function properly.";
                Debug.LogError($"Error! The Sprite Renderer of {gameObject} is missing the Sprite! {errorMsg}");
                throw new System.Exception();
            }

            if (_transform.position.z == 0f)
            {
                string errorMsg = "\nA parallax element must have a value other than zero on the Z axis.";
                errorMsg += "\nSet a value greater than zero for background elements ";
                errorMsg += "and lesser than zero for foreground elements.";
                Debug.LogError($"Error! The element {gameObject} Z axis value is zero! {errorMsg}");
                throw new System.Exception();
            }

            _initialPosition = _transform.position;
        }

        protected virtual void OnLateUpdateEnter()
        {
            Vector3 nextPosition = CalculateNextPosition();
            Move(nextPosition);
        }

        #endregion

        #endregion

        #region Private methods
        #endregion
    }
}
