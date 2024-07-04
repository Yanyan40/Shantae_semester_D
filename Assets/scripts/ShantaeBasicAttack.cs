using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantaeBasicAttack : MonoBehaviour
{
    public GameObject attackTrigger; 
    public KeyCode attackKey = KeyCode.Z;
    public float attackDuration = 0.5f; 

    private Coroutine attackCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
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
        attackTrigger.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackTrigger.SetActive(false);
    }

}
