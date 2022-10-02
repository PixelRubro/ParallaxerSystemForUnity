using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftBoiledGames.Parallaxer.Helpers;

namespace SoftBoiledGames.Parallaxer
{
    public class ParallaxMovingElement : ParallaxLayeredElement
    {
        #region Serialized fields

        [SerializeField]
        private MovingPattern _movingPattern;

        [SerializeField] 
        [InspectorAttributes.ShowIf(nameof(IsRandom), false)]
        private EDirection _movementDirection;

        [SerializeField] 
        [InspectorAttributes.ShowIf(nameof(IsRandom), false)]
        private Vector2 _customDirection;

        [SerializeField]
        private float _randomnessStrength = 0.25f;

        [SerializeField]
        private float _movementSpeed = 0.5f;

        [SerializeField]
        [InspectorAttributes.LeftToggle]
        private bool _changesPositionWhenRedrawn = false;

        #endregion

        #region Non-serialized fields

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

        #region Protected methods

        #region Overridden methods
        protected override void Initialize()
        {
            base.Initialize();
            _random = new System.Random();
        }

        protected override void OnLateUpdateEnter()
        {
            Vector3 nextPosition = CalculateNextPosition();
            Move(nextPosition);
        }

        #endregion

        #endregion

        #region Private methods

        private Vector3 MoveLinearly()
        {
            Vector3 displacement = Vector3.zero;

            switch (_movementDirection)
            {
                case EDirection.Right:
                    displacement.x = _movementSpeed * Time.deltaTime;
                    break;
                case EDirection.Left:
                    displacement.x = -_movementSpeed * Time.deltaTime;
                    break;
                case EDirection.Up:
                    displacement.y = _movementSpeed * Time.deltaTime;
                    break;
                case EDirection.Down:
                    displacement.y = -_movementSpeed * Time.deltaTime;
                    break;
                default:
                    break;
            }

            return displacement;
        }

        private Vector3 MoveRandomly()
        {
            Vector3 randomDirection = MathLibrary.GetRandomNormalizedVector2();
            randomDirection *= _randomnessStrength;
            randomDirection += InitialPosition;
            return randomDirection * _movementSpeed * Time.deltaTime;
        }

        protected override Vector3 CalculateNextPosition()
        {
            switch (_movingPattern)
            {
                case MovingPattern.Random:
                    return MoveRandomly() + GetParallaxMovement();
                case MovingPattern.Linear:
                    return MoveLinearly() + GetParallaxMovement();
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}