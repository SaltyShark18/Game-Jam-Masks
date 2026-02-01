using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMask : MonoBehaviour
{
    // settings
    public bool timeMask = false;

    private List<MovingPlatform> allMovingPlatforms = new List<MovingPlatform>();
    private List<float> originalSpeeds = new List<float>();
    public float slowDownFactor = 0.25f;

    public float maxCharge = 7f;    // Max time to freeze
    public float rechargeTime = 7f; // Time to recharge
    private float currentCharge = 7f;
    private float rechargeRate;

    // charge bar
    public RectTransform chargeBar;
    public RectTransform chargeBackground;
    private float originalBarLength;
    public Color activeColor = Color.cyan;
    public Color fullColor = Color.blue;
    public Color lowColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        rechargeRate = maxCharge / rechargeTime;

        CacheAllPlatforms();

        if (chargeBar != null && chargeBackground != null) // if charge bar present, determine length
        {
            originalBarLength = chargeBackground.sizeDelta.y;

            chargeBar.anchorMin = new Vector2(chargeBar.anchorMin.x, 0f);
            chargeBar.anchorMax = new Vector2(chargeBar.anchorMax.x, 0f);
            chargeBar.pivot = new Vector2(chargeBar.pivot.x, 0f);
            UpdateChargeBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeMask && currentCharge < maxCharge)
        {
            RechargeTimeMask();
        }
        
        if (timeMask && currentCharge > 0)
        {
            DrainTimeMask();
        }
        else if (timeMask && currentCharge <= 0)
        {
            DeactivateTimeMask();
        }

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleTimeMask();
        }*/
    }

    public void ToggleTimeMask()
    {
        if (timeMask)
        {
            DeactivateTimeMask();
        }
        else if (currentCharge > 0)
        {
            ActivateTimeMask();
        }
        else
        {
            Debug.Log("Time Mask needs to recharge!");
        }
    }

    void ActivateTimeMask()
    {
        if (timeMask || currentCharge <= 0) return;

        timeMask = true;

        SoundEffectManager.Play("MaskSwap");

        for (int i = 0; i < allMovingPlatforms.Count; i++)
        {
            if (allMovingPlatforms[i] != null)
            {
                // Store the current speed before slowing (in case it was already modified)
                if (!timeMask) // Only store original speed on first activation
                {
                    originalSpeeds[i] = allMovingPlatforms[i].moveSpeed;
                }

                // Apply slowdown
                allMovingPlatforms[i].moveSpeed = originalSpeeds[i] * slowDownFactor;

                Debug.Log($"Slowing platform: {allMovingPlatforms[i].gameObject.name}, " +
                         $"Tag: {allMovingPlatforms[i].gameObject.tag}, " +
                         $"Active: {allMovingPlatforms[i].gameObject.activeSelf}, " +
                         $"New Speed: {allMovingPlatforms[i].moveSpeed}");
            }
        }

        Debug.Log("Time Mask Activated");
        UpdateChargeBar();
    }

    void DeactivateTimeMask()
    {
        if (!timeMask) return;

        timeMask = false;

        for (int i = 0; i < allMovingPlatforms.Count; i++)
        {
            if (allMovingPlatforms[i] != null)
            {
                allMovingPlatforms[i].moveSpeed = originalSpeeds[i];
            }
        }
        Debug.Log("Time Mask Deactivated");
        UpdateChargeBar();
    }

    void RechargeTimeMask()
    {
        currentCharge += rechargeRate * Time.deltaTime;
        currentCharge = Mathf.Min(currentCharge, maxCharge);
        UpdateChargeBar();
    }

    void DrainTimeMask()
    {
        currentCharge -= Time.deltaTime;
        currentCharge = Mathf.Max(currentCharge, 0f);
        UpdateChargeBar();
    }

    void UpdateChargeBar()
    {
        if (chargeBar != null)
        {
            float chargePercent = currentCharge / maxCharge;
            Vector2 newSize = chargeBar.sizeDelta;
            newSize.y = originalBarLength * chargePercent;
            chargeBar.sizeDelta = newSize;
            Image barImage = chargeBar.GetComponent<Image>();
            if (barImage != null)
            {
                barImage.color = Color.Lerp(lowColor, fullColor, chargePercent);
                if (timeMask)
                {
                    barImage.color = activeColor;
                }
            }
        }
    }

    void CacheAllPlatforms()
    {
        // Clear lists
        allMovingPlatforms.Clear();
        originalSpeeds.Clear();

        // Find ALL objects in the scene including inactive ones
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            // Only include objects in the current scene (not prefabs)
            if (obj.scene.IsValid())
            {
                MovingPlatform platform = obj.GetComponent<MovingPlatform>();
                if (platform != null)
                {
                    allMovingPlatforms.Add(platform);
                    originalSpeeds.Add(platform.moveSpeed);
                }
            }
        }

        Debug.Log($"TimeMask: Found {allMovingPlatforms.Count} total moving platforms (including inactive)");

        // Log all platforms found
        for (int i = 0; i < allMovingPlatforms.Count; i++)
        {
            if (allMovingPlatforms[i] != null)
            {
                GameObject platformObj = allMovingPlatforms[i].gameObject;
                Debug.Log($"  Platform {i}: {platformObj.name}, " +
                         $"Tag: {platformObj.tag}, " +
                         $"Active: {platformObj.activeSelf}, " +
                         $"Speed: {originalSpeeds[i]}");
            }
        }
    }

    // Refresh the platform list (call this if new platforms are created at runtime)
    public void RefreshPlatformList()
    {
        CacheAllPlatforms();

        if (timeMask)
        {
            for (int i = 0; i < allMovingPlatforms.Count; i++)
            {
                if (allMovingPlatforms[i] != null)
                {
                    allMovingPlatforms[i].moveSpeed = originalSpeeds[i] * slowDownFactor;
                }
            }
        }
    }

    // Helper Methods /////////////////////////////////////////
    public bool HasCharge()
    {
        return currentCharge > 0;
    }

    public float GetChargePercentage()
    {
        return currentCharge / maxCharge;
    }

    public float GetCurrentCharge()
    {
        return currentCharge;
    }

}
