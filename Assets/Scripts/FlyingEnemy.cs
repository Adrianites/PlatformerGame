using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 5f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone attackDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;

    Animator anim;
    Rigidbody2D rb;
    Damageable dmgable;

    Transform nextWaypoint;
    int waypointIndex = 0;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dmgable = GetComponent<Damageable>();
    }

    public void Start()
    {
        nextWaypoint = waypoints[waypointIndex];
    }

    void OnEnable()
    {
        dmgable.damageableDeath.AddListener(OnDeath);
    }

    public void Update()
    {
        HasTarget = attackDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if(dmgable.IsAlive)
        {
            if(CanMove)
            {
                Flight();
            }
            else
            {   
                // no movement
                rb.velocity = Vector3.zero;
            }
        }
    }

    [SerializeField]
    public bool _hasTarget = false;

    public bool HasTarget { get 
    {
        return _hasTarget;
    } 
    private set
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

    private void Flight()
    {
        //fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // check if we reached the waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * speed;
        UpdateDirection();

        // see if it needs to switch waypoints
        if(distance <= waypointReachedDistance)
        {   
            //switch to next waypoint
            waypointIndex++;
            if(waypointIndex >= waypoints.Count)
            {   
                // loop back to the first waypoint
                waypointIndex = 0;
            }
            nextWaypoint = waypoints[waypointIndex];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            // facing right
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }

        }
        else
        {
            // facing left
            if(rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }
        }
        
    }

    public void OnDeath()
    {
        // dead
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        if(deathCollider != null)
        {
            deathCollider.enabled = true;
        }
    }
}
