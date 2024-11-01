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

    [Header("Ability Settings")]
    public float baseCooldown = 10f;
    public GameObject stimDevicePrefab;
    public float baseAOERadius = 5f;
    private float AOERadius;
    public float buffDuration = 10f;
    public float lingerDuration = 0f;
    public AbilityUIManager abilityUI;

    [Header("Buff Settings")]
    public float fireRateBuffPercentage = 10f;
    public float damageReductionPercentage = 10f;
    public float movementSpeedBuffPercentage = 0f;
    public bool applyLinger = false;
    public float healPerKill = 0f;
    public float vulnerableChance = 0f;
    public float weakenChance = 0f;

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;

    private bool isAbilityReady = true;
    private GameObject currentStimDevice;
    private PlayerShooting playerShooting;
    private ThirdPersonMovement movement;

    

    private void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        movement = GetComponent<ThirdPersonMovement>();
        AOERadius = baseAOERadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityThree) && isAbilityReady)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    IEnumerator ActivateAbility()
    {
        isAbilityReady = false;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);

        ApplyUpgrades();

        Vector3 spawnPosition = transform.position;
        currentStimDevice = Instantiate(stimDevicePrefab, spawnPosition, Quaternion.identity);
        currentStimDevice.GetComponent<StimDevice>().Initialize(AOERadius, buffDuration, fireRateBuffPercentage, damageReductionPercentage, movementSpeedBuffPercentage, applyLinger, lingerDuration);
        StimSize stimSize = currentStimDevice.GetComponent<StimSize>();
        stimSize.UpdateSize(AOERadius);

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
        abilityUI.SetIconOnCooldown(2, isAbilityReady, baseCooldown);
    }

    private void ApplyUpgrades()
    {
        switch (currentUpgrade)
        {
            case UpgradePath.A:
                applyLinger = true;
                lingerDuration = 5f;
                break;
            case UpgradePath.AA:
                applyLinger = true;
                lingerDuration = 5f;
                AOERadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 25f;
                break;
            case UpgradePath.AAA:
                applyLinger = true;
                lingerDuration = 5f;
                AOERadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 25f;
                damageReductionPercentage = 33f;
                break;
            case UpgradePath.AAB:
                applyLinger = true;
                lingerDuration = 12.5f;
                AOERadius = baseAOERadius * 1.25f;
                movementSpeedBuffPercentage = 25f;
                break;
            case UpgradePath.AB:
                applyLinger = true;
                lingerDuration = 5f;
                healPerKill = 1f;
                break;
            case UpgradePath.ABA:
                applyLinger = true;
                lingerDuration = 5f;
                healPerKill = 1f;
                vulnerableChance = 2f;
                break;
            case UpgradePath.ABB:
                applyLinger = true;
                lingerDuration = 5f;
                healPerKill = 1f;
                weakenChance = 2f;
                break;
        }
    }
}
