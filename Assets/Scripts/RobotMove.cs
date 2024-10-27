using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public float speed = 5f;
    public float switchTime = 2f; // Time in seconds to switch direction
    private float timer;
    private int direction = 1; // 1 for right, -1 for left
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = switchTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            direction *= -1; // Switch direction
            timer = switchTime; // Reset timer
        }

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // Set the walking animation parameter
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }
}