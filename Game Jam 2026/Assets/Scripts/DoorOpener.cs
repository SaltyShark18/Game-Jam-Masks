using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private Animator doorAnimator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        doorAnimator = GetComponent<Animator>();
    }

    // Example function to call when the door should open
    public void OpenDoor()
    {
        // Set the trigger parameter in the Animator to start the animation
        doorAnimator.SetTrigger("OpenTrigger");
    }

    // Example for trigger-based opening (if Is Trigger is checked on the collider)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player (assuming player has the tag "Player")
        if (other.CompareTag("Player"))
        {
            OpenDoor();
            // Optional: disable the collider after opening if needed
            // GetComponent<Collider2D>().enabled = false; 
        }
    }
}
