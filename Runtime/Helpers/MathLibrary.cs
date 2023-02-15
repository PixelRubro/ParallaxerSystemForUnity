using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSpark.Parallaxer.Helpers
{
    public class MathLibrary : MonoBehaviour
    {
        /// <summary>
        /// Return num > 0 if angle between vectors is greater than 90 degrees, 
        /// return num < 0 if angle between vectors is lesser than 90 degrees
        /// return 0 if angle between vectors is equal to 90 degrees.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float Dot(Vector3 v1, Vector3 v2) //ok
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Magnitude(Vector3 v) 
        {
            return Mathf.Sqrt(Square(v.x) + Square(v.y) + Square(v.z));
        }

        /// <summary>
        /// Returns the distance between the two vectors.
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="destinationPos"></param>
        /// <returns></returns>
        public static float Distance(Vector3 currentPos, Vector3 destinationPos) //ok
        {
            return Mathf.Sqrt(Square(destinationPos.x - currentPos.x) + 
                Square(destinationPos.y - currentPos.y) + Square(destinationPos.z - currentPos.z));
        }

        /// <summary>
        /// Returns the direction from currentPos to focusPos.
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="focusPos"></param>
        /// <returns></returns>
        public static Vector3 Direction(Vector3 currentPos, Vector3 focusPos) 
        {
            return new Vector3(focusPos.x - currentPos.x, focusPos.y - currentPos.y, focusPos.z - currentPos.z);
        }

        /// <summary>
        /// Returns the vector normal, which is the same vector with magnitude 1.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Normal(Vector2 v)
        {
            return new Vector2(v.x / (v).magnitude, v.y / (v).magnitude);
        }

        /// <summary>
        /// Returns the angle between two vectors in radians.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float Angle(Vector2 v1, Vector2 v2)
        {
            // return Mathf.Acos(Dot(v1.normalized, v2.normalized) / (v1).magnitude * (v2).magnitude);
            return Mathf.Acos(Dot(v1, v2) / (Distance(Vector2.zero, v1) * Distance(Vector2.zero, v2)));
        }

        /// <summary>
        /// Returns n squared.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static float Square(float n) 
        {
            return n * n;
        }

        /// <summary>
        /// Returns n squared.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Square(int n) 
        {
            return n * n;
        }

        /// <summary>
        /// Returns something less than zero if the rotation needs to be clockwise and returns 
        /// greater than zero if counter-clockwise
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
        }

        /// <summary>
        /// Make the object with the currentPos face the focusPos using Atan2.
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="focusPos"></param>
        /// <returns></returns>
        public static Quaternion LookAt2D(Vector3 currentPos, Vector3 focusPos) 
        { 
            Vector2 dirNormalized = Normal(Direction(currentPos, focusPos));
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dirNormalized.y, dirNormalized.x) * Mathf.Rad2Deg);
            return rotation;
        }

        /// <summary>
        /// Make the object with the currentPos face the focusPos (dont use on update).
        /// </summary>
        /// <param name="facingDirection"></param>
        /// <param name="currentPos"></param>
        /// <param name="focusPos"></param>
        /// <returns></returns>
        public static Vector3 LookAt2D(Vector3 facingDirection, Vector3 currentPos, Vector3 focusPos)
        {
            Vector3 direction = Direction(currentPos, focusPos);
            float angle = Angle(facingDirection, direction);
            bool clockwise = Cross(currentPos, direction).z < 0 ? true : false;
            return Rotate(facingDirection, angle, clockwise);
        }

        /// <summary>
        /// Rotates the object on itself. Store this in object's transform.up.
        /// </summary>
        /// <param name="facingDirection"></param>
        /// <param name="angle"></param>
        /// <param name="rotateClockwise"></param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector3 facingDirection, float angle, bool rotateClockwise)
        {
            // angle *= 180 / 2;
            if (rotateClockwise)
                angle = 2 * Mathf.PI - angle;

            return new Vector2(facingDirection.x * Mathf.Cos(angle) - facingDirection.y * Mathf.Sin(angle),
                                facingDirection.x * Mathf.Sin(angle) + facingDirection.y * Mathf.Cos(angle));
        }

        /// <summary>
        /// "Moves" the object to a destiny.!-- Please store the return in the localPosition atribute.
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="destinationPos"></param>
        /// <returns></returns>
        public static Vector2 Translate(Vector3 currentPos, Vector3 destinationPos) 
        {
            return currentPos + destinationPos * Time.deltaTime;
        }

        /**
        "Moves" the object while pixel perfect. Please store the return in the localPosition atribute
        */
        // public static Vector2 TranslatePixelArt(Vector3 currentPos, Vector3 destinationPos, float pixelInUnits) 
        // {
        //     return Vector3.Lerp(currentPos, Translate(currentPos, destinationPos * pixelInUnits), Time.deltaTime * Random.Range(300f, 500f));
        //     // return Translate(currentPos, destinationPos * pixelInUnits);
        // }

        /// <summary>
        /// Returns a number which must then be multiplied with a angle in radians what will 
        /// result in a number representing the same angle in degrees.
        /// </summary>
        /// <returns></returns>
        public static float RadiansToDegrees() 
        {
            return 180 / Mathf.PI;
        }

        /**
        Round the unit measured in unit values to the nearest value that can
        perfect pixel
        */
        // public static float RoundUnitToPixel(float unityUnits, float pixelPerUnit) 
        // {//please set the V-Sync to Every V Blank
        //     float valueInPixels = unityUnits * pixelPerUnit;
        //     valueInPixels = Mathf.Round(valueInPixels);
        //     float roundedUnityUnits = valueInPixels * (1 / pixelPerUnit);
        //     return roundedUnityUnits;
        // }

        /**
        Round vector axes to the nearest value that can be perfect pixel
        */
        // public static Vector2 RoundVectorToPixel(Vector2 vector, float pixelPerUnit) 
        // {//please set the V-Sync to Every V Blank
        //     return new Vector2(RoundUnitToPixel(vector.x, pixelPerUnit), RoundUnitToPixel(vector.y, pixelPerUnit));
        // }

        // public static float GetPixelPerfectCameraSize()
        // {

        // }

        /// <summary>
        /// Move towards horizontally.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Vector2 MoveTowardsHorizontally(Vector2 start, Vector2 end, float maxDistanceDelta)
        {
            Vector2 a = start - end;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f) {return end;}
            return new Vector2(start.x + a.x, start.y) / magnitude * maxDistanceDelta;
        }

        /// <summary>
        /// Return a diagonal vector with 1 for x and -1 for y.
        /// </summary>
        /// <param name="Vector2(1"></param>
        /// <returns></returns>
        public static Vector2 GetDownRightVector2() {return new Vector2(1, -1);}

        /// <summary>
        /// Returns a diagonal vector with -1 for x and 1 for y.
        /// </summary>
        /// <param name="Vector2(-1"></param>
        /// <returns></returns>
        public static Vector2 GetUpLeftVector2() {return new Vector2(-1, 1);}

        /// <summary>
        /// Calculate the velocity.
        /// </summary>
        /// <param name="initialPos"></param>
        /// <param name="finalPos"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector2 GetVelocity(Vector2 initialPos, Vector2 finalPos, float deltaTime)
        {
            return (finalPos - initialPos) / deltaTime;
        }

        /// <summary>
        /// Calculate the velocity.
        /// </summary>
        /// <param name="initialPos"></param>
        /// <param name="finalPos"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector3 GetVelocity(Vector3 initialPos, Vector3 finalPos, float deltaTime)
        {
            return (finalPos - initialPos) / deltaTime;
        }

        /// <summary>
        /// Calculate velocity on a single axis.
        /// </summary>
        /// <param name="initialPos"></param>
        /// <param name="finalPos"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float GetVelocity(float initialPos, float finalPos, float deltaTime) 
        {
            return (finalPos - initialPos) / deltaTime;
        }

        /// <summary>
        /// Return the absolute value of n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static float Abs(float n) 
        {
            if (n >= 0) return n;
            return -n;
        }

        /// <summary>
        /// Return the absolute value of n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Abs(int n) 
        {
            if (n >= 0) return n;
            return -n;
        }

        /// <summary>
        /// Returns a Vector2 representing the initial velocity of the rigidbody so it hits the target
        /// while going through a parabola. The <paramref name="initialPos"/> should be from where the
        /// rigidbody is launched and not the rigidbody's own position.
        /// </summary>
        /// <param name="initialPos"></param>
        /// <param name="finalPos"></param>
        /// <param name="time"></param>
        public static Vector2 GetParabolaInitialVelocity(Vector2 initialPos, Vector2 finalPos, float time)
        {
            Vector2 velocity = GetVelocity(initialPos, finalPos, time);
            velocity.y += 0.5f * Abs(Physics2D.gravity.y) * Square(time) / time;
            return velocity;
        }

        /// <summary>
        /// Simulate a heads or tails toss.
        /// </summary>
        /// <param name="Random.Range(0"></param>
        /// <returns></returns>
        public static bool HeadsOrTails() {return Random.Range(0, 2) > 0;}

        /// <summary>
        /// Simulate a dice roll with the given number of sides. The minimum value is 1.
        /// </summary>
        /// <param name="Random.Range(1"></param>
        /// <param name="1"></param>
        /// <returns></returns>
        /// 
        public static int RollDice(int sides) { return Random.Range(1, sides + 1); }

        /// <summary>
        /// Get a random Vector2 with a length of 1.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetRandomNormalizedVector2()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        /// <summary>
        /// Get a random Vector3 with a length of 1.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomNormalizedVector3()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        public static Vector2 Acceleration(Vector2 netForce, float mass)
        {
            if (netForce.Equals(Vector2.zero))
            {
                return Vector2.zero;
            }

            return netForce / mass;
        }

        public static Vector3 Acceleration(Vector3 netForce, float mass)
        {
            if (netForce.Equals(Vector3.zero))
            {
                return Vector3.zero;
            }

            return netForce / mass;
        }

        public static Vector2 Velocity(Vector2 acceleration)
        {
            return acceleration * Time.deltaTime;
        }

        public static Vector3 Velocity(Vector3 acceleration)
        {
            return acceleration * Time.deltaTime;
        }

        public static Vector2 ClampVector2(Vector2 value, float maxLength)
        {
            var magnitude = value.magnitude;

            if (magnitude <= maxLength)
            {
                return value;
            }

            return value.normalized * maxLength;
        }

        public static Vector3 ClampVector3(Vector3 value, float maxLength)
        {
            var magnitude = value.magnitude;

            if (magnitude <= maxLength)
            {
                return value;
            }

            return value.normalized * maxLength;
        }

        /// <summary>
        /// Translates a world position <paramref name="worldPosition"/> to relative position in relation
        /// to a point <paramref name="pointToRelateTo"/>;
        /// </summary>
        /// <param name="worldPosition">Position to be translated.</param>
        /// <param name="pointToRelateTo">Point which will be the origin relative to point in <paramref name="worldPosition"/></param>
        /// <returns></returns>
        public static Vector3 WorldToLocalPosition(Vector3 worldPosition, Vector3 pointToRelateTo)
        {
            return worldPosition - pointToRelateTo;
        }

        /// <summary>
        /// Round the <paramref name="value"/> to a number divisible by 0.5.
        /// </summary>
        public static float RoundToHalf(float value)
        {
            return (float) System.Math.Round(value * 2, System.MidpointRounding.AwayFromZero) * 0.5f;
        }
    }
}
