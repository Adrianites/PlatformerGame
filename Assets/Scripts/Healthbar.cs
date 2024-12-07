using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthSlider;
    Damageable playerDmgable;

    void Awake()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        
        if (Player == null)
        {
            Debug.LogError("Player not found, do you have 'Player' tag?");
        }
        playerDmgable = Player.GetComponent<Damageable>();
    }
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDmgable.CurrentHealth, playerDmgable.MaxHealth);
    }

    void OnEnable()
    {
        playerDmgable.healthChanged.AddListener(UpdateHealthbar);
    }

    void OnDisable()
    {
        playerDmgable.healthChanged.RemoveListener(UpdateHealthbar);
    }

    private void UpdateHealthbar(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return (float)currentHealth / maxHealth;
    }
}
