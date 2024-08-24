using System.Collections;
using UnityEngine;

public class ShantaeBasicAttack : MonoBehaviour
{
    //public GameObject attackTrigger;
    public KeyCode attackKey = KeyCode.Z;
    public float attackDuration = 0.5f;

    private Coroutine attackCoroutine;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found.");
        }
    }

    void Update()
    {
        if (!playerController.isDead && Input.GetKeyDown(attackKey))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            attackCoroutine = StartCoroutine(ActivateAttackTrigger());
        }
    }

    private IEnumerator ActivateAttackTrigger()
    {
        //attackTrigger.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        //attackTrigger.SetActive(false);
    }
}
