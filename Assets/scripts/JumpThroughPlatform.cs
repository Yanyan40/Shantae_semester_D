using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    private Collider platformCollider;
    private Renderer platformRenderer;

    public Material normalMaterial;
    public Material triggeredMaterial;

    void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BottomDetector"))
        {
            platformCollider.isTrigger = false;
            platformRenderer.material = triggeredMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BottomDetector"))
        {
            platformCollider.isTrigger = true;
            platformRenderer.material = normalMaterial;
        }
    }
}
