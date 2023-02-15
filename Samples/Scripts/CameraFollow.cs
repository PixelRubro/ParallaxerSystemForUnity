using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSpark.Parallaxer.Demo
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private float interpVelocity;
        
        [SerializeField]
        private float minDistance;

        [SerializeField]
        private float followDistance;

        [SerializeField]
        private GameObject target;

        [SerializeField]
        private Vector3 offset;

        private Transform _transform;
        
        void Start ()
        {
            _transform = transform;
        }

        void FixedUpdate ()
        {
            if (target)
            {
                Vector3 posNoZ = _transform.position;
                posNoZ.z = target.transform.position.z;

                Vector3 targetDirection = (target.transform.position - posNoZ);

                interpVelocity = targetDirection.magnitude * 5f;

                var targetPos = _transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

                _transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);

            }
        }
    }
}
