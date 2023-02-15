using UnityEngine;

namespace PixelSpark.Parallaxer.Helpers
{
    public enum EDirection
    {
        Up, Down, Right, Left, Forward, Backward, None
    }

    public class Direction
    {
        public static EDirection FlipDirection(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Right:
                    return EDirection.Left;
                case EDirection.Left:
                    return EDirection.Right;
                case EDirection.Up:
                    return EDirection.Down;
                case EDirection.Down:
                    return EDirection.Up;
                case EDirection.Forward:
                    return EDirection.Backward;
                case EDirection.Backward:
                    return EDirection.Forward;
                default:
                    return EDirection.None;
            }
        }

        public static bool IsHorizontal(EDirection direction)
        {
            return (direction == EDirection.Right) || (direction == EDirection.Left);
        }

        public static bool IsVertical(EDirection direction)
        {
            return (direction == EDirection.Up) || (direction == EDirection.Down);
        }

        public static Vector2 ToVector2(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Right:
                    return Vector2.right;
                case EDirection.Left:
                    return Vector2.left;
                case EDirection.Up:
                    return Vector2.up;
                case EDirection.Down:
                    return Vector2.down;
                default:
                    return Vector2.zero;
            }
        }

        public static Vector3 ToVector3(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Right:
                    return Vector3.right;
                case EDirection.Left:
                    return Vector3.left;
                case EDirection.Up:
                    return Vector3.up;
                case EDirection.Down:
                    return Vector3.down;
                case EDirection.Forward:
                    return Vector3.forward;
                case EDirection.Backward:
                    return Vector3.back;
                default:
                    return Vector3.zero;
            }
        }
    }
}
