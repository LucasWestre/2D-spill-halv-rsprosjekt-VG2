using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Setup")]
    public int obstacleLayer;
    public Transform respawnPoint;

    [Header("Audio Settings")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float volume = 1f;

    void Start()
    {
        if (obstacleLayer == -1)
        {
            Debug.LogError("Layer 'Obstacle' does not exist! Please create it in the Unity Editor.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == obstacleLayer)
        {
            Respawn();
        }
    }

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

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, volume);
        }

        transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, transform.position.z);
        transform.rotation = respawnPoint.rotation;

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
            rb2d.linearVelocity = Vector2.zero; 
            rb2d.angularVelocity = 0f;
        }
    }
}
