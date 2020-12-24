using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoukaiFox.Math;

namespace YoukaiFox.Parallax
{
    public class ParallaxExtendableElement : ParallaxElement
    {
        #region Serialized fields

        [SerializeField] private float _parallaxExitDistance = 1.5f;

        #endregion

        #region Properties

        public bool IsActive => _isActive;

        #endregion

        #region Non-serialized fields

        private bool _isActive;
        private SpriteRenderer[] _copies;
        private float _boundsSizeX;

        #endregion

        #region Constant fields

        private const int CopiesQuantity = 3;

        #endregion

        #region Virtual methods

        protected override void Initialize()
        {
            base.Initialize();
            CreateCopies();
            PlaceChildrenCopies();
        }

        protected override void LateLoop()
        {
            base.LateLoop();
            CheckDisplacement();
        }    

        #endregion

        #region Public methods
        #endregion

        #region Private methods

        private void CreateCopies()
        {
            _boundsSizeX = base.SpriteRenderer.bounds.size.x;
            base.SpriteRenderer.enabled = false;
            _copies = new SpriteRenderer[CopiesQuantity];
            GameObject dummyObject = new GameObject();
            SpriteRenderer spriteRenderer = dummyObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = base.SpriteRenderer.sortingOrder;
            spriteRenderer.sprite = base.SpriteRenderer.sprite;

            for (int i = 0; i < _copies.Length; i++)
            {
                _copies[i] = Instantiate(spriteRenderer, base.transform, false);
            }

            Destroy(dummyObject);
        }

        private void PlaceChildrenCopies()
        {
            if (_copies.Length <= 0) 
                return;

            Bounds bounds = base.SpriteRenderer.bounds;

            _copies[0].transform.localPosition = new Vector3(
                -bounds.size.x, 
                _copies[0].transform.localPosition.y, 
                _copies[0].transform.localPosition.z);

            _copies[2].transform.localPosition = new Vector3(
                bounds.size.x, 
                _copies[2].transform.localPosition.y, 
                _copies[2].transform.localPosition.z);
        }

        private void RotateCopies(Direction.Directions direction)
        {
            SpriteRenderer extendedSprite = null;

            if (direction == Direction.Directions.Right)
            {
                extendedSprite = _copies[0];
                extendedSprite.transform.localPosition = new Vector3(
                    _copies[2].transform.localPosition.x + _boundsSizeX,
                    extendedSprite.transform.localPosition.y,
                    extendedSprite.transform.localPosition.z
                );

                _copies[0] = _copies[1];
                _copies[1] = _copies[2];
                _copies[2] = extendedSprite;
            }
            else if (direction == Direction.Directions.Left)
            {
                extendedSprite = _copies[2];
                extendedSprite.transform.localPosition = new Vector3(
                    _copies[0].transform.localPosition.x - _boundsSizeX,
                    extendedSprite.transform.localPosition.y,
                    extendedSprite.transform.localPosition.z
                );

                _copies[2] = _copies[1];
                _copies[1] = _copies[0];
                _copies[0] = extendedSprite;
            }
        }

        private void CheckDisplacement()
        {
            Direction.Directions direction = base.ParallaxManager.MovementDirection;

            if (direction == Direction.Directions.None)
                return;

            Bounds cameraBounds = base.ParallaxManager.GetCameraBounds();
            float currentDistance = 0f;

            if (direction == Direction.Directions.Right)
            {
                currentDistance = _copies[2].bounds.max.x - cameraBounds.max.x;
            }
            else if (direction == Direction.Directions.Left)
            {
                currentDistance = _copies[0].bounds.min.x - cameraBounds.min.x;
            }

            if (YoukaiMath.Abs(currentDistance) <= _parallaxExitDistance)
            {
                RotateCopies(direction);
            }
        }

        #endregion
    }

}