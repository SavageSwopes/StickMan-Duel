using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] Transform teleportDestination;
    [SerializeField] float cooldownTime = 1.0f; // Delay in seconds before teleporting the player
    [SerializeField] private Vector2 offset;

    private static float lastTeleportTime;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the teleportation zone.");
            // Teleport the player to the specified position
            if (Time.time - lastTeleportTime < cooldownTime)
            {
                Debug.Log("Teleportation is on cooldown. Please wait.");
                return; // Exit if teleportation is still on cooldown
            }
           Vector2 finalPosition = (Vector2)teleportDestination.position + offset;
            other.transform.position = finalPosition;
            lastTeleportTime = Time.time; // Update the last teleportation time
        }

    }


}
