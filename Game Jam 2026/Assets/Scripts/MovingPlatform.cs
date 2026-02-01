using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Vector3 nextPosition;
    private Transform currentPlayer = null;
    public bool isParentingEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 oldPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }

        if (currentPlayer != null && isParentingEnabled)
        {
            Vector3 movement = transform.position - oldPosition;
            currentPlayer.position += movement;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isParentingEnabled) return;
        if (collision.gameObject.CompareTag("Player") && gameObject.activeSelf)
        {
            if (currentPlayer == null)
            {
                currentPlayer = collision.transform;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isParentingEnabled) return;
        if (collision.gameObject.CompareTag("Player") && currentPlayer == collision.transform)
        {
            currentPlayer = null;
        }
    }

    public void SetParentingEnabled(bool enabled)
    {
        isParentingEnabled = enabled;

        if (!enabled && currentPlayer != null)
        {
            currentPlayer = null;
        }
    }
}