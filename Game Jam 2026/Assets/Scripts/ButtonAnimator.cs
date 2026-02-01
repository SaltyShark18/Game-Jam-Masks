using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ButtonAnimator : MonoBehaviour
{
    public Animator targetAnimator; // Drag the GameObject with the Animator here in the Inspector
    private const string PRESS_TRIGGER_NAME = "Press"; // Match the name in the Animator

    public void PlayPressAnimation()
    {
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(PRESS_TRIGGER_NAME);
        }
    }
}
