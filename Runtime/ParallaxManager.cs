using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using PixelSpark.Parallaxer.InspectorAttributes;
#endif
using PixelSpark.Parallaxer.Helpers;

namespace PixelSpark.Parallaxer
{
    public class ParallaxManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        [Tooltip("Controls whether the camera will be automatically set to the camera set as the Main Camera.")]
        private bool _useMainCamera = true;

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.HideIf(nameof(_useMainCamera))]
#endif
        [Tooltip("Camera used to calculate the parallax speed.")]
        private Camera _targetCamera;

        [SerializeField]
        [Tooltip("Show debug fields.")]
        private bool _debugMode;

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.ShowIf(nameof(_debugMode))]
#endif
        private Transform _mainCameraTransform;

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.ShowIf(nameof(_debugMode))]
#endif
        private Vector3 _previousCameraPosition;

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.ShowIf(nameof(_debugMode))]
#endif
        private Vector3 _currentCameraPosition;

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.ShowIf(nameof(_debugMode))]
#endif
        private float _currentCameraHorizontalDisplacement;

        #endregion

        #region Non-Serialized Fields

        private Vector3 _currentCameraDisplacement;

        private EDirection _cameraMovementDirection;

        private float _lowestZvalueAvailable;

        private float _screenAspect;

        private List<ParallaxElement> _parallaxElements = new List<ParallaxElement>();

        #endregion

        #region Properties

        internal EDirection MovementDirection => _cameraMovementDirection;

        internal float CameraLeftmostHorizontalPoint => TargetCameraBounds().min.x;

        internal float CameraRightmostHorizontalPoint => TargetCameraBounds().max.x;

        internal Bounds CameraBounds => TargetCameraBounds();

        #endregion

        #region Custom structures
        #endregion

        #region Unity Methods

        private void Awake()
        {
            ReferenceComponents();
        }

        private void Start()
        {
            TrackChildrenParallaxElements();
            SetupInitialValues();
            FindFurthestObject();
        }

        private void Update()
        {
            CalculateCameraDisplacement();
            MoveChildrenParallaxObjects();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Internal methods

        internal float GetLowestZAxisValueAvailable()
        {
            return _lowestZvalueAvailable;
        }

        #endregion

        #region Private Methods

        private void TrackChildrenParallaxElements()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var element = transform.GetChild(i).GetComponent<ParallaxElement>();

                if (element != null)
                {
                    _parallaxElements.Add(element);
                }
                else
                {
                    transform.GetChild(i).SetParent(null, false);
                }
            }
        }

        private void CalculateCameraDisplacement()
        {
            var currentCameraPosition = _mainCameraTransform.position;
            _currentCameraPosition = currentCameraPosition;

            if (_currentCameraPosition.x > _previousCameraPosition.x)
            {
                _cameraMovementDirection = EDirection.Right;
            }
            else if (_currentCameraPosition.x < _previousCameraPosition.x)
            {
                _cameraMovementDirection = EDirection.Left;
            }
            else
            {
                _cameraMovementDirection = EDirection.None;
            }

            _currentCameraDisplacement = currentCameraPosition - _previousCameraPosition;
            _currentCameraHorizontalDisplacement = currentCameraPosition.x - _previousCameraPosition.x;
            _previousCameraPosition = currentCameraPosition;
        }

        private void MoveChildrenParallaxObjects()
        {
            foreach (var p in _parallaxElements)
            {
                p.Move(_currentCameraDisplacement, _cameraMovementDirection);
            }
        }

        private void FindFurthestObject()
        {
            _lowestZvalueAvailable = MathLibrary.Abs(Mathf.Floor(_mainCameraTransform.position.z + 1f));
        }

        private void ReferenceComponents()
        {
            if (_useMainCamera)
            {
                _targetCamera = Camera.main;
            }

            _mainCameraTransform = Camera.main.transform;
        }

        private Bounds TargetCameraBounds()
        {
            if (_targetCamera == null)
            {
                return new Bounds(Vector2.zero, Vector2.zero);
            }

            float cameraHeight = _targetCamera.orthographicSize * 2;
            Vector3 boundsCenter = _targetCamera.transform.position;
            Vector3 boundsSize = new Vector3(cameraHeight * _screenAspect, cameraHeight, 0f);
            return new Bounds(boundsCenter, boundsSize);
        }

        private void SetupInitialValues()
        {
            _previousCameraPosition = _mainCameraTransform.position;
            _currentCameraPosition = _mainCameraTransform.position;
            _screenAspect = (float) Screen.width / (float) Screen.height;
        }

        #endregion
    }
}
