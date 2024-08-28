using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public Transform playerTransform; 
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    void Update()
    {
        if (playerTransform != null)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        
        if (playerTransform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
