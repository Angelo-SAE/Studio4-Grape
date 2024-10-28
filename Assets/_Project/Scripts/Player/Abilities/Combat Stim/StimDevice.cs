using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimDevice : MonoBehaviour
{
    
    private float aoeRadius;
    private float buffDuration;
    private float fireRateBuffPercentage;
    private float damageReductionPercentage;
    private float movementSpeedBuffPercentage;
    private bool applyLinger;
    private float lingerDuration;

    private Coroutine stimCoroutine;
    private Coroutine lingerCoroutine;
    private GameObject playerObject;
    private int playerLayerMask;

    private void Start()
    {
        playerLayerMask = LayerMask.GetMask("Player");
    }

    public void Initialize(float radius, float duration, float fireRateBuff, float damageReduction, float movementBuff, bool linger, float lingerDur)
    {
        aoeRadius = radius;
        buffDuration = duration;
        fireRateBuffPercentage = fireRateBuff;
        damageReductionPercentage = damageReduction;
        movementSpeedBuffPercentage = movementBuff;
        applyLinger = linger;
        lingerDuration = lingerDur;

        stimCoroutine = StartCoroutine(StimEffect());
        StartCoroutine(DestroyAfterDuration());
    }

    IEnumerator StimEffect()
    {
        while (true)
        {
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, aoeRadius, playerLayerMask);

            if (playersInRange.Length > 0)
            {
                playerObject = playersInRange[0].gameObject;

                PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
                ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();

                if (shooting != null)
                {
                    shooting.ModifyFireRate(1 - (fireRateBuffPercentage / 100f));
                }

                if (movement != null && movementSpeedBuffPercentage > 0)
                {
                    movement.movementMultipler = (1 + (movementSpeedBuffPercentage / 100f));
                }

                if (lingerCoroutine != null)
                {
                    StopCoroutine(lingerCoroutine);
                    lingerCoroutine = null;
                }
            }
            else if (playerObject != null && applyLinger && playersInRange.Length == 0)
            {
                PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
                ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();
                Debug.Log("aaaa");
                movement.StartLingerCoroutine(lingerDuration);
                playerObject = null;
            }
            else if (playerObject != null && !applyLinger && playersInRange.Length == 0)
            {
                PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
                ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();

                if (shooting != null)
                {
                    shooting.ResetFireRate();
                }

                if (movement != null && movementSpeedBuffPercentage > 0)
                {
                    movement.movementMultipler = 1;
                }
            }



            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ApplyLingerEffect(GameObject playerObject)
    {
        PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
        ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();

        yield return new WaitForSeconds(lingerDuration);

        if (shooting != null)
        {
            shooting.ResetFireRate();
        }

        if (movement != null && movementSpeedBuffPercentage > 0)
        {
            //movement.movementMultipler = 1;
        }
    }
    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (stimCoroutine != null)
        {
            StopCoroutine(stimCoroutine);
        }

        if (lingerCoroutine != null)
        {
            StopCoroutine(lingerCoroutine);
        }
    }
}
  
    
