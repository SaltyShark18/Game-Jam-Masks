using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public static event Action<GameObject> OnPlayerDeath;

    // public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;

    //public static event Action OnPlayerDeath;

    void Start()
    {
        currentHealth = maxHealth;
        ResetHealth();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // GameController.OnReset += ResetHealth;
        // GameController.HealthUpgraded += IncreaseMaxHP;
        //HealthItem.OnHealthCollect += Heal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy enemy = collision.GetComponent<Enemy>();
        // if (enemy)
        // {
        //     TakeDamage(enemy.damage);
        //     //SoundEffectManager.Play("PlayerHit");
        // }
        ItsATrap trap = collision.GetComponent<ItsATrap>();
        if (trap && trap.damage > 0)
        {
            UnityEngine.Debug.Log("I am on trap. Ohhh noooo. :|");
            TakeDamage(trap.damage);
        }

        // FallingTrap fallTrap = collision.GetComponent<FallingTrap>();
        // if (fallTrap)
        // {
        //     TakeDamage(fallTrap.damage);
        // }
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // healthUI.UpdateHearts(currentHealth);
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        // healthUI.SetMaxHearts(maxHealth);
    }

    void IncreaseMaxHP(int hpInc)
    {
        maxHealth += hpInc;
        UnityEngine.Debug.Log(maxHealth);
        // healthUI.SetMaxHearts(hpInc);
        ResetHealth();
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            //Will do game over and player is dead.
            if (CheckpointManager.instance != null)
            {
                CheckpointManager.instance.RespawnPlayer(transform);
            }
            currentHealth = maxHealth;
        }
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
