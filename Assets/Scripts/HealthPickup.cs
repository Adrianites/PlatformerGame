using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 25;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    PlayerController Player;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmgable = collision.GetComponent<Damageable>();
        if (dmgable)
        {
            bool wasHealed = dmgable.Heal(healthRestore);
            if (wasHealed)
            {
                Destroy(gameObject);
            }
        }
    }
}
