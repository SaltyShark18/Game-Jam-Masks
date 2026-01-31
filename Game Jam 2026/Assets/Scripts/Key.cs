using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID = 1;
    public float floatHeight = 1f;   // How high it floats above the player
    public float orbitDistance = 1.5f;  // Distance from the player
    public float orbitSpeed = 2f; // Orbit speed
    public float orbitRadius = 1.5f;    // Orbit radius

    private Transform player;
    private int orbitIndex = 0;
    private float orbitAngle = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player == null)
        {
            player = other.transform;
            KeyManager keyManager = player.GetComponent<KeyManager>();
           
            if (keyManager != null)
            {
                orbitIndex = keyManager.keys.Count;
                keyManager.AddKey(this);
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            orbitAngle += Time.deltaTime * orbitSpeed;

            float angle = orbitAngle + (orbitIndex * Mathf.PI * 0.5f);
            float x = Mathf.Cos(angle) * orbitRadius;
            float y = Mathf.Sin(angle) * orbitRadius + floatHeight;

            transform.position = player.position + new Vector3(x, y, 0);
        }
    }
}