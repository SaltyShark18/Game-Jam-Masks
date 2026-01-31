using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMask : MonoBehaviour
{
    // Settings
    public GameObject fireballPrefab;
    public Transform firePoint;         // where fireball originates
    public float fireballSpeed = 10f;   // fireball speed
    public float fireballLifetime = 3f; // fireball lifespan
    public float fireballCooldown = 1f; // fireball cooldown time
    private float lastMoveDirection = 1f;     // default: right
    public AudioSource fireballSound;
    private float nextFireTime = 0f;
    private SpriteRenderer playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();

        if (firePoint == null)
        {
            firePoint = transform;
            Debug.LogWarning("Firepoint not found. Using player instead");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            lastMoveDirection = Mathf.Sign(horizontal);
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        // cooldown
        nextFireTime = Time.time + fireballCooldown;
        
        // audio.
        if (fireballSound != null)
        {
            fireballSound.Play();
        }

        // instantiate fireball
        if (fireballPrefab != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = GetFireDirection();
                rb.velocity = new Vector2(direction * fireballSpeed, 0);
                SpriteRenderer fireballSprite = fireball.GetComponent<SpriteRenderer>();
                if (fireballSprite != null && direction < 0)
                {
                    fireballSprite.flipX = true;
                }
            }
            Destroy(fireball, fireballLifetime);
        }
        else
        {
            Debug.LogWarning("Fireball prefab not assigned!");
        }
    }

    float GetFireDirection()
    {
        return lastMoveDirection;
    }
}
