using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float shootInterval = 2f;
    public Transform player;
    public GameObject landingPredictionPrefab; // Drag your landing prediction GameObject prefab here
    private float timer = 0f;
    private GameObject activePredictionObject;
    private float yOffset = 0.05f; // Small Y offset

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned! Cannot shoot.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Vector3 direction = (player.position - shootingPoint.position).normalized;
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            projectileRb.AddForce(direction * 10f, ForceMode.Impulse);
            ShowLandingPrediction(projectile.transform.position, direction);
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component!");
        }
    }

    void ShowLandingPrediction(Vector3 startPosition, Vector3 direction)
    {
        if (landingPredictionPrefab != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPosition, Vector3.down, out hit))
            {
                Vector3 predictionPosition = hit.point;
                predictionPosition.y += yOffset; // Apply the Y offset

                if (activePredictionObject != null)
                {
                    Destroy(activePredictionObject);
                }

                activePredictionObject = Instantiate(landingPredictionPrefab, predictionPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No floor detected below the starting position!");
            }
        }
        else
        {
            Debug.LogError("Landing prediction prefab is not assigned!");
        }
    }
}
