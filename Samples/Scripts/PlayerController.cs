using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSpark.Parallaxer.Demo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _movingSpeed = 6f;

        [SerializeField]
        private float _jumpStrength = 8f;

        private Transform _transform;

        private Rigidbody2D _rb2d;

        private SpriteRenderer _spriteRenderer;

        private Animator _animator;

        private float _previousHorizontalVelocity;

        private bool _isFacingRight = true;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _transform = transform;
        }

        private void Update()
        {
            Move();
            Jump();
        }

        private void Move()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            _rb2d.velocity = new Vector2(horizontalInput * _movingSpeed, _rb2d.velocity.y);
            CheckDirection(horizontalInput);
            PlayAnimation(horizontalInput);
        }

        private void CheckDirection(float horizontalInput)
        {
            if (horizontalInput == 0f)
            {
                return;
            }

            if (horizontalInput > 0f)
            {
                if (!_isFacingRight)
                {
                    FlipDirection();
                }
            }
            else
            {
                if (_isFacingRight)
                {
                    FlipDirection();
                }
            }
        }

        private void FlipDirection()
        {
            _isFacingRight = !_isFacingRight;
            _transform.localScale = new Vector3(_transform.localScale.x * -1f, _transform.localScale.y, _transform.localScale.z);
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(_rb2d.velocity.y, 0f))
            {
                _rb2d.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
            }
        }

        private void PlayAnimation(float horizontalInput)
        {
            if (horizontalInput == 0)
            {
                _animator.Play("idle");
            }
            else
            {
                if (Mathf.Approximately(_rb2d.velocity.y, 0f))
                {
                    _animator.Play("walk");
                }
            }
        }
    }    
}
