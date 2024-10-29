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

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;
    public float aoeIncreasePercentage;
    public float damageIncreasePercentage;
    public float explosionDamage;
    public float weakenPercentage;
    public float vulnerableDuration;
    public float magneticStrengthIncrease;

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

        Vector3 throwDirection = cameraTransform.forward.normalized;

        Vector3 spawnPosition = transform.position + throwDirection * 1.5f + new Vector3(0, 2, 0);
        GameObject core = Instantiate(corePrefab, spawnPosition, Quaternion.identity);

        finalAOERadius = baseAOERadius;
        float duration = baseDuration;
        bool applyExplosion = false;
        bool applyStun = false;
        bool applyWeaken = false;
        bool applyVulnerable = false;
        bool strongerMagneticEffect = false;

        switch (currentUpgrade)
        {
            case UpgradePath.A:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                break;
            case UpgradePath.AA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                applyExplosion = true;
                break;
            case UpgradePath.AAA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                applyExplosion = true;
                applyStun = true;
                break;
            case UpgradePath.AAB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                applyExplosion = true;
                explosionDamage *= 1.5f;
                break;
            case UpgradePath.AB:
                applyWeaken = true;
                weakenPercentage = 33f;
                break;
            case UpgradePath.ABA:
                applyWeaken = true;
                weakenPercentage = 50f;
                break;
            case UpgradePath.ABB:
                applyWeaken = true;
                weakenPercentage = 33f;
                duration = 15f;
                break;
            case UpgradePath.B:
                applyVulnerable = true;
                damageIncreasePercentage = 50f;
                break;
            case UpgradePath.BA:
                applyVulnerable = true;
                damageIncreasePercentage = 50f;
                vulnerableDuration = 5f;
                break;
            case UpgradePath.BAA:
                applyVulnerable = true;
                damageIncreasePercentage = 75f;
                vulnerableDuration = 5f;
                break;
            case UpgradePath.BAB:
                applyVulnerable = true;
                damageIncreasePercentage = 50f;
                vulnerableDuration = -1f;  // Permanent
                break;
            case UpgradePath.BB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                break;
            case UpgradePath.BBA:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                strongerMagneticEffect = true;
                break;
            case UpgradePath.BBB:
                finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
                strongerMagneticEffect = true;
                duration = 15f;
                break;
        }

        MagneticCoreBehaviour coreBehaviour = core.GetComponent<MagneticCoreBehaviour>();
        coreBehaviour.Initialize(finalAOERadius, duration, applyExplosion, explosionDamage);
        coreBehaviour.applyVulnerable = applyVulnerable;
        coreBehaviour.damageIncreasePercentage = damageIncreasePercentage;
        coreBehaviour.vulnerableDuration = vulnerableDuration;
        coreBehaviour.applyStun = applyStun;
        coreBehaviour.applyWeaken = applyWeaken;
        coreBehaviour.weakenPercentage = weakenPercentage;
        coreBehaviour.strongerMagneticEffect = strongerMagneticEffect;

        coreSize = core.GetComponent<CoreSize>();
        coreSize.UpdateSize(finalAOERadius);

        Rigidbody coreRigidbody = core.GetComponent<Rigidbody>();
        if (coreRigidbody != null)
        {
            Vector3 playerVelocity = playerRigidbody != null ? playerRigidbody.velocity : Vector3.zero;
            Vector3 throwVelocity = throwDirection * throwStrength + playerVelocity;
            coreRigidbody.velocity = throwVelocity;
        }
        else
        {
            Debug.LogError("Core prefab is missing a Rigidbody component.");
        }

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
    }

    public void Upgrade()
    {
        currentUpgrade = UpgradePath.A;
    }
}
