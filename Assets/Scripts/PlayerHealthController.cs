using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()//функция вызывается всякий раз когда происходит вызов экземпляра скрипта
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    [HideInInspector]
    public int currentHealth;
    public int maxHealth;

    public float invincLength;
    private float invincCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;

    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }


    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            if(flashCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                {
                    sprite.enabled = !sprite.enabled;
                }
                flashCounter = flashLength;
            }

            if (invincCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                {
                    sprite.enabled = true;
                }
                flashCounter = 0f;

            }
        }
        
    }

    public void DamagePlayer(int damageAmoumt)
    {
        if (invincCounter <= 0)
        {
            currentHealth -= damageAmoumt;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //gameObject.SetActive(false);
                RespawnController.instance.Respawn();

                AudioManager.instance.PlaySFX(8);
            }
            else
            {
                invincCounter = invincLength;

                AudioManager.instance.PlaySFXAdjusted(11);
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }

    }

    public void FillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
