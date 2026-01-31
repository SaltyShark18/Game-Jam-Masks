using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0.2f, -5f);
    public bool followPosition = true;

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
    }
}