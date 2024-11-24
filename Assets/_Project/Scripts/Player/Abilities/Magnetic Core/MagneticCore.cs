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
        ABB
    }

    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Ability Settings")]
    [SerializeField] private float baseCooldown;
    [SerializeField] private float baseDuration;
    [SerializeField] private float baseAOERadius;
    [SerializeField] private GameObject corePrefab;
    [SerializeField] private AbilityUIManager abilityUI;


    [Header("Throw Settings")]
    [SerializeField] private float throwStrength;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody playerRigidbody;

    private bool isAbilityReady;

    CoreSize coreSize;

    [Header("Upgrade Paths")]
    [SerializeField] private UpgradePath upgradePath1;
    [SerializeField] private UpgradePath upgradePath2;

    [Header("Upgrade Path 1")]
    [SerializeField] private float A_AOEIncrease;

    [SerializeField] private float AA_ExplosionDamage;
    [SerializeField] private float AAA_StunDuration;
    [SerializeField] private float AAB_ExplosionDamage;

    [SerializeField] private float AB_ExplosionDamage;
    [SerializeField] private float AB_ExplosionDelay;
    [SerializeField] private float ABA_ExplosionDelay;
    [SerializeField] private float ABB_ExplosionDamage;

    [Header("Upgrade Path 2")]
    [SerializeField] private float A_DurationIncrease;

    [SerializeField] private float AAA_VulnerableDuration;
    [SerializeField] private float AAB_DurationIncrease; //same as ABA increase

    [SerializeField] private float AB_WeakenStrength;
    [SerializeField] private float AB_WeakenDuration;
    [SerializeField] private float ABA_DurationIncrease;
    [SerializeField] private float ABB_WeakenStrength;

    private void Start()
    {
        isAbilityReady = true;
    }

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

        float finalAOERadius = baseAOERadius;
        float finalDuration = baseDuration;

        switch (upgradePath1)
        {
            case UpgradePath.A:
                finalAOERadius += A_AOEIncrease;
            break;

            case UpgradePath.AA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AA_ExplosionDamage;
            break;

            case UpgradePath.AAA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AA_ExplosionDamage;
                coreBehaviour.stun = true;
                coreBehaviour.stunDuration = AAA_StunDuration;
            break;

            case UpgradePath.AAB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AAB_ExplosionDamage;
            break;

            case UpgradePath.AB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = AB_ExplosionDelay;
                coreBehaviour.explosionDamage = AB_ExplosionDamage;
            break;

            case UpgradePath.ABA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = ABA_ExplosionDelay;
                coreBehaviour.explosionDamage = AB_ExplosionDamage;
            break;

            case UpgradePath.ABB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = AB_ExplosionDelay;
                coreBehaviour.explosionDamage = ABB_ExplosionDamage;
            break;
        }

        switch (upgradePath2)
        {
            case UpgradePath.A:
                finalDuration += A_DurationIncrease;
            break;

            case UpgradePath.AA:
                finalDuration += A_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = 0.1f;
            break;

            case UpgradePath.AAA:
                finalDuration += A_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = AAA_VulnerableDuration;
            break;

            case UpgradePath.AAB:
                finalDuration += AAB_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = 0.1f;
            break;

            case UpgradePath.AB:
                finalDuration += A_DurationIncrease;
                coreBehaviour.weaken = true;
                coreBehaviour.weakenStrength = AB_WeakenStrength;
                coreBehaviour.weakenDuration = AB_WeakenDuration;
            break;

            case UpgradePath.ABA:
                finalDuration += ABA_DurationIncrease;
                coreBehaviour.weaken = true;
                coreBehaviour.weakenStrength = AB_WeakenStrength;
                coreBehaviour.weakenDuration = AB_WeakenDuration;
            break;

            case UpgradePath.ABB:
                finalDuration += A_DurationIncrease;
                coreBehaviour.weaken = true;
                coreBehaviour.weakenStrength = ABB_WeakenStrength;
                coreBehaviour.weakenDuration = AB_WeakenDuration;
            break;
        }


        coreBehaviour.aoeRadius = finalAOERadius;
        coreBehaviour.duration = finalDuration;

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

}
