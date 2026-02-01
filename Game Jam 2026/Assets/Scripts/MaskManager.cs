using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaskType
{
    Spirit,
    Time,
    Fire
}

public class MaskManager : MonoBehaviour
{
    // settings
    public MaskType currentMask = MaskType.Spirit;
    public SpiritMask spiritMask;
    public TimeMask timeMask;
    public FireMask fireMask;

    public SpriteSwapper spriteSwapper;

    public float maskSwitchCooldown = 0.3f;
    private float lastSwitchTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        SetActiveMask(currentMask);
    }

    // Update is called once per frame
    void Update()
    {
        // Switch masks with Left Shift
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastSwitchTime)
        {
            SwitchToNextMask();
            lastSwitchTime = Time.time + maskSwitchCooldown;
        }
        
        // Activate current mask ability with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateCurrentMask();
        }
        
        // Also can select masks with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveMask(MaskType.Spirit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveMask(MaskType.Time);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveMask(MaskType.Fire);
        }
    }
    
    void SwitchToNextMask()
    {
        // Cycle through masks
        switch (currentMask)
        {
            case MaskType.Spirit:
                SetActiveMask(MaskType.Time);
                break;
            case MaskType.Time:
                SetActiveMask(MaskType.Fire);
                break;
            case MaskType.Fire:
                SetActiveMask(MaskType.Spirit);
                break;
        }
        
        // Play mask switch sound
        SoundEffectManager.Play("MaskSwap");
        
        // Update sprite swapper if available
        if (spriteSwapper != null)
        {
            spriteSwapper.ToggleSprites();
        }
    }
    
    void SetActiveMask(MaskType newMask)
    {
        // Disable all masks first
        if (spiritMask != null) spiritMask.enabled = false;
        //if (timeMask != null) timeMask.enabled = false;
        if (fireMask != null) fireMask.enabled = false;
        
        // Enable the selected mask
        currentMask = newMask;
        
        switch (currentMask)
        {
            case MaskType.Spirit:
                if (spiritMask != null) spiritMask.enabled = true;
                break;
            case MaskType.Time:
                if (timeMask != null) timeMask.enabled = true;
                break;
            case MaskType.Fire:
                if (fireMask != null) fireMask.enabled = true;
                break;
        }
        if (spriteSwapper != null)
        {
            spriteSwapper.SetMask(currentMask);
        }
        Debug.Log($"Switched to {currentMask} Mask");
    }
    
    void ActivateCurrentMask()
    {
        switch (currentMask)
        {
            case MaskType.Spirit:
                if (spiritMask != null && spiritMask.enabled)
                {
                    spiritMask.ToggleSpiritMode();
                }
                break;
                
            case MaskType.Time:
                if (timeMask != null && timeMask.enabled)
                {
                    timeMask.ToggleTimeMask();
                }
                break;
                
            case MaskType.Fire:
                if (fireMask != null && fireMask.enabled)
                {
                    fireMask.ForceShoot();
                }
                break;
        }
    }
    
    // Public method to get current mask
    public MaskType GetCurrentMask()
    {
        return currentMask;
    }
    
    // Public method to switch to specific mask
    public void SwitchToMask(MaskType maskType)
    {
        SetActiveMask(maskType);
    }

}
