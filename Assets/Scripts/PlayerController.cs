using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private double jumpCooldown;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private PhysicsMaterial2D bounceMat;
    private double _bounceTime;
    private double _jumpTime;
    private Rigidbody2D _rb;
    private bool _wasWallJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleFall();
        HandleMovement();
        HandleJump();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>().sharedMaterial == bounceMat) _bounceTime = Time.time;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && Time.time > _jumpTime + jumpCooldown)
        {
            if (IsGrounded())
            {
                _rb.velocity += new Vector2(0, jumpForce);
                _wasWallJump = false;
            }

            else if (IsTouchingWall())
            {
                var dir = IsTouchingLeftWall() ? -1 : 1;
                _rb.velocity = new Vector2(jumpForce * dir * 0.6f, jumpForce * 0.5f);
                _wasWallJump = true;
            }

            _jumpTime = Time.time;
        }
    }

    private void HandleFall()
    {
        if (Input.GetKey(KeyCode.S) && _bounceTime + 0.2 < Time.time)
            _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeed);
    }

    private void HandleMovement()
    {
        if (_bounceTime + 0.2 > Time.time) return;
        var s = IsGrounded() ? speed : speed / 3f;
        if (Input.GetKey(KeyCode.S)) s *= 1.8f;
        if (_wasWallJump && _jumpTime + jumpCooldown > Time.time)
        {
            // don't want to instantly be able to override direction on wall jump
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, Math.Max(s, _rb.velocity.x), 0.8f), _rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, Math.Min(-s, _rb.velocity.x), 0.8f), _rb.velocity.y);
        }
        else if (IsGrounded())
        {
            // if on the ground, stop moving horizontally
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 0.3f), _rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        var pos = transform.position - new Vector3(0, 0.8f, 0f);
        return Physics2D.OverlapPoint(pos) != null;
    }

    private bool IsTouchingWall()
    {
        return IsTouchingLeftWall() || IsTouchingRightWall();
    }

    private bool IsTouchingLeftWall()
    {
        var pos = transform.position - new Vector3(-0.4f, 0f, 0f);
        return Physics2D.OverlapPoint(pos) != null;
    }

    private bool IsTouchingRightWall()
    {
        var pos = transform.position - new Vector3(0.4f, 0f, 0f);
        return Physics2D.OverlapPoint(pos) != null;
    }
}