using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Collider[] collidersInRange = Physics.OverlapSphere(transform.position, aoeRadius, playerLayerMask);
            HashSet<GameObject> uniquePlayersInRange = new HashSet<GameObject>();

            foreach (Collider collider in collidersInRange)
            {
                uniquePlayersInRange.Add(collider.transform.root.gameObject);
            }
            //Debug.Log("found " +  uniquePlayersInRange.Count + " players are in the hash set");

            if (uniquePlayersInRange.Count == 1)
            {
                playerObject = uniquePlayersInRange.First(); // Assuming one player scenario

                PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
                ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();

                if (shooting != null)
                {

                    shooting.ModifyFireRate(1 - (fireRateBuffPercentage / 100f));
                    shooting.isStimmed = true;
                }
                else
                {
                    Debug.Log("shooting componenet not found");
                }

                if (movement != null && movementSpeedBuffPercentage > 0)
                {
                    movement.movementMultipler = (1 + (movementSpeedBuffPercentage / 100f));
                    movement.isStimmed = true;
                }
                else
                {
                    Debug.Log("movement componenet not found");
                }

                if (lingerCoroutine != null)
                {
                    StopCoroutine(lingerCoroutine);
                    lingerCoroutine = null;
                }
            }
            else if (uniquePlayersInRange.Count > 1)
            {
                Debug.LogError("More than one player detected!");
            }
            else if (playerObject != null && applyLinger)
            {
                //lingerCoroutine = StartCoroutine(ApplyLingerEffect(playerObject));
                PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
                ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();

                movement.StartLingerCoroutine(lingerDuration);
                shooting.StartLingerCoroutine(lingerDuration);
                
                playerObject = null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    
    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        if (playerObject != null)
        {
            PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
            ThirdPersonMovement movement = playerObject.GetComponent<ThirdPersonMovement>();
            movement.StartLingerCoroutine(lingerDuration);
            shooting.StartLingerCoroutine(lingerDuration);
        }
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
  
    
