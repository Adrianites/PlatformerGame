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
    public float rollSpeed = 20f;
    public float crouchSpeed = 0f;
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
                    if (IsCrouching)
                    {       
                        if (isRunning)
                        {
                            if (IsRolling)
                            {
                                return rollSpeed;
                            }
                            else
                            {
                                return runSpeed;
                            }
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else 
                    {
                        return crouchSpeed;
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

    [SerializeField]
    private bool isRolling = false;
    public bool IsRolling { get
        {
            return isRolling;
        } 
        private set
        {
            isRolling = value;
            anim.SetBool(AnimStrings.IsRolling, value);
        } 
    }

    [SerializeField]
    private bool isCrouching = false;
    public bool IsCrouching { get
        {
            return isCrouching;
        } 
        private set
        {
            isCrouching = value;
            anim.SetBool(AnimStrings.IsCrouching, value);
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

        public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRolling = true;
        }
        else if (context.canceled)
        {
            IsRolling = false;
        }
    }

            public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsCrouching = true;
        }
        else if (context.canceled)
        {
            IsCrouching = false;
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
