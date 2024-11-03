using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StimDevice : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Player stats")]
    private PlayerStats playerStats;

    [Header("Base Stats")]
    public float aoeRadius;
    public float buffDuration;

    [Header("Buffs")]
    public float attackSpeedBuffPercentage;
    public float damageReductionBuffPercentage;
    public float movementSpeedBuffPercentage;

    [Header("Linger Efffect")]
    public float lingerDuration;

    [Header("Extra")]
    private bool resetPlayer;
    private bool playerInRange;

    private void Start()
    {
        playerStats = playerObject.value.GetComponent<PlayerStats>();
        StartCoroutine(DestroyAfterDuration());
    }

    private void Update()
    {
        CheckForPlayerInRange();
        if(playerInRange && !resetPlayer)
        {
            playerStats.StimPlayer(attackSpeedBuffPercentage, damageReductionBuffPercentage, movementSpeedBuffPercentage);
            resetPlayer = true;
        } else if(!playerInRange && resetPlayer)
        {
            playerStats.ResetStimStatsAfterTime(lingerDuration);
            resetPlayer = false;
        }
    }

    private void CheckForPlayerInRange()
    {
        if(aoeRadius >= Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(playerObject.value.transform.position.x, 0f, playerObject.value.transform.position.z)))
        {
            playerInRange = true;
        } else {
            playerInRange = false;
        }
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(buffDuration);
        if (playerInRange)
        {
            playerStats.ResetStimStatsAfterTime(lingerDuration);
        }
        Destroy(gameObject);
    }
}
