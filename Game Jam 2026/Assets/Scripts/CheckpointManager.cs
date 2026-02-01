using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 pos, Quaternion rot)
    {
        savedPosition = pos;
        savedRotation = rot;
        hasCheckpoint = true;
        Debug.Log("Checkpoint saved at: " + savedPosition);
    }

    public void RespawnPlayer(Transform player)
    {
        if (!hasCheckpoint)
        {
            Debug.Log("No checkpoint set yet!");
            return;
        }

        player.position = savedPosition;
        player.rotation = savedRotation;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        RestorePlayerHealth(player.gameObject);
    }

    private void RestorePlayerHealth(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreFullHealth();
        }
        else
        {
            Debug.LogWarning("No PlayerHealth component found on player!");
        }
    }
}
