using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMask : MonoBehaviour
{
    // settings
    public bool spiritMask = false;
    private GameObject player;

    private List<GameObject> spiritObjects = new List<GameObject>();
    private List<GameObject> realObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CacheAllObjects();
        HideSpiritObjects();
        ShowRealObjects();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleSpiritMode();
        }*/
    }

    void CacheAllObjects()      // Cache all objects at start
    {
        GameObject[] allSpirit = GameObject.FindGameObjectsWithTag("Spirit");
        GameObject[] allReal = GameObject.FindGameObjectsWithTag("Real");

        spiritObjects.AddRange(allSpirit);
        realObjects.AddRange(allReal);
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
        if (spiritMask) return;

        spiritMask = true;
        DetachPlayerIfOnPlatform("Real");

        //Debug.Log("Spirit Mask On");
        StartCoroutine(SwitchToSpiritWorld());
    }

    void DeactivateSpiritMode()
    {
        if (!spiritMask) return;

        spiritMask = false;
        DetachPlayerIfOnPlatform("Spirit");

        //Debug.Log("Spirit Mask Off");
        StartCoroutine(SwitchToRealWorld());
    }

    IEnumerator SwitchToSpiritWorld()
    {
        SetPlatformParenting(false);
        DetachPlayerFromPlatforms("Real");
        HideRealObjects();
        yield return null;
        ShowSpiritObjects();
        SetPlatformParenting(true);
    }

    IEnumerator SwitchToRealWorld()
    {
        SetPlatformParenting(false);
        DetachPlayerFromPlatforms("Spirit");
        HideSpiritObjects();
        yield return null;
        ShowRealObjects();
        SetPlatformParenting(true);
    }

    void ShowSpiritObjects() // activate spirit objects
    {
        foreach (GameObject obj in spiritObjects)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
    }

    void ShowRealObjects() // activate spirit objects
    {
        foreach (GameObject obj in realObjects)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
    }

    void HideSpiritObjects() // deactivate spirit objects
    {
        foreach (GameObject obj in spiritObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                if (obj.CompareTag("Player")) continue;
                obj.SetActive(false);
            }
        }
    }

    void HideRealObjects() // deactivate spirit objects
    {
        foreach (GameObject obj in realObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                if (obj.CompareTag("Player")) continue;
                obj.SetActive(false);
            }
        }
    }

    void DetachPlayerFromPlatforms(string tag)
    {
        if (player == null) return;

        Transform currentParent = player.transform.parent;

        while (currentParent != null)
        {
            if (currentParent.CompareTag(tag))
            {
                player.transform.SetParent(null, true);
                Debug.Log($"Force-detached player from parent {currentParent.name} with tag {tag}");
                break;
            }
            currentParent = currentParent.parent;
        }
    }

    void DetachPlayerIfOnPlatform(string tag)
    {
        if (player == null) return;

        Transform playerTransform = player.transform;

        if (playerTransform.parent != null)
        {
            GameObject parentObj = playerTransform.parent.gameObject;
            if (parentObj.CompareTag(tag))
            {
                playerTransform.SetParent(null, true);

                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

                Debug.Log($"Player detached from {tag} platform");
            }
        }
    }

    void SetPlatformParenting(bool allow)
    {
        MovingPlatform[] allPlatforms = FindObjectsOfType<MovingPlatform>();
        foreach (MovingPlatform platform in allPlatforms)
        {
            platform.SetParentingEnabled(allow);
        }
    }
}