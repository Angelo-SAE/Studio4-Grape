using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenade : MonoBehaviour
{


    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject gamePauseObject;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Ability Settings")]
    [SerializeField] private float baseCooldown;  // Fast cooldown
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private AbilityUIManager abilityUI;

    private bool isAbilityReady = true;

    [Header("Throw Settings")]
    [SerializeField] private float throwStrength = 20f;  // Adjust to control throw distance
    [SerializeField] private float upwardThrowAngle = 15f;
    [SerializeField] private Transform cameraTransform;  // Reference to player's camera
    [SerializeField] private Rigidbody playerRigidbody;  // Reference to player's Rigidbody

    [Header("Damage Settings")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float blastRadius;

    [Header("Upgrade Modifiers")]
    [SerializeField] public AbilityUpgradePath.Upgrade[] upgradePaths;

    [Header("Path 1 Upgrades")]
    [SerializeField] private int A_BlastRadiusIncrease;

    [SerializeField] private float AA_StunDuration;
    [SerializeField] private float AAA_StunDuration;
    [SerializeField] private int AAB_BlastRadiusIncrease;

    [SerializeField] private float AB_SlowDuration;
    [SerializeField] private float ABA_SlowDuration;
    //[SerializeField] private float ABB_VulnerableDuration; the vulnerable duration is equal to the slow duration


    [Header("Path 2 Upgrades")]
    [SerializeField] private int A_BaseDamageIncrease;

    [SerializeField] private float AA_FireDuration;
    [SerializeField] private float AA_FireDamagePerTick;
    [SerializeField] private float AAA_FireDuration;
    [SerializeField] private float AAB_FireDamagePerTick;

    [SerializeField] private int AB_BleedTickSpeed;
    [SerializeField] private float AB_BleedDamage;
    [SerializeField] private float ABA_BleedTickSpeed;
    [SerializeField] private float ABB_BleedDamage;



    private void Start()
    {
        upgradePaths = new AbilityUpgradePath.Upgrade[2];
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }
    }

    private Vector3 CalculateThrowDirection(Vector3 cameraForward)
    {
        float angleInRadians = upwardThrowAngle * Mathf.Deg2Rad;
        Vector3 adjustedDirection = Quaternion.AngleAxis(upwardThrowAngle * -1, cameraTransform.right) * cameraForward;
        return adjustedDirection.normalized;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityOne) && isAbilityReady && !gamePauseObject.value)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        isAbilityReady = false;
        abilityUI.SetIconOnCooldown(3, isAbilityReady, baseCooldown);

        Vector3 throwDirection = CalculateThrowDirection(cameraTransform.forward);
        Vector3 spawnPosition = transform.position + throwDirection * 1.5f + new Vector3(0, 2, 0);
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, Quaternion.identity);

        float finalBlastRadius = blastRadius;
        float finalBaseDamage = baseDamage;

        ThermobaricGrenadeBehaviour grenadeBehaviour = grenade.GetComponent<ThermobaricGrenadeBehaviour>();

        switch(upgradePaths[0])
        {
            case AbilityUpgradePath.Upgrade.A:
                finalBlastRadius += A_BlastRadiusIncrease;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                finalBlastRadius += A_BlastRadiusIncrease;
                grenadeBehaviour.stun = true;
                grenadeBehaviour.stunDuration = AA_StunDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                finalBlastRadius += A_BlastRadiusIncrease;
                grenadeBehaviour.stun = true;
                grenadeBehaviour.stunDuration = AAA_StunDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                finalBlastRadius += AAB_BlastRadiusIncrease;
                grenadeBehaviour.stun = true;
                grenadeBehaviour.stunDuration = AA_StunDuration;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                finalBlastRadius += A_BlastRadiusIncrease;
                grenadeBehaviour.slow = true;
                grenadeBehaviour.slowDuration = AB_SlowDuration;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                finalBlastRadius += A_BlastRadiusIncrease;
                grenadeBehaviour.slow = true;
                grenadeBehaviour.slowDuration = ABA_SlowDuration;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
                finalBlastRadius += A_BlastRadiusIncrease;
                grenadeBehaviour.slow = true;
                grenadeBehaviour.slowDuration = AB_SlowDuration;
                grenadeBehaviour.vulnerable = true;
                grenadeBehaviour.vulnerableDuration = AB_SlowDuration;
            break;
        }

        switch(upgradePaths[1])
        {
            case AbilityUpgradePath.Upgrade.A:
                finalBaseDamage += A_BaseDamageIncrease;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = AA_FireDamagePerTick * playerStats.DamageMultiplier;
                grenadeBehaviour.fireDuration = AA_FireDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = AA_FireDamagePerTick * playerStats.DamageMultiplier;
                grenadeBehaviour.fireDuration = AAA_FireDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = AAB_FireDamagePerTick * playerStats.DamageMultiplier;
                grenadeBehaviour.fireDuration = AA_FireDuration;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.applyBleed = true;
                grenadeBehaviour.bleedTickSpeed = AB_BleedTickSpeed;
                grenadeBehaviour.bleedDamage = AB_BleedDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.applyBleed = true;
                grenadeBehaviour.bleedTickSpeed = ABA_BleedTickSpeed;
                grenadeBehaviour.bleedDamage = AB_BleedDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
                finalBaseDamage += A_BaseDamageIncrease;
                grenadeBehaviour.applyBleed = true;
                grenadeBehaviour.bleedTickSpeed = AB_BleedTickSpeed;
                grenadeBehaviour.bleedDamage = ABB_BleedDamage * playerStats.DamageMultiplier;
            break;
        }

        grenadeBehaviour.blastRadius = finalBlastRadius;
        grenadeBehaviour.explosionDamage = finalBaseDamage * playerStats.DamageMultiplier;

        Rigidbody grenadeRigidbody = grenade.GetComponentInChildren<Rigidbody>();

        if (grenadeRigidbody != null)
        {
            Vector3 playerVelocity = playerRigidbody != null ? playerRigidbody.velocity : Vector3.zero;
            Vector3 throwVelocity = throwDirection * throwStrength + playerVelocity;
            grenadeRigidbody.velocity = throwVelocity;
        }

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
        abilityUI.SetIconOnCooldown(3, isAbilityReady, baseCooldown);
    }

}
