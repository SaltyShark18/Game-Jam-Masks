using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMask : MonoBehaviour
{
    // settings
    public bool spiritMask = false;
    public AudioSource spiritMaskAudio;
    public AudioSource noMaskAudio;

    // Start is called before the first frame update
    void Start()
    {
        HideSpiritObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleSpiritMode();
        }
    }

    public void ToggleSpiritMode()
    {
        if (!spiritMask)
        {
            ActivateSpiritMode();
        }
        else
        {
            DeactivateSpiritMode();
        }
    }

    void ActivateSpiritMode()
    {
        spiritMask = true;
        if (!spiritMaskAudio.isPlaying)
        {
            spiritMaskAudio.Play();
        }
        //Debug.Log("Spirit Mask On");
        ShowSpiritObjects();
    }

    void DeactivateSpiritMode()
    {
        spiritMask = false;
        if (!noMaskAudio.isPlaying)
        {
            noMaskAudio.Play();
        }
        //Debug.Log("Spirit Mask Off");
        HideSpiritObjects();
    }

    void ShowSpiritObjects() // activate spirit objects
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int activatedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Spirit") && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                activatedCount++;
            }
        }
        //Debug.Log($"Activated {activatedCount} spirit objects");
    }

    void HideSpiritObjects() // deactivate spirit objects
    {
        GameObject[] spiritObjects = GameObject.FindGameObjectsWithTag("Spirit");
        foreach (GameObject obj in spiritObjects)
        {
            obj.SetActive(false);
        }
        //Debug.Log($"Hidden {spiritObjects.Length} spirit objects");
    }

}
