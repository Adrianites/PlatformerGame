using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 25;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    PlayerController Player;
    AudioSource audioSource;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmgable = collision.GetComponent<Damageable>();
        if (dmgable && dmgable.CurrentHealth < dmgable.MaxHealth)
        {
            bool wasHealed = dmgable.Heal(healthRestore);
            if (wasHealed)
            {
                if (audioSource)
                {
                AudioSource.PlayClipAtPoint(audioSource.clip, gameObject.transform.position, audioSource.volume);
                }
                Destroy(gameObject);
            }
        }
    }
}
