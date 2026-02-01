using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // settings
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public bool stayPressed = true;   // if button stays pressed when player leaves  
    public float pressCooldown = 0.5f;
    public float lastPressTime = 0f;

    private AudioSource audioSource;
    public AudioClip pressSound;
    public AudioClip releaseSound;

    private SpriteRenderer spriteRenderer;
    public bool isPressed = false;

    public System.Action<ButtonController> OnButtonPressed;
    public System.Action<ButtonController> OnButtonReleased;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer == null)
        {
            Debug.LogError("ButtonController: No SpriteRenderer found!");
        }

        // Set initial sprite
        if (spriteRenderer != null && unpressedSprite != null)
        {
            spriteRenderer.sprite = unpressedSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Press button when player or touches it
        if (other.CompareTag("Player") && !isPressed)
        {
            PressButton();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Also handle regular collisions
        if (collision.gameObject.CompareTag("Player") && !isPressed)
        {
            PressButton();
        }
    }

    void PressButton()
    {
        // Cooldown check
        if (Time.time - lastPressTime < pressCooldown) return;

        isPressed = true;
        lastPressTime = Time.time;

        // Change sprite
        if (spriteRenderer != null && pressedSprite != null)
        {
            spriteRenderer.sprite = pressedSprite;
        }

        // Play sound
        if (audioSource != null && pressSound != null)
        {
            audioSource.PlayOneShot(pressSound);
        }

        // Notify listeners
        Debug.Log($"Button {gameObject.name} pressed!");
        OnButtonPressed?.Invoke(this);

        // If button doesn't stay pressed, release after a delay
        if (!stayPressed)
        {
            Invoke("ReleaseButton", 1f); // Release after 1 second
        }

        void ReleaseButton()
        {
            if (!isPressed) return;

            isPressed = false;

            // Change sprite back
            if (spriteRenderer != null && unpressedSprite != null)
            {
                spriteRenderer.sprite = unpressedSprite;
            }

            // Play release sound
            if (audioSource != null && releaseSound != null)
            {
                audioSource.PlayOneShot(releaseSound);
            }

            // Notify listeners
            Debug.Log($"Button {gameObject.name} released!");
            OnButtonReleased?.Invoke(this);
        }
    }

    // helper methods
    public bool IsPressed()
    {
        return isPressed;
    }
}