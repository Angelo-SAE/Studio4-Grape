using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStim : MonoBehaviour
{

    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject gamePauseObject;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Ability Settings")]
    [SerializeField] private float baseCooldown;

    [SerializeField] private float baseAOERadius;
    [SerializeField] private float baseStimDuration;

    [SerializeField] private float baseAttackSpeedPercent;

    [Header("Extra")]
    [SerializeField] private GameObject stimDevicePrefab;
    [SerializeField] private AbilityUIManager abilityUI;

    private bool isAbilityReady;

    [Header("Upgrade Paths")]
    [SerializeField] public AbilityUpgradePath.Upgrade[] upgradePaths;

    [Header("Upgrade Path 1")]
    [SerializeField] private float A_LingerDuration;

    [SerializeField] private float AA_MovementSpeedPercent;
    [SerializeField] private float AAA_MovementSpeedPercent;
    [SerializeField] private float AAB_LingerDuration; //The same as ABA_LingerDuration

    [SerializeField] private float AB_AOEIncrease;
    [SerializeField] private float ABB_AttackSpeedIncrease;
    [SerializeField] private float ABA_LingerDuration; //The same as AAA_LingerDuration

    [Header("Upgrade Path 2")]
    [SerializeField] private float A_HealChancePercent;
    [SerializeField] private float A_HealAmount;

    [SerializeField] private float AA_DamageReductionPercent;
    [SerializeField] private float AAA_DamageReductionPercent;
    [SerializeField] private float AAB_HealAmount; //The same as ABA_HealAmount

    [SerializeField] private float AB_VulnerableChancePercent;
    [SerializeField] private float AB_VulnerableDuration;
    [SerializeField] private float ABB_VulnerableChancePercent;
    [SerializeField] private float ABA_HealAmount; //The same as AAA_HealAmount



    private void Start()
    {
        upgradePaths = new AbilityUpgradePath.Upgrade[2];
        isAbilityReady = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityThree) && isAbilityReady && !gamePauseObject.value)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        isAbilityReady = false;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);

        Vector3 spawnPosition = transform.position;
        GameObject currentStimDevice = Instantiate(stimDevicePrefab, spawnPosition, Quaternion.identity);
        StimDevice stimDevice = currentStimDevice.GetComponent<StimDevice>();

        float aoeRadius = baseAOERadius;
        float stimDuration = baseStimDuration;
        float attackSpeedPercent = baseAttackSpeedPercent;

        switch(upgradePaths[0])
        {
            case AbilityUpgradePath.Upgrade.A:
                stimDevice.lingerDuration = A_LingerDuration;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                stimDevice.lingerDuration = A_LingerDuration;
                stimDevice.movementSpeedPercent = AA_MovementSpeedPercent;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                stimDevice.lingerDuration = A_LingerDuration;
                stimDevice.movementSpeedPercent = AAA_MovementSpeedPercent;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                stimDevice.lingerDuration = AAB_LingerDuration;
                stimDevice.movementSpeedPercent = AA_MovementSpeedPercent;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                stimDevice.lingerDuration = A_LingerDuration;
                aoeRadius += AB_AOEIncrease;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                stimDevice.lingerDuration = ABA_LingerDuration;
                aoeRadius += AB_AOEIncrease;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
                stimDevice.lingerDuration = A_LingerDuration;
                aoeRadius += AB_AOEIncrease;
                attackSpeedPercent += ABB_AttackSpeedIncrease;
            break;
        }

        switch(upgradePaths[1])
        {
            case AbilityUpgradePath.Upgrade.A:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = A_HealAmount;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = A_HealAmount;
                stimDevice.damageReductionPercent = AA_DamageReductionPercent;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = A_HealAmount;
                stimDevice.damageReductionPercent = AAA_DamageReductionPercent;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = AAB_HealAmount;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = A_HealAmount;
                stimDevice.vulnerableChancePercent = AB_VulnerableChancePercent;
                stimDevice.vulnerableDuration = AB_VulnerableDuration;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = ABA_HealAmount;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
                stimDevice.healChancePercent = A_HealChancePercent;
                stimDevice.healAmount = A_HealAmount;
                stimDevice.vulnerableChancePercent = ABB_VulnerableChancePercent;
                stimDevice.vulnerableDuration = AB_VulnerableDuration;
            break;
        }

        stimDevice.aoeRadius = aoeRadius;
        stimDevice.stimDuration = stimDuration;
        stimDevice.attackSpeedPercent = attackSpeedPercent;

        StimSize stimSize = currentStimDevice.GetComponent<StimSize>();
        stimSize.UpdateSize(aoeRadius);

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);
    }
}
