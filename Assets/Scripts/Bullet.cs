using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f; // Destroys the bullet after 3 seconds so it doesn't lag your game

    void Start()
    {
        // Push the bullet forward in the direction its right (red) arrow is pointing
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;

        // Clean up the bullet automatically after a few seconds
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the bullet if it hits anything (like the player or a wall)
        // Ignore other triggers so bullets don't destroy each other mid-air
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
