using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public int buttonsRequired = 1;
    public bool requireAllButtons = true;
    private SpriteRenderer spriteRenderer;
    private Sprite lockedDoorSprite;
    public Sprite unlockedDoorSprite;
    public Collider2D doorCollider;

    private AudioSource audioSource;
    public AudioClip unlockSound;

    public List<ButtonController> linkedButtons = new List<ButtonController>();
    private bool isUnlocked = false;
    private int currentlyPressedButtons = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer != null)
        {
            lockedDoorSprite = spriteRenderer.sprite;
        }

        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }

        Debug.Log($"ButtonDoor {gameObject.name}: Requires {buttonsRequired} button(s)");
        Debug.Log($"Linked buttons: {linkedButtons.Count}");

        // Subscribe to all linked buttons
        foreach (ButtonController button in linkedButtons)
        {
            if (button != null)
            {
                button.OnButtonPressed += OnButtonPressed;
                button.OnButtonReleased += OnButtonReleased;
                Debug.Log($"Subscribed to button: {button.gameObject.name}");
            }
        }

        UpdateDoorState();
    }

    void OnButtonPressed(ButtonController button)
    {
        Debug.Log($"ButtonDoor: Button {button.gameObject.name} pressed!");

        if (isUnlocked) return; // Already unlocked

        UpdateDoorState();
    }

    void OnButtonReleased(ButtonController button)
    {
        Debug.Log($"ButtonDoor: Button {button.gameObject.name} released!");

        UpdateDoorState();
    }

    void UpdateDoorState()
    {
        // Count currently pressed buttons
        currentlyPressedButtons = 0;
        foreach (ButtonController button in linkedButtons)
        {
            if (button != null && button.IsPressed())
            {
                currentlyPressedButtons++;
            }
        }

        Debug.Log($"ButtonDoor: {currentlyPressedButtons}/{buttonsRequired} buttons pressed");

        // Check if door should unlock
        bool shouldUnlock = false;

        if (requireAllButtons)
        {
            // Need ALL required buttons pressed
            shouldUnlock = (currentlyPressedButtons >= buttonsRequired);
        }
        else
        {
            // Need ANY button pressed (at least 1)
            shouldUnlock = (currentlyPressedButtons >= 1);
        }

        if (shouldUnlock && !isUnlocked)
        {
            UnlockDoor();
        }
        else if (!shouldUnlock && isUnlocked)
        {
            LockDoor();
        }
    }

    void UnlockDoor()
    {
        isUnlocked = true;

        // Change sprite
        if (spriteRenderer != null && unlockedDoorSprite != null)
        {
            spriteRenderer.sprite = unlockedDoorSprite;
        }

        // Disable collider
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // Play sound
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }

        Debug.Log($"ButtonDoor {gameObject.name} UNLOCKED!");
    }

    void LockDoor()
    {
        isUnlocked = false;

        // Change sprite back
        if (spriteRenderer != null && lockedDoorSprite != null)
        {
            spriteRenderer.sprite = lockedDoorSprite;
        }

        // Enable collider
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }

        Debug.Log($"ButtonDoor {gameObject.name} LOCKED!");
    }

    // helper methods
    public bool IsUnlocked() { return isUnlocked; }
    public int GetPressedButtonCount() { return currentlyPressedButtons; }
    public int GetTotalButtonCount() { return linkedButtons.Count; }

}
