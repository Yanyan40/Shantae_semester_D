using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float shootInterval = 2f;
    public Transform player;
    private float timer = 0f;

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
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component!");
        }
    }
}
