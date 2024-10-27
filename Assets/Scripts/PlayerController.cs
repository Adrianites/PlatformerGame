using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f; 
    public float runSpeed = 10f;
    public float airWalkSpeed = 3f;
    public float JumpImpulse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    public float currentMoveSpeed { get
        {   
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (isRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else 
                {
                    return airWalkSpeed;
                }
            }
            else
            {
                // Idle
                return 0;
        } 
    }
    }


    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            anim.SetBool(AnimStrings.IsMoving, value);
        } 
    }

    [SerializeField]
    private bool isRunning = false;
    public bool IsRunning { get
        {
            return isRunning;
        } 
        private set
        {
            isRunning = value;
            anim.SetBool(AnimStrings.IsRunning, value);
        } 
    }


    public bool _isFacingRight = true;
    public bool IsFacingRight { get 
        {
            return _isFacingRight;
        }
        private set
        {
            // flip only if the value is new
            if (_isFacingRight != value)
            {
                // flip the local scale to make player look the other way
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }    
    }

    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
        anim.SetFloat(AnimStrings.yVelocity, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // face left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded)
        {
            anim.SetTrigger(AnimStrings.Jump);
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
        }
    }
}
