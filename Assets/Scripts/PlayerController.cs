using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private double jumpCooldown;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;
    private Rigidbody2D _rb;
    private double _jumpTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleFall();
        HandleMovement();
    }

    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.W) && Time.time > _jumpTime + jumpCooldown)
        {
            if (IsGrounded())
            {
                _rb.velocity += new Vector2(0, jumpForce);
            }
            else if (IsTouchingWall())
            {
                var dir = IsTouchingLeftWall() ? -1 : 1;
                _rb.velocity += new Vector2(jumpForce * 1.5f * dir, jumpForce / 1.5f);
            }
            _jumpTime = Time.time;
        }
        
    }

    private void HandleFall()
    {
        if (Input.GetKey(KeyCode.S))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeed);
        }
    }

    private void HandleMovement()
    {
        var s = IsGrounded() ? speed : speed / 3f;
        if (Input.GetKey(KeyCode.S)) s *= 2.0f;
        
        if (Input.GetKey(KeyCode.D))
        {
            _rb.velocity = new Vector2(Math.Max(s, _rb.velocity.x), _rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(Math.Min(-s, _rb.velocity.x), _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
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
