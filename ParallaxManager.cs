using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoukaiFox.UnityExtensionMethods;

namespace YoukaiFox.Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] [ReorderableList]
        private List<ParallaxElement> _parallaxElements;

        #endregion

        #region Non-Serialized Fields

        private Camera _mainCamera;
        private Transform _mainCameraTransform;
        private Vector3 _previousCameraPosition;
        private Vector3 _currentCameraPosition;
        private Vector3 _currentCameraDisplacement;
        private Direction.Directions _cameraMovementDirection;

        #endregion

        #region Properties

        public Vector3 CurrentCameraPosition => _currentCameraPosition;
        public Vector3 CurrentCameraDisplacement => _currentCameraDisplacement;
        public Direction.Directions MovementDirection => _cameraMovementDirection;
        public int HighestSortingOrder = 5;

        #endregion

        #region Unity Methods

        private void Awake() 
        {
            _mainCamera = Camera.main;
            _mainCameraTransform = Camera.main.transform;
            AssignManagedElements();
        }

        private void Start() 
        {
            _previousCameraPosition = _mainCameraTransform.position;
            _currentCameraPosition = _mainCameraTransform.position;
        }

        private void Update() 
        {
            UpdateCameraPosition();
            UpdateCameraDisplacement();
            UpdateCameraMovementDirection();
            UpdatePreviousCameraPosition();
        }

        #endregion

        #region Public Methods

        public Bounds GetCameraBounds()
        {
            return _mainCamera.OrthographicBounds();
        }

        #endregion

        #region Private Methods

        private void AssignManagedElements()
        {
            for (int i = 0; i < _parallaxElements.Count; i++)
            {
                _parallaxElements[i].SetManager(this);
                _parallaxElements[i].SetSortingOrder(i);
                HighestSortingOrder = i;
            }
        }

        private void UpdateCameraPosition()
        {
            _currentCameraPosition = _mainCameraTransform.position;
        }

        private void UpdateCameraDisplacement()
        {
            _currentCameraDisplacement = _currentCameraPosition - _previousCameraPosition;
        }

        private void UpdateCameraMovementDirection()
        {
            if (_currentCameraPosition.x > _previousCameraPosition.x)
            {
                _cameraMovementDirection = Direction.Directions.Right;
            }
            else if (_currentCameraPosition.x < _previousCameraPosition.x)
            {
                _cameraMovementDirection = Direction.Directions.Left;
            }
            else
            {
                _cameraMovementDirection = Direction.Directions.None;
            }
        }

        private void UpdatePreviousCameraPosition()
        {
            _previousCameraPosition = _currentCameraPosition;
        }

        #endregion
    }

}
