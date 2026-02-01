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
    public Collider2D doorCollider;
    public List<LanternController> linkedTorches = new List<LanternController>();
    private int currentlyLitTorches = 0;

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

        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }

        if (doorCollider != null)
        {
            Debug.Log($"Door collider found: {doorCollider.name}. Enabled: {doorCollider.enabled}. IsTrigger: {doorCollider.isTrigger}");
        }
        else
        {
            Debug.LogWarning("No door collider found!");
        }

        Debug.Log($"Torches required: {torchesRequired}");
        Debug.Log($"Linked torches count: {linkedTorches.Count}");

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

        // Disable collider so player can pass through
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        Debug.Log("Door unlocked!");

        // Optional: Play sound
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
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

        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }
    }
}
