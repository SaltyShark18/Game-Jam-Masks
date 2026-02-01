using UnityEngine;

public class LanternController : MonoBehaviour
{
    // Reference to the new sprite you want to use when lit
    public Sprite litSprite; 
    // Reference to the SpriteRenderer component (will be found automatically in Start)
    private SpriteRenderer spriteRenderer; 
    // Variable to store the original unlit sprite
    private Sprite unlitSprite; 

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        // Store the original sprite
        unlitSprite = spriteRenderer.sprite; 
    }

    // Called when a collision occurs with another 2D collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Fireball" tag
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Change the sprite to the lit sprite
            ChangeToLitSprite();
        }
    }

    // A function to handle the sprite change
    void ChangeToLitSprite()
    {
        // Ensure the sprite is not already the lit one to avoid redundant calls
        if (spriteRenderer.sprite != litSprite)
        {
            spriteRenderer.sprite = litSprite;
            // Optional: Destroy the fireball after it hits the lantern
            Destroy(gameObject);
        }
    }
}
