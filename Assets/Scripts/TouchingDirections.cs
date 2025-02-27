using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{   
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.02f;
    public float ceilingDistance = 0.05f;
    CapsuleCollider2D touchingCol;
    Animator anim;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];


    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded { get 
        {
            return _isGrounded;
        } 
        private set
        {
            _isGrounded = value;
            anim.SetBool(AnimStrings.IsGrounded, value);
        } 
    }

    
    [SerializeField]
    private bool _isOnWall = true;

    public bool IsOnWall { get 
        {
            return _isOnWall;
        } 
        private set
        {
            _isOnWall = value;
            anim.SetBool(AnimStrings.IsOnWall, value);
        } 
    }

    
    [SerializeField]
    private bool _isOnCeiling = true;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling { get 
        {
            return _isOnCeiling;
        } 
        private set
        {
            _isOnCeiling = value;
            anim.SetBool(AnimStrings.IsOnCeiling, value);
        } 
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits , groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
