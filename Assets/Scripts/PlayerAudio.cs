using UnityEngine;
using System.Collections;

public class PlayerFootstepsTimer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float stepInterval = 0.4f; // Time between steps

    private Rigidbody2D rb;
    private bool isWalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is moving based on velocity magnitude
        if (rb.linearVelocity.magnitude > 0.1f && !isWalking)
        {
            StartCoroutine(FootstepRoutine());
        }
        else if (rb.linearVelocity.magnitude <= 0.1f && isWalking)
        {
            isWalking = false;
            StopAllCoroutines();
        }
    }

    IEnumerator FootstepRoutine()
    {
        isWalking = true;

        while (isWalking)
        {
            audioSource.PlayOneShot(footstepClip);
            yield return new WaitForSeconds(stepInterval); // Wait before the next step
        }
    }
}
