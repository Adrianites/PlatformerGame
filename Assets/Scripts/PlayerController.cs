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
    public float RollImpulse = 1000f;
    Vector2 moveInput;

    TouchingDirections touchingDirections;
    public float currentMoveSpeed { get
        { 
            if (canMove)
            {  
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else if (IsCrouching)
                        {
                            return crouchSpeed;
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
                    return 0; // Idle
                } 
            }
            else
            {
                return 0; // Locked movement
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

    [SerializeField]
    private bool isAttacking = false;
    public bool IsAttacking { get
        {
            return isAttacking;
        } 
        private set
        {
            isAttacking = value;
            anim.SetBool(AnimStrings.IsAttacking, value);
        } 
    }

    [SerializeField]
    private bool isAltAttacking = false;
    public bool IsAltAttacking { get
        {
            return isAltAttacking;
        } 
        private set
        {
            isAltAttacking = value;
            anim.SetBool(AnimStrings.IsAltAttacking, value);
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

    public bool canMove {get
        {
            return anim.GetBool(AnimStrings.canMove);
        }
    }

    public bool isAlive {get
        {
            return anim.GetBool(AnimStrings.isAlive);
        }
    }

    public bool LockVelocity { get
        {
            return anim.GetBool(AnimStrings.lockVelocity);
        } 
        set
        {
            anim.SetBool(AnimStrings.lockVelocity, value);
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
        if (!LockVelocity)
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
        anim.SetFloat(AnimStrings.yVelocity, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (isAlive)
        {
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
        }
        else 
        {
            IsMoving = false;
        }
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnAltAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAltAttacking = true;
        }
        else if (context.canceled)
        {
            IsAltAttacking = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && canMove)
        {
            anim.SetTrigger(AnimStrings.JumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded)
        {
            anim.SetTrigger(AnimStrings.RollTrigger);
            rb.velocity = new Vector2(RollImpulse, rb.velocity.y);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {   
        LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

}


