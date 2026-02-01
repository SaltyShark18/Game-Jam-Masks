using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement library

public class SceneTrigger : MonoBehaviour
{
    // Optional: Expose the scene name or index in the Inspector
    [SerializeField] private string sceneToLoad; 

    // This function is called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger has a specific tag (e.g., "Player")
        if (other.CompareTag("Player"))
        {
            // Load the specified scene by name or build index
            SceneManager.LoadScene(sceneToLoad); 
        }
    }
}
