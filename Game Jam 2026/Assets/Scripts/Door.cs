using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int requiredKeyID = 1;       // number of keys needed
    public int requiredKeyAmount = 1;   // key id; 1=forestKey, 2=___key, 3=mansionKey
    public Sprite openDoorSprite;
    private Collider2D blockerCollider;
    private Collider2D triggerCollider;

    void Start()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            if (col.isTrigger)
                triggerCollider = col;
            else
                blockerCollider = col;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KeyManager keyManager = other.GetComponent<KeyManager>();

            if (keyManager != null && keyManager.UseKeys(requiredKeyID, requiredKeyAmount))
            {
                OpenDoor();
            }
            else
            {
                Debug.Log($"Need {requiredKeyAmount} key(s) with ID {requiredKeyID}");
            }
        }
    }

    void OpenDoor()
    {
        Debug.Log("Door opened!");
        if (openDoorSprite != null)
            GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        if (blockerCollider != null)
            blockerCollider.enabled = false;
        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }
}