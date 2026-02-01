using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    [System.Serializable]
    public class MaskSprites
    {
        public string maskName;
        public Sprite idleSprite;
        public Sprite walkSprite;
    }

    [Header("Sprite Renderers")]
    public SpriteRenderer idleRenderer;
    public SpriteRenderer walkRenderer;

    [Header("Mask Sprites Collection")]
    public List<MaskSprites> allMaskSprites = new List<MaskSprites>();

    [Header("Default Sprites")]
    public Sprite defaultIdle;
    public Sprite defaultWalk;

    private int currentMaskIndex = 0;

    void Start()
    {
        if (idleRenderer == null || walkRenderer == null)
        {
            Debug.LogError("SpriteSwapper: Missing SpriteRenderer references!");
            return;
        }

        // Initialize with first mask or default
        if (allMaskSprites.Count > 0)
        {
            SetMask(0);
        }
        else
        {
            idleRenderer.sprite = defaultIdle;
            walkRenderer.sprite = defaultWalk;
        }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ToggleSprites();
            SoundEffectManager.Play("MaskSwap");
        }
    }*/

    public void ToggleSprites()
    {
        currentMaskIndex = (currentMaskIndex + 1) % allMaskSprites.Count;
        SetMask(currentMaskIndex);
    }

    public void SetMask(int index)
    {
        if (index < 0 || index >= allMaskSprites.Count)
        {
            Debug.LogError($"Invalid mask index: {index}");
            return;
        }

        currentMaskIndex = index;
        MaskSprites mask = allMaskSprites[index];

        if (mask.idleSprite != null)
            idleRenderer.sprite = mask.idleSprite;
        if (mask.walkSprite != null)
            walkRenderer.sprite = mask.walkSprite;

        Debug.Log($"Swapped to {mask.maskName} mask sprites");
    }

    public void SetMask(MaskType maskType)
    {
        // Convert MaskType to string that matches your list
        string maskName = "";
        switch (maskType)
        {
            case MaskType.Spirit:
                maskName = "Spirit";
                break;
            case MaskType.Time:
                maskName = "Time";
                break;
            case MaskType.Fire:
                maskName = "Fire";
                break;
        }

        // Find and set the mask
        bool found = false;
        for (int i = 0; i < allMaskSprites.Count; i++)
        {
            if (allMaskSprites[i].maskName == maskName)
            {
                SetMask(i);
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogError($"No sprite entry found for mask: {maskName}. Check your 'All Mask Sprites' list in Inspector.");
        }
    }

    // Set to default (no mask)
    public void SetDefault()
    {
        if (defaultIdle != null)
            idleRenderer.sprite = defaultIdle;
        if (defaultWalk != null)
            walkRenderer.sprite = defaultWalk;
    }
}