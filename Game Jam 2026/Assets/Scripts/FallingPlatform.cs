using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 2f;
    public float destroyWait = 1f;

    bool isFalling;
    Rigidbody2D rb;

    private float spikeXPos;
    private float spikeYPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spikeXPos = rb.position.x;
        spikeYPos = rb.position.y;
        // GameController.OnReset += ResetObject;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    
    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyWait);
        gameObject.SetActive(false);
        rb.transform.position = new Vector3(spikeXPos, spikeYPos, 0);
        rb.bodyType = RigidbodyType2D.Static;
        isFalling = false;
    }

    void ResetObject()
    {
        gameObject.SetActive(true);
    }
}

