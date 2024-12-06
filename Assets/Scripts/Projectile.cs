using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);
    public int damage = 10;
    Rigidbody2D rb;
    public PlayerController player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        if (player.IsFacingRight)
        {
            rb.velocity = new Vector2(moveSpeed.x, moveSpeed.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed.x, moveSpeed.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmgable = collision.GetComponent<Damageable>();
        if (dmgable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = dmgable.Hit(damage, deliveredKnockback);
            
            if(gotHit)
            {
                Debug.Log(collision.name + "Hits for" + damage);
                Destroy(gameObject);
        }
    }
    }
}
