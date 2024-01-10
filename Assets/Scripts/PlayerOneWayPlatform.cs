using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime;
    private PlayerMovementScript playerMovementScript;

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    public void Drop(InputAction.CallbackContext ctx)
    {
        // Check if the drop action is performed
        if (ctx.performed)
        {
            // Rotate the platform effector to enable dropping through one-way platforms
            effector.rotationalOffset = 180f;
            // Uncomment the line below if you want to reset rotation after a delay
            // StartCoroutine(ResetRotation());
        }

        // Check if the drop action is canceled
        if (ctx.canceled)
        {
            // Reset the rotational offset to disable dropping through one-way platforms
            effector.rotationalOffset = 0f;
        }
    }

    // Coroutine to reset the rotation after a delay
    private IEnumerator ResetRotation()
    {
        // Wait for a short duration before resetting the rotation
        yield return new WaitForSeconds(0.1f);

        // Check if the wait time condition is met
        if (waitTime <= 0)
        {
            // Reset the rotational offset to disable dropping through one-way platforms
            effector.rotationalOffset = 0f;
        }
    }
}