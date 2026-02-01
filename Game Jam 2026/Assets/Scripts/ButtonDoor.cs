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
    private Collider2D blockerCollider;

    public List<ButtonController> linkedButtons = new List<ButtonController>();
    private bool isUnlocked = false;
    private int currentlyPressedButtons = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            lockedDoorSprite = spriteRenderer.sprite;
        }

        blockerCollider = GetComponent<Collider2D>();
        if (blockerCollider == null)
        {
            Debug.LogError("ButtonDoor: NO COLLIDER FOUND ON DOOR!");
        }
        else
        {
            Debug.Log($"ButtonDoor: Found collider: {blockerCollider.name}, IsTrigger: {blockerCollider.isTrigger}");

            // If it's a trigger, it won't block the player
            if (blockerCollider.isTrigger)
            {
                Debug.LogWarning("ButtonDoor: Collider is set as Trigger! Should NOT be trigger to block player.");
            }
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

        SoundEffectManager.Play("DoorOpen");

        // Change sprite
        if (spriteRenderer != null && unlockedDoorSprite != null)
        {
            spriteRenderer.sprite = unlockedDoorSprite;
        }

        SoundEffectManager.Play("DoorOpen");

        if (blockerCollider != null)
            blockerCollider.enabled = false;

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
        if (blockerCollider != null)
        {
            blockerCollider.enabled = true;
        }

        Debug.Log($"ButtonDoor {gameObject.name} LOCKED!");
    }

    // helper methods
    public bool IsUnlocked() { return isUnlocked; }
    public int GetPressedButtonCount() { return currentlyPressedButtons; }
    public int GetTotalButtonCount() { return linkedButtons.Count; }

}