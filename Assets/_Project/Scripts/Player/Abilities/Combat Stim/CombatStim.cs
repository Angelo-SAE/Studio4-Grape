using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStim : MonoBehaviour
{
    public enum UpgradePath
    {
        None,
        A,
        AA,
        AAA,
        AAB,
        AB,
        ABA,
        ABB
    }

    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Ability Settings")]
    public float baseCooldown = 10f;
    public GameObject stimDevicePrefab;
    public float baseAOERadius = 5f;
    private float aoeRadius;
    public float buffDuration = 10f;
    public float lingerDuration = 0f;
    public AbilityUIManager abilityUI;

    [Header("Buff Settings")]
    public float attackSpeedBuffPercentage = 10f;
    public float damageReductionBuffPercentage = 10f;
    public float movementSpeedBuffPercentage = 0f;
    public float healPerKill = 0f;
    public float vulnerableChance = 0f;
    public float weakenChance = 0f;

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;

    private bool isAbilityReady = true;


    private void Start()
    {
        aoeRadius = baseAOERadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityThree) && isAbilityReady)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        isAbilityReady = false;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);

        ApplyUpgrades();

        Vector3 spawnPosition = transform.position;
        GameObject currentStimDevice = Instantiate(stimDevicePrefab, spawnPosition, Quaternion.identity);
        StimDevice stimDevice = currentStimDevice.GetComponent<StimDevice>();

        stimDevice.aoeRadius = aoeRadius;
        stimDevice.buffDuration = buffDuration;
        stimDevice.attackSpeedBuffPercentage = attackSpeedBuffPercentage;
        stimDevice.damageReductionBuffPercentage = damageReductionBuffPercentage;
        stimDevice.movementSpeedBuffPercentage = movementSpeedBuffPercentage;
        stimDevice.lingerDuration = lingerDuration;

        StimSize stimSize = currentStimDevice.GetComponent<StimSize>();
        stimSize.UpdateSize(aoeRadius);

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);
    }

    private void ApplyUpgrades()
    {
        switch (currentUpgrade)
        {
            case UpgradePath.A:
                lingerDuration = 5f;
                break;
            case UpgradePath.AA:
                lingerDuration = 5f;
                aoeRadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 0.25f;
                break;
            case UpgradePath.AAA:
                lingerDuration = 5f;
                aoeRadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 0.25f;
                damageReductionBuffPercentage = 0.33f;
                break;
            case UpgradePath.AAB:
                lingerDuration = 12.5f;
                aoeRadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 0.25f;
                break;
            case UpgradePath.AB:
                lingerDuration = 5f;
                healPerKill = 1f;
                break;
            case UpgradePath.ABA:
                lingerDuration = 5f;
                healPerKill = 1f;
                vulnerableChance = 2f;
                break;
            case UpgradePath.ABB:
                lingerDuration = 5f;
                healPerKill = 1f;
                weakenChance = 2f;
                break;
        }
    }
}
