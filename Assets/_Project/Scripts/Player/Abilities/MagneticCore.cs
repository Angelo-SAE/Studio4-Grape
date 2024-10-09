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
        B,
        BA
    }

    [Header("Ability Settings")]
    public float baseCooldown;
    public float baseDuration;
    public float baseAOERadius;
    public float finalAOERadius;
    public GameObject corePrefab;

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;
    public float aoeIncreasePercentage;
    public float aoeDecreasePercentage;
    public float damageIncreasePercentage;
    public float explosionDamage;

    [Header("Throw Settings")]
    public float throwStrength = 20f;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;

    public bool isAbilityReady = true;

    CoreSize coreSize;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isAbilityReady)
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
        if (currentUpgrade == UpgradePath.A || currentUpgrade == UpgradePath.AA)
        {
            finalAOERadius *= 1 + (aoeIncreasePercentage / 100f);
        }
        else if (currentUpgrade == UpgradePath.B || currentUpgrade == UpgradePath.BA)
        {
            finalAOERadius *= 1 - (aoeDecreasePercentage / 100f);
            core.GetComponent<MagneticCoreBehaviour>().applyVulnerable = true;
            core.GetComponent<MagneticCoreBehaviour>().damageIncreasePercentage = damageIncreasePercentage;
            core.GetComponent<MagneticCoreBehaviour>().vulnerablePersists = currentUpgrade == UpgradePath.BA;
        }

        core.GetComponent<MagneticCoreBehaviour>().Initialize(finalAOERadius, baseDuration, currentUpgrade == UpgradePath.AA, explosionDamage);

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
}
