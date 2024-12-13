using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    Damageable damageable;
    GameObject player;

 void Awake()
    {
        damageable = GetComponent<Damageable>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Damageable>().CurrentHealth = 0;
        }
    }
}
