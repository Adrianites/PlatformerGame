using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof (Damageable))]
public class PlayerController : MonoBehaviour
{
#region Setting Variables
    public float walkSpeed = 5f; 
    public float runSpeed = 10f;
    public float crouchSpeed = 0f;
    public float airWalkSpeed = 3f;
    public float JumpImpulse = 10f;
    public float RollImpulse = 1f;
    public bool IsRolling = false;
    public InteractionType interactionType;
    public enum InteractionType {Portal, Lever, Door, Chest, NPC};
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable dmgable;
    Attractable attractable;
    Rigidbody2D rb;

    SpriteRenderer sr;
    Animator anim;
    GameObject portal;
    public PortalController portalController;

    GameObject BulletLeft;
    GameObject BulletRight;
    GameObject BulletCrouchLeft;
    GameObject BulletCrouchRight;
    GameObject LightLeft;
    GameObject LightRight;
    GameObject MeleeLeft;
    GameObject MeleeRight;
    
#endregion

#region Current Movement Speed
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
#endregion

#region Animation Variables
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

    [SerializeField]
    public bool _isFacingRight = true;
    public bool IsFacingRight { 
        get 
        {
            // Debug.Log("is facing right " + _isFacingRight);
            return _isFacingRight;
        }
        private set
        {
            // flip only if the value is new
            if (_isFacingRight != value)
            {
                // flip the local scale to make player look the other way
                // transform.localScale *= new Vector2(-1, 1);
                sr.flipX = !sr.flipX;
                // Debug.Log("flipping character");
            }
            _isFacingRight = value;
            // Debug.Log("not flipping character");
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
#endregion

#region Awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        dmgable = GetComponent<Damageable>();
        attractable = GetComponent<Attractable>();
        if (GameObject.FindGameObjectWithTag("Portal") != null)
        {
            portal = GameObject.FindGameObjectWithTag("Portal");
            portalController = portal.GetComponent<PortalController>();
        }
        BulletLeft = GameObject.FindGameObjectWithTag("BulletLeft");
        BulletRight = GameObject.FindGameObjectWithTag("BulletRight");
        BulletCrouchLeft = GameObject.FindGameObjectWithTag("BulletCrouchLeft");
        BulletCrouchRight = GameObject.FindGameObjectWithTag("BulletCrouchRight");
        MeleeLeft = GameObject.FindGameObjectWithTag("MeleeLeft");
        MeleeRight = GameObject.FindGameObjectWithTag("MeleeRight");
        if (GameObject.FindGameObjectWithTag("LightLeft") != null)
        {
            LightLeft = GameObject.FindGameObjectWithTag("LightLeft");
            LightRight = GameObject.FindGameObjectWithTag("LightRight");
        }
    }
#endregion

    void Start()
    {
        BulletCrouchLeft.SetActive(false);
        BulletLeft.SetActive(false);
        MeleeLeft.SetActive(false);
        BulletCrouchRight.SetActive(false);
        if (LightLeft != null)
        {
        LightLeft.SetActive(false);
        }
    }

#region Fixed Updates
    void FixedUpdate()
    {   
        if (!dmgable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
        anim.SetFloat(AnimStrings.yVelocity, rb.velocity.y);
    }
#endregion

#region Input Actions
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (isAlive && UIManager.isPaused == false)
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
        if (moveInput.x > 0 && !IsFacingRight && UIManager.isPaused == false)
        {
            // face right
            IsFacingRight = true;
            BulletLeft.SetActive(false);
            BulletCrouchLeft.SetActive(false);
            if(!IsCrouching)
            {
            BulletRight.SetActive(true);
            BulletCrouchRight.SetActive(false);
            }
            else
            {
            BulletCrouchRight.SetActive(true);
            BulletRight.SetActive(false);
            }
            MeleeLeft.SetActive(false);
            MeleeRight.SetActive(true);
            if (LightLeft != null)
            {
                LightLeft.SetActive(false);
                LightRight.SetActive(true);
            }

            // Debug.Log("facing right");
        }
        else if (moveInput.x < 0 && IsFacingRight && UIManager.isPaused == false)
        {
            // face left
            IsFacingRight = false;
            BulletRight.SetActive(false);
            BulletCrouchRight.SetActive(false);
            if(!IsCrouching)
            {
            BulletLeft.SetActive(true);
            BulletCrouchLeft.SetActive(false);
            }
            else
            {
            BulletCrouchLeft.SetActive(true);
            BulletLeft.SetActive(false);
            }
            MeleeLeft.SetActive(true);
            MeleeRight.SetActive(false);
            if (LightLeft != null)
            {
                LightLeft.SetActive(true);
                LightRight.SetActive(false);
            }
            // Debug.Log("facing left");
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.isPaused == false)
        {
            IsRunning = true;
        }
        else if (context.canceled && UIManager.isPaused == false)
        {
            IsRunning = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.isPaused == false)
        {
            IsCrouching = true;
            if(IsFacingRight)
            {
                BulletRight.SetActive(false);
                BulletCrouchRight.SetActive(true);
            }
            else
            {
                BulletLeft.SetActive(false);
                BulletCrouchLeft.SetActive(true);
            }
        }
        else if (context.canceled && UIManager.isPaused == false)
        {
            IsCrouching = false;
            if(IsFacingRight)
            {
                BulletCrouchRight.SetActive(false);
                BulletRight.SetActive(true);
            }
            else
            {
                BulletCrouchLeft.SetActive(false);
                BulletLeft.SetActive(true);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.isPaused == false)
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
        if (context.started && UIManager.isPaused == false)
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
        if (context.started && (touchingDirections.IsGrounded && canMove || (attractable != null && attractable.IsOnSide())))
        {
            anim.SetTrigger(AnimStrings.JumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && UIManager.isPaused == false)
        {
            anim.SetTrigger(AnimStrings.RollTrigger);
            rb.velocity = new Vector2(RollImpulse, rb.velocity.y);
            IsRolling = true;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {   
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.isPaused == false)
        {
            switch(interactionType)
            {
                case InteractionType.Portal:
                    portalController.OnInteraction();
                    //Debug.Log("Interacting with portal (Player Controller)");
                    break;
                case InteractionType.Lever:
                    // Interact with lever
                    break;
                case InteractionType.Door:
                    // Interact with door
                    break;
                case InteractionType.Chest:
                    // Interact with chest
                    break;
                case InteractionType.NPC:
                    // Interact with NPC
                    break;
                default:
                    break;
            }
        }
    }
#endregion

    public void HealAnim()
    {   
        if (UIManager.isPaused == false)
        {
        anim.SetTrigger(AnimStrings.HealTrigger);
        }
    }

}
