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
    public float stimDuration;

    [Header("Buffs")]
    public float attackSpeedPercent;
    public float damageReductionPercent;
    public float movementSpeedPercent;

    [Header("OnHit Effects")]
    public float healChancePercent;
    public float healAmount;

    public float vulnerableChancePercent;
    public float vulnerableDuration;

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
            playerStats.StimPlayer(attackSpeedPercent / 100, damageReductionPercent / 100, movementSpeedPercent / 100, healChancePercent / 100, healAmount, vulnerableChancePercent / 100, vulnerableDuration);
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
        yield return new WaitForSeconds(stimDuration);
        if (playerInRange)
        {
            playerStats.ResetStimStatsAfterTime(lingerDuration);
        }
        Destroy(gameObject);
    }
}
