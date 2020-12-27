using System.Collections.Generic;
using UnityEngine;
using YoukaiFox.Math;
using YoukaiFox.UnityExtensionMethods;

namespace YoukaiFox.Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        #region Static fields

        public static ParallaxManager Instance;

        #endregion

        #region Serialized Fields

        [SerializeField] [ReorderableList] [ReadOnlyField]
        private List<ParallaxElement> _parallaxElements;

        #endregion

        #region Non-Serialized Fields

        private Camera _mainCamera;
        private Transform _mainCameraTransform;
        private Vector3 _previousCameraPosition;
        private Vector3 _currentCameraPosition;
        private Vector3 _currentCameraDisplacement;
        private Direction.Directions _cameraMovementDirection;
        private float _lowestZvalueAvailable;
        private int _centralSortOrderIndex;

        #endregion

        #region Properties

        public Vector3 CurrentCameraPosition => _currentCameraPosition;
        public Vector3 CurrentCameraDisplacement => _currentCameraDisplacement;
        public Direction.Directions MovementDirection => _cameraMovementDirection;
        public int CentralSortOrderIndex => _centralSortOrderIndex;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetupSingleton();
            ReferenceComponents();
        }

        private void Start()
        {
            SortChildrenElements();
            ConfigureSortingOrder();
            _previousCameraPosition = _mainCameraTransform.position;
            _currentCameraPosition = _mainCameraTransform.position;
            CalculateLowestZAxisValueAvailable();
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

        public float GetLowestZAxisValueAvailable()
        {
            return _lowestZvalueAvailable;
        }

        #endregion

        #region Private Methods

        private void SortChildrenElements()
        {
            _parallaxElements.Sort();
        }

        private void ConfigureSortingOrder()
        {
            bool foundCentralSortIndex = false;

            for (int i = 0; i < _parallaxElements.Count; i++)
            {
                if (_parallaxElements[i].GetPlane() == ParallaxLayeredElement.Plane.Background)
                {
                    _centralSortOrderIndex = i + 1;
                }
                else
                {
                    if (!foundCentralSortIndex)
                    {
                        foundCentralSortIndex = true;
                        _centralSortOrderIndex = ++i;
                    }
                }

                _parallaxElements[i].SetSortingOrder(i);
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

        private void CalculateLowestZAxisValueAvailable()
        {
            _lowestZvalueAvailable = YoukaiMath.Abs(Mathf.Floor(_mainCameraTransform.position.z + 1f));
        }

        private void ReferenceComponents()
        {
            _mainCamera = Camera.main;
            _mainCameraTransform = Camera.main.transform;
        }
        
        private void SetupSingleton()
        {
            if (Instance == null)
                Instance = this;
            else
                throw new System.Exception();
        }

        #endregion
    }
}
