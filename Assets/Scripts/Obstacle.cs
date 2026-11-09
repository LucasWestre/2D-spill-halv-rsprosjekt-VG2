using UnityEngine;

public class PlayerRespawn2D : MonoBehaviour
{
    // Assign your Respawn Point (an Empty GameObject) in the Inspector
    [SerializeField] private Transform respawnPoint; 

    private int obstacleLayer;

    private void Start()
    {
        // Convert the layer name "Obstacle" into its layer index
        obstacleLayer = LayerMask.NameToLayer("Obstacle");

        // Safety check to warn you if you forgot to create the layer in Unity
        if (obstacleLayer == -1)
        {
            Debug.LogError("Layer 'Obstacle' does not exist! Please create it in the Unity Editor.");
        }
    }

    // Triggered if your 2D obstacles have "Is Trigger" checked (e.g., zone triggers, water, pits)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == obstacleLayer)
        {
            Respawn();
        }
    }

    // Triggered if your 2D obstacles have solid collisions (e.g., solid wall hazards, spike objects)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == obstacleLayer)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("Cannot respawn! No Respawn Point assigned in the Inspector.");
            return;
        }

        // Teleport the player in 2D space
        transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, transform.position.z);
        transform.rotation = respawnPoint.rotation;

        // Reset 2D physics forces so the player doesn't carry over falling/running speed
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
            // Note: If using an older Unity version, use rb2d.velocity = Vector2.zero;
            rb2d.linearVelocity = Vector2.zero; 
            rb2d.angularVelocity = 0f;
        }
    }
}
