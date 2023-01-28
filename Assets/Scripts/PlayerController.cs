using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement mov;
    [SerializeField] private State state;
    [SerializeField] private float stateStartTime;
    
    private Rigidbody2D _rb;
    private bool facingLeft = false;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private enum State
    {
        Idle,
        Jump,
        Falling,
        Walking,
        WallJump
    }

    private void FixedUpdate()
    {
        ProcessStateChanges();
        ProcessInput();
        ProcessMovement();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W) && IsGrounded() && (state == State.Idle || state == State.Walking))
        {
            state = State.Jump;
            stateStartTime = Time.time;
        }
        if (Input.GetKey(KeyCode.A) && state == State.Idle)
        {
            state = State.Walking;
            stateStartTime = Time.time;
            facingLeft = true;
        }
        if (Input.GetKey(KeyCode.D) && state == State.Idle)
        {
            state = State.Walking;
            stateStartTime = Time.time;
            facingLeft = false;
        }
    }


    private void ProcessStateChanges()
    {
        if (_rb.velocity.sqrMagnitude < mov.idleCutoff && (state == State.Falling || state == State.Walking))
        {
            state = State.Idle;
            stateStartTime = Time.time;
        }
        
        if (state == State.Jump && Time.time - stateStartTime > mov.jumpCurve[mov.jumpCurve.length - 1].time)
        {
            // Jump is over, revert to idle
            state = State.Idle;
            stateStartTime = Time.time;
        }

        if (state != State.Jump && state != State.WallJump && !IsGrounded())
        {
            // Not jumping, but we're in the air, so we're falling
            state = State.Falling;
            stateStartTime = Time.time;
        }
        
        if ((state == State.Falling || state == State.Jump) && IsGrounded())
        {
            // Was falling or jumping but we hit the ground, so revert
            state = State.Idle;
            stateStartTime = Time.time;
        }
    }
    

    private void ProcessMovement()
    {
        if (state == State.Jump)
        {
            var stateDuration = mov.jumpCurve[mov.jumpCurve.length - 1].time;
            if (Time.time - stateStartTime > stateDuration)
            {
                state = State.Idle;
                stateStartTime = Time.time;
            }
            else
            {
                _rb.velocity = new Vector2(
                    _rb.velocity.x,
                    mov.jumpCurve.Evaluate((Time.time - stateStartTime) / stateDuration)
                );   
            }
        }

        if (state == State.Walking)
        {
            
        }

        if (state == State.Falling)
        {
            _rb.velocity = new Vector2(
                _rb.velocity.x,
                mov.fallSpeed
            );   
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
