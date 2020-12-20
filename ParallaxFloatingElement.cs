using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace YoukaiFox.Parallax
{
    
    public class ParallaxFloatingElement : ParallaxElement
    {
        #region Serialized fields
        [BeginGroup("Additional values")]
        [SerializeField]
        private FloatingPattern _floatingPattern;

        [SerializeField] [ShowIf(nameof(IsLinear), true)]
        private Direction.Directions _linearMovementDirection;

        // [SerializeField] [LeftToggle]
        // private bool _movesWhenCameraIsStationary = false;

        [SerializeField]
        private float _maxFloatDistance = 0.25f;

        [SerializeField] [Range(0f, 1f)] 
        private float _movementSpeed = 0.5f;

        [EndGroup] [SerializeField] [LeftToggle]
        private bool _changesPositionWhenRedrawn = false;
        #endregion

        #region Non-serialized fields

        private Tween _floatingTween;
        private bool IsLinear => _floatingPattern == FloatingPattern.Linear;

        #endregion

        public enum FloatingPattern
        {
            Static, Random, Linear
        }

        #region Unity events
        #endregion

        #region Virtual methods
        protected override void Initialize()
        {
            base.Initialize();

            if (_floatingPattern == FloatingPattern.Random)
            {
                FloatAround();
            }
        }

        protected override void LateLoop()
        {
            if (_floatingPattern == FloatingPattern.Linear)
            {
                MoveLinearly();
            }
        }

        #endregion

        #region Public methods
        #endregion

        #region Private methods

        private void FloatAround()
        {
            Vector3 randomDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * _maxFloatDistance;
            randomDirection += InitialPosition;
            _floatingTween = base.Transform.DOMove(randomDirection, 101f - _movementSpeed).SetEase(Ease.InSine).OnComplete(FloatAround);
        }

        private void MoveLinearly()
        {
            Vector3 displacement = Vector3.zero;

            switch (_linearMovementDirection)
            {
                case Direction.Directions.Right:
                    displacement.x = _movementSpeed * Time.deltaTime;
                    break;
                case Direction.Directions.Left:
                    displacement.x = -_movementSpeed * Time.deltaTime;
                    break;
                case Direction.Directions.Up:
                    displacement.y = _movementSpeed * Time.deltaTime;
                    break;
                case Direction.Directions.Down:
                    displacement.y = -_movementSpeed * Time.deltaTime;
                    break;
                default:
                    break;
            }

            Move(base.Transform.position += displacement);
        }

        #endregion
    }

}