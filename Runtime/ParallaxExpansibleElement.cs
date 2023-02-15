using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelSpark.Parallaxer.Helpers;

namespace PixelSpark.Parallaxer
{
    public class ParallaxExpansibleElement : ParallaxElement
    {
        #region Serialized fields

        [SerializeField]
        [Tooltip("Object moving speed relative to the camera displacement.")]
        [Range(0f, 1f)]
        private float _relativeSpeed = 0.2f;

        [SerializeField]
        [Tooltip("Cache the speed calculation result to improve performance. Check this when you are done adjusting the speed.")]
#if UNITY_EDITOR
        [InspectorAttributes.LeftToggle]
#endif
        private bool _cacheSpeed = false;

        [SerializeField] 
#if UNITY_EDITOR
        [InspectorAttributes.LeftToggle]
#endif
        private bool _preventHorizontalMovement = false;

        [SerializeField] 
#if UNITY_EDITOR
        [InspectorAttributes.LeftToggle] 
#endif
        private bool _preventVerticalMovement = false;

        private float _parallaxExitDistance = 1f;

        #endregion

        #region Non-serialized fields

        private SpriteRenderer[] _copies;
        
        private float _cachedRendererWidth;

        private float _cachedBackgroundSpeed;

        private float _cachedForegroundSpeed;

        #endregion

        #region Properties

        protected float RelativeSpeed => _relativeSpeed;

        #endregion

        #region Constant fields

        private const int CopiesQuantity = 3;

        #endregion

        #region Custom structures

        private enum Position
        {
            Left = 0, Central = 1, Right = 2
        }
        #endregion

        #region Unity events

        #endregion

        #region Public methods

        public void SetMovementConstraints(bool horizontal, bool vertical)
        {
            _preventHorizontalMovement = horizontal;
            _preventVerticalMovement = vertical;
        }

        #endregion

        #region Internal methods

        internal override void Move(Vector2 displacement, EDirection direction)
        {
            CheckCameraPosition(displacement, direction);
            var targetPosition = Transform.position;

            if (Plane == ParallaxPlane.Background)
            {
                var speed = _cachedBackgroundSpeed;

                if (_cacheSpeed == false)
                {
                    speed = 1f - _relativeSpeed;
                }

                targetPosition = base.Transform.position + (Vector3) displacement * speed;
            }
            else if (Plane == ParallaxPlane.Foreground)
            {
                var speed = _cachedForegroundSpeed;

                if (_cacheSpeed == false)
                {
                    speed = (1f + _relativeSpeed) * -1f;
                }

                targetPosition = Transform.position + (Vector3) displacement * (speed + 1f);
            }
            else if (Plane == ParallaxPlane.Midground)
            {
                Transform.position += (Vector3) displacement;
            }

            if ((PreventMovingBelowInitialPos) && (targetPosition.y < InitialPosition.y))
            {
                targetPosition.y = InitialPosition.y;
            }

            if (_preventHorizontalMovement == true)
            {
                targetPosition.x = InitialPosition.x;
            }

            if (_preventVerticalMovement == true)
            {
                targetPosition.y = InitialPosition.y;
            }

            Transform.position = targetPosition;
        }

        #endregion

        #region Protected methods

        #region Overridden methods

        protected override void Initialize()
        {
            base.Initialize();
            CreateCopies();
            PlaceChildrenCopies();
            CalculateSpeed();
        }

        #endregion

        #endregion

        #region Private methods

        private void CheckCameraPosition(Vector2 displacement, EDirection direction)
        {
            if (Manager == null)
            {
                return;
            }

            if (Direction.IsHorizontal(direction) == false)
            {
                return;
            }

            var currentDistance = GetDistanceToExitParallax(direction);

            if (MathLibrary.Abs(currentDistance) <= _parallaxExitDistance)
            {
                MoveCopies(direction);
            }
        }

        private void CalculateSpeed()
        {
            _cachedBackgroundSpeed = 1f - _relativeSpeed;
            _cachedForegroundSpeed = (_relativeSpeed + 1f) * 1f;
        }

        private float GetDistanceToExitParallax(EDirection direction)
        {
            var cameraBounds = Manager.CameraBounds;

            if (direction == EDirection.Right)
            {
                var parallaxRightMostPoint = _copies[(int) Position.Right].bounds.max.x;
                return parallaxRightMostPoint - cameraBounds.max.x;
            }
            
            if (direction == EDirection.Left)
            {
                var parallaxLeftMostPoint = _copies[(int) Position.Left].bounds.min.x;
                return parallaxLeftMostPoint - cameraBounds.min.x;
            }

            return 0f;
        }

        private void CreateCopies()
        {
            _cachedRendererWidth = SpriteRenderer.bounds.size.x;
            SpriteRenderer.enabled = false;
            _copies = new SpriteRenderer[CopiesQuantity];
            var dummyObject = new GameObject();
            var spriteRenderer = dummyObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = SpriteRenderer.sortingOrder;
            spriteRenderer.sprite = SpriteRenderer.sprite;
            spriteRenderer.sortingLayerID = SpriteRenderer.sortingLayerID;

            for (var i = 0; i < _copies.Length; i++)
            {
                _copies[i] = Instantiate(spriteRenderer, transform, false);
                _copies[i].gameObject.name = $"Element{i}";
            }

            Destroy(dummyObject);
        }

        private void PlaceChildrenCopies()
        {
            if (_copies.Length <= 0)
            {
                return;
            }

            var bounds = base.SpriteRenderer.bounds;

            _copies[(int) Position.Left].transform.localPosition = new Vector3(
                -bounds.size.x, 
                _copies[(int) Position.Left].transform.localPosition.y, 
                _copies[(int) Position.Left].transform.localPosition.z);

            _copies[(int) Position.Right].transform.localPosition = new Vector3(
                bounds.size.x, 
                _copies[(int) Position.Right].transform.localPosition.y, 
                _copies[(int) Position.Right].transform.localPosition.z);
        }

        private void MoveCopies(EDirection direction)
        {
            if (direction == EDirection.Right)
            {
                var extendedSprite = _copies[(int) Position.Left];
                extendedSprite.transform.localPosition = new Vector3(
                    _copies[(int) Position.Right].transform.localPosition.x + _cachedRendererWidth,
                    extendedSprite.transform.localPosition.y,
                    extendedSprite.transform.localPosition.z
                );

                _copies[(int) Position.Left] = _copies[(int) Position.Central];
                _copies[(int) Position.Central] = _copies[(int) Position.Right];
                _copies[(int) Position.Right] = extendedSprite;
            }
            else if (direction == EDirection.Left)
            {
                var extendedSprite = _copies[(int) Position.Right];
                extendedSprite.transform.localPosition = new Vector3(
                    _copies[(int) Position.Left].transform.localPosition.x - _cachedRendererWidth,
                    extendedSprite.transform.localPosition.y,
                    extendedSprite.transform.localPosition.z
                );

                _copies[(int) Position.Right] = _copies[(int) Position.Central];
                _copies[(int) Position.Central] = _copies[(int) Position.Left];
                _copies[(int) Position.Left] = extendedSprite;
            }
        }

        #endregion
    }
}
