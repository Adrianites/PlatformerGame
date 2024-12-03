using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator anim;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _currentHealth = 100;
    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;

            if (_currentHealth <= 0)
            {
                IsAlive = false;
            }
        }
    }


    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibleTime = 0.25f;



    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            anim.SetBool(AnimStrings.isAlive, value);
            Debug.Log("IsAlive: " + value);
        }
    }

    public bool LockVelocity { get
        {
            return anim.GetBool(AnimStrings.lockVelocity);
        } 
        set
        {
            anim.SetBool(AnimStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibleTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            
            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            CurrentHealth -= damage;
            isInvincible = true;

            anim.SetTrigger(AnimStrings.HitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        // unable to be hit
        return false;
    }
}
