using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YoukaiFox.Math;

namespace YoukaiFox.Parallax
{
    public class ParallaxMovingElement : ParallaxLayeredElement
    {
        #region Serialized fields
        [BeginGroup("Additional values")]
        [SerializeField]
        private MovingPattern _movingPattern;

        [SerializeField] [ShowIf(nameof(IsRandom), false)]
        private Direction.Directions _movementDirection;

        [SerializeField]
        private float _randomnessStrength = 0.25f;

        [SerializeField]
        private float _movementSpeed = 0.5f;

        [EndGroup] [SerializeField] [LeftToggle]
        private bool _changesPositionWhenRedrawn = false;
        #endregion

        #region Non-serialized fields

        private Tween _floatingTween;
        private System.Random _random;
        #endregion

        #region Properties

        private bool IsLinear => _movingPattern == MovingPattern.Linear;
        private bool IsRandom => _movingPattern == MovingPattern.Random;

        #endregion

        public enum MovingPattern
        {
            Random, Linear
        }

        #region Unity events
        #endregion

        #region Virtual methods
        protected override void Initialize()
        {
            base.Initialize();
            _random = new System.Random();
        }

        #endregion

        #region Public methods
        #endregion

        #region Private methods

        private void FloatAround()
        {
            Vector3 randomDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * _randomnessStrength;
            randomDirection += InitialPosition;
            _floatingTween = base.Transform.DOMove(randomDirection, 101f - _movementSpeed).SetEase(Ease.InSine).OnComplete(FloatAround);
        }

        private Vector3 MoveLinearly()
        {
            Vector3 displacement = Vector3.zero;

            switch (_movementDirection)
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

            return base.Transform.position += displacement;
        }

        private Vector3 MoveRandomly()
        {
            Vector3 randomDirection = YoukaiMath.GetRandomNormalizedVector2();
            randomDirection *= _randomnessStrength;
            randomDirection += InitialPosition;
            return randomDirection;
        }

        protected override Vector3 CalculateNextPosition()
        {
            switch (_movingPattern)
            {
                case MovingPattern.Random:
                    return MoveRandomly();
                case MovingPattern.Linear:
                    return MoveLinearly();
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        #endregion
    }

}