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
    
    private Animator animation;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HandleFall();
        HandleMovement();
        HandleJump();
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
            animation.SetBool("Run", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>().sharedMaterial == bounceMat)
        {
            // todo: play bounce sound
            _bounceTime = Time.time;
        }
        else if ( col.relativeVelocity.magnitude > 14)
        {
            // todo: play oof sound
            var p = GetComponent<ParticleSystem>();
            GlobalData.Instance.cashAmount -= 10;
            p.Stop();
            p.Play();
            var cam = Camera.main.GetComponent<CameraFollow>();
            cam.screenshakeUntil = Time.time + 0.2f;
            cam.screenshakeIntensityMultiplier = 0.4f;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && Time.time > _jumpTime + jumpCooldown)
        {
            if (IsGrounded())
            {
                _rb.velocity += new Vector2(0, jumpForce);
                _wasWallJump = false;
                // todo: play jump sound
            }

            else if (IsTouchingWall())
            {
                var dir = IsTouchingLeftWall() ? -1 : 1;
                _rb.velocity = new Vector2(jumpForce * dir * 0.6f, jumpForce * 0.5f);
                _wasWallJump = true;
                // todo: play jump sound
            }

            _jumpTime = Time.time;
            animation.SetTrigger("Jump");
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
            animation.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, Math.Min(-s, _rb.velocity.x), 0.8f), _rb.velocity.y);
            animation.SetBool("Run", true);
        }
        else if (IsGrounded())
        {
            // if on the ground, stop moving horizontally
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 0.3f), _rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return IsTouching(new Vector3(0, -1f, 0));
    }

    private bool IsTouching(Vector3 offset)
    {
        var pos = transform.position + offset;
        var point = Physics2D.OverlapPoint(pos);
        return point != null && point.attachedRigidbody != null && point.attachedRigidbody.simulated;
    }

    private bool IsTouchingWall()
    {
        return IsTouchingLeftWall() || IsTouchingRightWall();
    }

    private bool IsTouchingLeftWall()
    {
        return IsTouching(new Vector3(0.7f, 0f, 0f));
    }

    private bool IsTouchingRightWall()
    {
        return IsTouching(new Vector3(-0.7f, 0f, 0f));
    }
}