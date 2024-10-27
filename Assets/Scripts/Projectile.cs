using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public UnityEngine.Vector2 moveSpeed = new UnityEngine.Vector2(7f, 0);


    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new UnityEngine.Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            bool gotHit = damageable.Hit(damage, UnityEngine.Vector2.zero);

            if (gotHit)
            {
                Debug.Log(collision.name + "Hits for" + damage);
                Destroy(gameObject);
            }
        }
    }
}
