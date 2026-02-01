using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDoor : MonoBehaviour
{
    // settings
    public int torchesRequired = 1;
    private bool isUnlocked = false;
    private SpriteRenderer spriteRenderer;
    private Sprite lockedDoorSprite;
    public Sprite unlockedDoorSprite;
    public List<LanternController> linkedTorches = new List<LanternController>();
    private int currentlyLitTorches = 0;
    private Collider2D blockerCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            lockedDoorSprite = spriteRenderer.sprite;
            Debug.Log($"Found sprite renderer. Locked sprite: {lockedDoorSprite?.name}");
        }
        else
        {
            Debug.LogError("NO SPRITE RENDERER FOUND ON DOOR!");
        }

        blockerCollider = GetComponent<Collider2D>();
        if (blockerCollider == null)
        {
            Debug.LogError("NO COLLIDER FOUND ON DOOR! Door needs a Collider2D to block player.");
        }
        else
        {
            Debug.Log($"Found door collider: {blockerCollider.name}, IsTrigger: {blockerCollider.isTrigger}");

            // If it's a trigger, it won't block the player - make sure it's NOT a trigger
            if (blockerCollider.isTrigger)
            {
                Debug.LogWarning("Door collider is set as Trigger! It should NOT be a trigger to block player.");
            }
        }

        //Debug.Log($"Torches required: {torchesRequired}");
        //Debug.Log($"Linked torches count: {linkedTorches.Count}");

        // Subscribe to all linked torches
        foreach (LanternController torch in linkedTorches)
        {
            if (torch != null)
            {
                Debug.Log($"Subscribing to torch: {torch.gameObject.name}");
                torch.OnLanternLit += OnTorchLit;
            }
            else
            {
                Debug.LogWarning("Null torch in linkedTorches list!");
            }
        }

        UpdateDoorState();
    }

    void OnTorchLit(LanternController torch)
    {
        //Debug.Log($"OnTorchLit called! Torch: {torch?.gameObject?.name}");

        if (isUnlocked) // Already unlocked
        {
            Debug.Log("Door already unlocked, ignoring torch.");
            return;
        }

        currentlyLitTorches++;
        //Debug.Log($"Torch lit! {currentlyLitTorches}/{torchesRequired} torches lit");

        CheckDoorUnlock();
    }

    void CheckDoorUnlock()
    {
        //Debug.Log($"CheckDoorUnlock: {currentlyLitTorches}/{torchesRequired}, isUnlocked: {isUnlocked}");
        if (currentlyLitTorches >= torchesRequired && !isUnlocked)
        {
            //Debug.Log($"UNLOCKING DOOR: {gameObject.name}");
            UnlockDoor();
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
        else
        {
            if (spriteRenderer == null) Debug.LogError("No sprite renderer!");
            if (unlockedDoorSprite == null) Debug.LogError("No unlocked door sprite assigned!");
        }

        Debug.Log("Door unlocked!");

        SoundEffectManager.Play("DoorOpen");

        if (blockerCollider != null)
            blockerCollider.enabled = false;
    }

    void UpdateDoorState()
    {
        // Count currently lit torches
        currentlyLitTorches = 0;
        foreach (LanternController torch in linkedTorches)
        {
            if (torch != null && torch.IsLit())
            {
                currentlyLitTorches++;
            }
        }
        //Debug.Log($"Total lit torches: {currentlyLitTorches}");
        CheckDoorUnlock();
    }

    // helper methods
    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public int GetLitTorchCount()
    {
        return currentlyLitTorches;
    }

    public int GetTotalTorchCount()
    {
        return linkedTorches.Count;
    }

    public void ResetDoor()
    {
        isUnlocked = false;
        currentlyLitTorches = 0;

        if (spriteRenderer != null && lockedDoorSprite != null)
        {
            spriteRenderer.sprite = lockedDoorSprite;
        }

        if (blockerCollider != null)
        {
            blockerCollider.enabled = true;
        }
    }
}
