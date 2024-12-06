using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof (Damageable))]
public class Knight : MonoBehaviour
{
    public float walkAcceleration = 30f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.02f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable dmgable;
    Animator anim;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;



    public WalkableDirection WalkDirection
    {
        get {return _walkDirection;}
        set {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    [SerializeField]
    public bool _hasTarget = false;
    public bool HasTarget { get 
    {
        return _hasTarget;
    } private set
        {
            _hasTarget = value;
            anim.SetBool(AnimStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get 
        {
            return anim.GetBool(AnimStrings.canMove);
        }
    }

    public float AttackCooldown { get
    {
        return anim.GetFloat(AnimStrings.attackCooldown);
    }
    private set
    {
        anim.SetFloat(AnimStrings.attackCooldown, Mathf.Max(value, 0));
    }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        dmgable = GetComponent<Damageable>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if (!dmgable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                // accelerate to max speed
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.deltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }

    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else 
        {
            Debug.LogError("Invalid WalkDirection");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}