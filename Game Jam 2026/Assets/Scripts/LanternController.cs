using UnityEngine;

public class LanternController : MonoBehaviour
{
    // Reference to the new sprite you want to use when lit
    public Sprite litSprite; 
    // Reference to the SpriteRenderer component (will be found automatically in Start)
    private SpriteRenderer spriteRenderer; 
    // Variable to store the original unlit sprite
    private Sprite unlitSprite;
    private bool isLit = false;

    public System.Action<LanternController> OnLanternLit;

    public GameObject lightingEffect;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on " + gameObject.name);
        }
        // Store the original sprite
        unlitSprite = spriteRenderer.sprite;
        if (litSprite == null)
        {
            Debug.LogError("LitSprite is not assigned in Inspector on " + gameObject.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)      // call on collision
    {
        //Debug.Log("Collision with: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

        // Check if the colliding object has the "Fireball" tag
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Change the sprite to the lit sprite
            //ChangeToLitSprite();
            LightLantern(collision.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)     // also call on trigger
    {
        //Debug.Log("Trigger with: " + other.gameObject.name + ", Tag: " + other.gameObject.tag);

        if (other.CompareTag("Fireball") && !isLit)
        {
            Debug.Log($"Torch {gameObject.name}: Fireball trigger detected!");
            LightLantern(other.gameObject);
        }
    }

    void LightLantern(GameObject fireball)
    {
        isLit = true;
        spriteRenderer.sprite = litSprite;

        if (litSprite != null)
        {
            spriteRenderer.sprite = litSprite;
        }
        else
        {
            Debug.LogError("Lit sprite is not assigned in the Inspector!");
        }

        OnLanternLit?.Invoke(this);

        /*if (lightingEffect != null)       // for lighting effect (if assigned)
        {
            Instantiate(lightingEffect, transform.position, Quaternion.identity);
        }*/

        //Debug.Log("Lantern lit!");
    }

    // helper methods
    public bool IsLit()
    {
        return isLit;
    }

    public void ResetLantern()
    {
        isLit = false;
        spriteRenderer.sprite = unlitSprite;
    }

    // A function to handle the sprite change       // removed method
    /*void ChangeToLitSprite()
    {
        // Ensure the sprite is not already the lit one to avoid redundant calls
        if (spriteRenderer.sprite != OrangeLantern)
        {
            spriteRenderer.sprite = OrangeLantern;
            // Optional: Destroy the fireball after it hits the lantern
            Destroy(collision.gameObject);
        }
    }*/
}
