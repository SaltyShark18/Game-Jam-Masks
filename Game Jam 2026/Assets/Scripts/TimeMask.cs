using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMask : MonoBehaviour
{
    // settings
    public bool timeMask = false;
    public AudioSource timeMaskAudio;
    public AudioSource noMaskAudio;
    private MovingPlatform[] allMovingPlatforms;
    private float[] originalSpeeds;
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

        allMovingPlatforms = FindObjectsOfType<MovingPlatform>();
        originalSpeeds = new float[allMovingPlatforms.Length];

        for (int i = 0; i < allMovingPlatforms.Length; i++) // grab all moving platforms into a list
        {
            originalSpeeds[i] = allMovingPlatforms[i].moveSpeed;
        }

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleTimeMask();
        }
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

        if (!timeMaskAudio.isPlaying)
        {
            SoundEffectManager.Play("Time Stop");
        }

        for (int i = 0; i < allMovingPlatforms.Length; i++)
        {
            if (allMovingPlatforms[i] != null)
            {
                allMovingPlatforms[i].moveSpeed = originalSpeeds[i] * slowDownFactor;
            }
        }
        Debug.Log("Time Mask Activated");
        UpdateChargeBar();
    }

    void DeactivateTimeMask()
    {
        if (!timeMask) return;

        timeMask = false;

        if (!noMaskAudio.isPlaying)
        {
            noMaskAudio.Play();
        }

        for (int i = 0; i < allMovingPlatforms.Length; i++)
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
