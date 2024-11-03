using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCore : MonoBehaviour
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
    public float baseCooldown;
    public float baseDuration;
    public float baseAOERadius;
    public float finalAOERadius;
    public GameObject corePrefab;
    public AbilityUIManager abilityUI;

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;
    public float aoeIncreasePercentage;
    public float damageIncreasePercentage;
    public float explosionDamage;
    public float weakenPercentage;
    public float vulnerableDuration;
    public float magneticStrengthIncrease;
    private float stunDuration;

    [Header("Throw Settings")]
    public float throwStrength = 20f;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;

    public bool isAbilityReady = true;



    CoreSize coreSize;

    void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityTwo) && isAbilityReady)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    IEnumerator ActivateAbility()
    {
        isAbilityReady = false;
        abilityUI.SetIconOnCooldown(1, isAbilityReady, baseCooldown);

        Vector3 throwDirection = cameraTransform.forward.normalized;

        Vector3 spawnPosition = transform.position + throwDirection * 1.5f + new Vector3(0, 2, 0);
        GameObject core = Instantiate(corePrefab, spawnPosition, Quaternion.identity);
        MagneticCoreBehaviour coreBehaviour = core.GetComponent<MagneticCoreBehaviour>();

        finalAOERadius = baseAOERadius;
        float duration = baseDuration;

        switch (currentUpgrade)
        {
            case UpgradePath.A:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
            break;

            case UpgradePath.AA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                coreBehaviour.willExplode = true;
                coreBehaviour.explosionDamage = explosionDamage;
            break;

            case UpgradePath.AAA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                coreBehaviour.willExplode = true;
                coreBehaviour.explosionDamage = explosionDamage;
                coreBehaviour.applyStun = true;
                coreBehaviour.stunDuration = stunDuration;
            break;

            case UpgradePath.AAB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                coreBehaviour.willExplode = true;
                coreBehaviour.explosionDamage = explosionDamage * 1.5f;
            break;

            case UpgradePath.AB:
                coreBehaviour.applyWeaken = true;
                coreBehaviour.weakenPercentage = weakenPercentage;
            break;

            case UpgradePath.ABA:
                coreBehaviour.applyWeaken = true;
                coreBehaviour.weakenPercentage = weakenPercentage;
            break;

            case UpgradePath.ABB:
                coreBehaviour.applyWeaken = true;
                coreBehaviour.weakenPercentage = weakenPercentage;
                duration = 15f; //These need to be a set variable
            break;

            case UpgradePath.B:
                coreBehaviour.applyVulnerable = true;
                coreBehaviour.damageIncreasePercentage = damageIncreasePercentage;
            break;

            case UpgradePath.BA:
                coreBehaviour.applyVulnerable = true;
                coreBehaviour.damageIncreasePercentage = damageIncreasePercentage;
                coreBehaviour.vulnerableDuration = vulnerableDuration;
            break;

            case UpgradePath.BAA:
                coreBehaviour.applyVulnerable = true;
                coreBehaviour.damageIncreasePercentage = damageIncreasePercentage;
                coreBehaviour.vulnerableDuration = vulnerableDuration;
            break;

            case UpgradePath.BAB:
                coreBehaviour.applyVulnerable = true;
                coreBehaviour.damageIncreasePercentage = damageIncreasePercentage;
                coreBehaviour.vulnerableDuration = vulnerableDuration; // Permanent
            break;

            case UpgradePath.BB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
            break;

            case UpgradePath.BBA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                coreBehaviour.strongerMagneticEffect = true;
            break;

            case UpgradePath.BBB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                coreBehaviour.strongerMagneticEffect = true;
                duration = 15f; //These need to be a set variable
            break;
        }


        coreBehaviour.aoeRadius = finalAOERadius;
        coreBehaviour.duration = duration;

        coreSize = core.GetComponent<CoreSize>();
        coreSize.UpdateSize(finalAOERadius);

        Rigidbody coreRigidbody = core.GetComponent<Rigidbody>();
        Vector3 playerVelocity = playerRigidbody != null ? playerRigidbody.velocity : Vector3.zero;
        Vector3 throwVelocity = throwDirection * throwStrength + playerVelocity;
        coreRigidbody.velocity = throwVelocity;

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
        abilityUI.SetIconOnCooldown(1, isAbilityReady, baseCooldown);
    }

    public void Upgrade()
    {
        currentUpgrade = UpgradePath.A;
    }
}
