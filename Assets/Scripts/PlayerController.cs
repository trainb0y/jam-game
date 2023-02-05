using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private double jumpCooldown;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private PhysicsMaterial2D bounceMat;

    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip oof;

    private Animator _animation;
    private AudioSource _audio;

    private double _bounceTime;
    private double _jumpTime;
    private Rigidbody2D _rb;
    private bool _wasWallJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        HandleFall();
        HandleMovement();
        HandleJump();
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.S)) _animation.SetBool("Run", false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>().sharedMaterial == bounceMat)
        {
            _bounceTime = Time.time;
        }
        else if (col.relativeVelocity.magnitude > 14 && col.gameObject.GetComponent<BoxExplosion>() == null)
        {
            _audio.Stop();
            _audio.clip = oof;
            _audio.Play();
            var p = GetComponent<ParticleSystem>();
            GlobalData.Instance.cashAmount -= 10;
            p.Stop();
            p.Play();
            var cam = Camera.main.GetComponent<CameraFollow>();
            cam.screenshakeUntil = Time.time + 0.25f;
            cam.screenshakeIntensityMultiplier = 0.45f;
        }
        else if (col.relativeVelocity.magnitude > 10)
        {
            var cam = Camera.main.GetComponent<CameraFollow>();
            cam.screenshakeUntil = Time.time + 0.15f;
            cam.screenshakeIntensityMultiplier = 0.15f;
        }
    }

    private void HandleJump()
    {
        void AfterJump()
        {
            _jumpTime = Time.time;
            _animation.SetTrigger("Jump");
            var cam = Camera.main.GetComponent<CameraFollow>();
            cam.screenshakeUntil = Time.time + 0.1f;
            cam.screenshakeIntensityMultiplier = 0.15f;
            _audio.Stop();
            _audio.clip = jump;
            _audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.W) && Time.time > _jumpTime + jumpCooldown)
        {
            if (IsGrounded())
            {
                _rb.velocity += new Vector2(0, jumpForce);
                _wasWallJump = false;
                AfterJump();
            }

            else if (IsTouchingWall())
            {
                var dir = IsTouchingLeftWall() ? -1 : 1;
                _rb.velocity = new Vector2(jumpForce * dir * 0.6f, jumpForce * 0.5f);
                _wasWallJump = true;
                AfterJump();
            }
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
            _animation.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, Math.Min(-s, _rb.velocity.x), 0.8f), _rb.velocity.y);
            _animation.SetBool("Run", true);
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