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
    public float baseCooldown = 5f;  // Fast cooldown
    public GameObject grenadePrefab;

    [Header("Throw Settings")]
    public float throwStrength = 20f;  // Adjust to control throw distance
    public float upwardThrowAngle = 15f;
    public Transform cameraTransform;  // Reference to player's camera
    public Rigidbody playerRigidbody;  // Reference to player's Rigidbody

    [Header("Damage Settings")]
    public float baseDamage = 140f;
    public float minDamage = 35f;
    public float blastRadius = 10f;
    public float damageFalloffStart = 0f;

    [Header("Upgrade Modifiers")]
    public UpgradePath currentUpgrade = UpgradePath.None;
    public float increasedBlastRadiusPercentage = 15f;
    public float improvedBaseDamage = 200f;
    public float improvedMinDamage = 50f;
    public float fireDuration = 5f;
    public float fireDPS = 50f;
    public float stunDuration = 2f;
    public float knockbackForceMultiplier = 1f;
    public bool enemiesInFireSlowed = false;
    public float sphereDensity = 1f;
    public bool damageNoFalloff = false;
    public bool enemiesStayOnFire = false;
    public float onFireDuration = 1.5f;
    public float fireExpansionPercentage = 33f;

    private bool isAbilityReady = true;

    void Start()
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

    void Update()
    {
        if (Input.GetKeyDown(keyBindings.abilityOne) && isAbilityReady)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    IEnumerator ActivateAbility()
    {
        isAbilityReady = false;

        Vector3 throwDirection = CalculateThrowDirection(cameraTransform.forward);
        Vector3 spawnPosition = transform.position + throwDirection * 1.5f + new Vector3(0, 2, 0);
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, Quaternion.identity);

        float finalBlastRadius = blastRadius;
        float finalBaseDamage = baseDamage;
        float finalMinDamage = minDamage;
        bool spawnFire = false;
        bool knockbackAndStun = false;
        bool applyDamageOverTime = false;

        ThermobaricGrenadeBehaviour grenadeBehaviour = grenade.GetComponent<ThermobaricGrenadeBehaviour>();

        switch (currentUpgrade)
        {
            case UpgradePath.A:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f);
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
                knockbackAndStun = true;
                break;
            case UpgradePath.AAB:
                finalBlastRadius *= 1 + (increasedBlastRadiusPercentage / 100f);
                finalBaseDamage = improvedBaseDamage;
                finalMinDamage = improvedMinDamage;
                knockbackAndStun = false;
                applyDamageOverTime = true;
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
                spawnFire = true;
                break;
            case UpgradePath.BA:
                spawnFire = true;
                fireDuration = 7.5f;
                break;
            case UpgradePath.BAA:
                spawnFire = true;
                fireDuration = 7.5f;
                enemiesInFireSlowed = true;
                break;
            case UpgradePath.BAB:
                spawnFire = true;
                fireDuration = 7.5f;
                enemiesInFireSlowed = false;
                applyDamageOverTime = true;
                break;
            case UpgradePath.BB:
                spawnFire = true;
                fireDPS = 75f;
                break;
            case UpgradePath.BBA:
                spawnFire = true;
                fireDPS = 75f;
                fireExpansionPercentage = 33f;
                break;
            case UpgradePath.BBB:
                spawnFire = true;
                fireDPS = 75f;
                enemiesStayOnFire = true;
                break;
        }

        grenadeBehaviour.Initialize(finalBlastRadius, finalBaseDamage, finalMinDamage, damageFalloffStart, spawnFire, fireDuration, fireDPS, sphereDensity, knockbackAndStun, stunDuration, enemiesInFireSlowed, knockbackForceMultiplier, applyDamageOverTime, damageNoFalloff, fireExpansionPercentage, enemiesStayOnFire, onFireDuration);

        Rigidbody grenadeRigidbody = grenade.GetComponentInChildren<Rigidbody>();
        if (grenadeRigidbody != null)
        {
            Vector3 playerVelocity = playerRigidbody != null ? playerRigidbody.velocity : Vector3.zero;
            Vector3 throwVelocity = throwDirection * throwStrength + playerVelocity;
            grenadeRigidbody.velocity = throwVelocity;
        }
        else
        {
            Debug.LogError("Grenade prefab is missing a Rigidbody component.");
        }

        yield return new WaitForSeconds(baseCooldown);
        isAbilityReady = true;
    }

    public void Upgrade()
    {
        currentUpgrade = UpgradePath.A;
    }
}
