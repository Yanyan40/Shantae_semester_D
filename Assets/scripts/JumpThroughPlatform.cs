using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    private Collider platformCollider;
    private Renderer platformRenderer;

    public Material normalMaterial;
    public Material triggeredMaterial;

    private bool playerOnPlatform = false;

    void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
        platformCollider.isTrigger = true;  // Start as trigger
    }

    void Update()
    {
        // Check if the player is on the platform and pressing the down arrow
        if (playerOnPlatform)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                platformCollider.isTrigger = true;  // Allow player to fall through
                platformRenderer.material = triggeredMaterial;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                platformCollider.isTrigger = false;  // Set platform back to solid
                platformRenderer.material = normalMaterial;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            platformCollider.isTrigger = false;  // Ensure the platform is solid when the player first touches it
            platformRenderer.material = normalMaterial;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
            platformCollider.isTrigger = true;  // Reset platform to trigger when player leaves
            platformRenderer.material = normalMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BottomDetector"))
        {
            platformCollider.isTrigger = false;  // If the bottom detector is on the platform, make it solid
            platformRenderer.material = triggeredMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BottomDetector"))
        {
            platformCollider.isTrigger = true;  // Set to trigger again when bottom detector leaves
            platformRenderer.material = normalMaterial;
        }
    }
}
