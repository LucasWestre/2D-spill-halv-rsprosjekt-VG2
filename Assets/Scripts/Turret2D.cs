using UnityEngine;

public class Turret2D : MonoBehaviour
{
    [Header("Turret Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f; // Time in seconds between shots

    [Header("Audio Settings")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float volume = 0.7f;

    private float fireTimer;

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Spawn the bullet at the FirePoint's exact position and rotation
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Play shooting sound if assigned
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, firePoint.position, volume);
        }
    }
}
