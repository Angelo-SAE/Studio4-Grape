using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCore : MonoBehaviour
{

    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject gamePauseObject;

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
    [SerializeField] public AbilityUpgradePath.Upgrade[] upgradePaths;

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
        upgradePaths = new AbilityUpgradePath.Upgrade[2];
        isAbilityReady = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityTwo) && isAbilityReady && !gamePauseObject.value)
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

        switch (upgradePaths[0])
        {
            case AbilityUpgradePath.Upgrade.A:
                finalAOERadius += A_AOEIncrease;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AA_ExplosionDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AA_ExplosionDamage * playerStats.DamageMultiplier;
                coreBehaviour.stun = true;
                coreBehaviour.stunDuration = AAA_StunDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.bigExplode = true;
                coreBehaviour.explosionDamage = AAB_ExplosionDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = AB_ExplosionDelay;
                coreBehaviour.explosionDamage = AB_ExplosionDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = ABA_ExplosionDelay;
                coreBehaviour.explosionDamage = AB_ExplosionDamage * playerStats.DamageMultiplier;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
                finalAOERadius += A_AOEIncrease;
                coreBehaviour.miniExplosions = true;
                coreBehaviour.explosionDelay = AB_ExplosionDelay;
                coreBehaviour.explosionDamage = ABB_ExplosionDamage * playerStats.DamageMultiplier;
            break;
        }

        switch (upgradePaths[1])
        {
            case AbilityUpgradePath.Upgrade.A:
                finalDuration += A_DurationIncrease;
            break;

            case AbilityUpgradePath.Upgrade.AA:
                finalDuration += A_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = 0.1f;
            break;

            case AbilityUpgradePath.Upgrade.AAA:
                finalDuration += A_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = AAA_VulnerableDuration;
            break;

            case AbilityUpgradePath.Upgrade.AAB:
                finalDuration += AAB_DurationIncrease;
                coreBehaviour.vulnerable = true;
                coreBehaviour.vulnerableDuration = 0.1f;
            break;

            case AbilityUpgradePath.Upgrade.AB:
                finalDuration += A_DurationIncrease;
                coreBehaviour.weaken = true;
                coreBehaviour.weakenStrength = AB_WeakenStrength;
                coreBehaviour.weakenDuration = AB_WeakenDuration;
            break;

            case AbilityUpgradePath.Upgrade.ABA:
                finalDuration += ABA_DurationIncrease;
                coreBehaviour.weaken = true;
                coreBehaviour.weakenStrength = AB_WeakenStrength;
                coreBehaviour.weakenDuration = AB_WeakenDuration;
            break;

            case AbilityUpgradePath.Upgrade.ABB:
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
