using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenade : MonoBehaviour
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
        ABB,
        B,
        BA,
        BAA,
        BAB,
        BB,
        BBA,
        BBB
    }

    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;

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
    [SerializeField] private float baseDamage = 140f;
    [SerializeField] private float minDamage = 35f;
    [SerializeField] private float blastRadius = 10f;
    [SerializeField] private float damageFalloffStart = 0f;

    [Header("Upgrade Modifiers")] //need to have all different variables for each different change
    [SerializeField] private UpgradePath currentUpgrade = UpgradePath.None;


    [Header("Base Upgrades")]
    [SerializeField] private float improvedBaseDamage = 200f;
    [SerializeField] private float improvedMinDamage = 50f;
    [SerializeField] private float increasedBlastRadiusPercentage = 15f;

    [Header("Fire Upgrades")]
    [SerializeField] private float fireDuration = 5f;
    [SerializeField] private float spawnedFireDuration;
    [SerializeField] private float fireDamagePerTick = 50f;
    [SerializeField] private float fireDensity = 1f;
    [SerializeField] private float fireSlowStrength;
    [SerializeField] private float fireExpansionPercentage = 33f;

    [Header("Stun Upgrades")]
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float knockbackForceMultiplier = 1f;



    private void Start()
    {
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
        if (Input.GetKeyDown(keyBindings.abilityOne) && isAbilityReady)
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
        float finalMinDamage = minDamage;

        ThermobaricGrenadeBehaviour grenadeBehaviour = grenade.GetComponent<ThermobaricGrenadeBehaviour>();

        switch(currentUpgrade)
        {
            case UpgradePath.A:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f); //maybe just change variable so there will be no need to divide by 100
            break;

            case UpgradePath.AA:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f);
                finalBaseDamage = improvedBaseDamage;
                finalMinDamage = improvedMinDamage;
            break;

            case UpgradePath.AAA:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f);
                finalBaseDamage = improvedBaseDamage;
                finalMinDamage = improvedMinDamage;
                grenadeBehaviour.knockbackAndStun = true;
                grenadeBehaviour.knockbackForceMultiplier = knockbackForceMultiplier;
                grenadeBehaviour.stunDuration = stunDuration;
            break;

            case UpgradePath.AAB:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f);
                finalBaseDamage = improvedBaseDamage;
                finalMinDamage = improvedMinDamage;
                grenadeBehaviour.knockbackAndStun = false;
                grenadeBehaviour.knockbackForceMultiplier = knockbackForceMultiplier;
                grenadeBehaviour.stunDuration = stunDuration;
                grenadeBehaviour.applyDamageOverTime = true;
            break;

            case UpgradePath.AB:
                finalMinDamage = finalBaseDamage * 0.5f;
            break;

            case UpgradePath.ABA:
                finalBaseDamage = improvedBaseDamage;
                finalMinDamage = finalBaseDamage * 0.5f;
            break;

            case UpgradePath.ABB:
                finalMinDamage = finalBaseDamage;
            break;

            case UpgradePath.B:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = fireDamagePerTick;
            break;

            case UpgradePath.BA:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = fireDamagePerTick;
            break;

            case UpgradePath.BAA:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = fireDamagePerTick;
                grenadeBehaviour.enemiesInFireSlowed = true;
                grenadeBehaviour.fireSlowStrength = fireSlowStrength;
            break;

            case UpgradePath.BAB:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = fireDamagePerTick;
                grenadeBehaviour.enemiesInFireSlowed = false;
                grenadeBehaviour.applyDamageOverTime = true;
            break;

            case UpgradePath.BB:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = 10f;
            break;

            case UpgradePath.BBA:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDamagePerTick = 10f;
                grenadeBehaviour.fireExpansionPercentage = fireExpansionPercentage;
            break;

            case UpgradePath.BBB:
                grenadeBehaviour.spawnFire = true;
                grenadeBehaviour.fireDuration = fireDuration;
                grenadeBehaviour.fireDamagePerTick = 10f;
            break;
        }

        grenadeBehaviour.blastRadius = finalBlastRadius;
        grenadeBehaviour.baseDamage = finalBaseDamage;
        grenadeBehaviour.minDamage = finalMinDamage;
        grenadeBehaviour.damageFalloffStart = damageFalloffStart;
        grenadeBehaviour.fireDensity = fireDensity;
        grenadeBehaviour.spawnedFireDuration = spawnedFireDuration;

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

    public void Upgrade()
    {
        currentUpgrade = UpgradePath.A;
    }
}
