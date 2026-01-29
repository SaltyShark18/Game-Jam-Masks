using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItsATrap : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;
    //public AudioSource damageSFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            // if (damage == 0)
            // {
            //     //SoundEffectManager.Play("Jump");
            // }
            // else if (damage == 1)
            // {
            //     //SoundEffectManager.Play("PlayerHit");
            // }
            // atm only plays dmg effect
            //damageSFX.Play();
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}

