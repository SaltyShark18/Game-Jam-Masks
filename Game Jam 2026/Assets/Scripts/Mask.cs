using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0f, 0f);
    public bool followPosition = true;

    public GameObject MaskIdle;

    public GameObject MaskWalk;

    [SerializeField] public PlayerMovement pm;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Mask: No player assigned");
            return;
        }
        if (followPosition)
        {
            transform.position = player.position + offset;
        }

        if (pm.isWalking)
        {
            MaskIdle.SetActive(false);
            MaskWalk.SetActive(true);
        }
        else
        {
            MaskIdle.SetActive(true);
            MaskWalk.SetActive(false);
        }
    }
}