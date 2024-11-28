using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Player UI")]
    [SerializeField] private TMP_Text healthText;

    [Header("Player Stats")]
    [SerializeField] private float health;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float baseDamageReduction;
    [SerializeField] private float baseMovementSpeed;

    //temp serialization for testing VVVVVVVVVVV
    [SerializeField]private float attackSpeed;
    [SerializeField]private float damageReduction;
    [SerializeField]private float movementSpeed;
    [SerializeField]private float damageMultiplier;

    [SerializeField]private float onHitHealChance;
    [SerializeField]private float onHitHealAmount;
    [SerializeField]private float onHitVulnerableChance;
    [SerializeField]private float onHitVulnerableDuration;

    public float OnHitHealChance => onHitHealChance;
    public float OnHitHealAmount => onHitHealAmount;
    public float OnHitVulnerableChance => onHitVulnerableChance;
    public float OnHitVulnerableDuration => onHitVulnerableDuration;

    //Idk if i wil use later so i will leave these here for now
    private float BaseAttackSpeed => baseAttackSpeed;
    private float BaseDamageReduction => baseDamageReduction;
    private float BaseMovementSpeed => baseMovementSpeed;

    public float DamageMultiplier => damageMultiplier;
    public float AttackSpeed => attackSpeed;
    public float DamageReduction => damageReduction;
    public float MovementSpeed => movementSpeed;

    private float stimDelay;
    private float currentStimDelay;
    private bool resetStim;

    private void Start()
    {
        healthText.text = health.ToString();
        attackSpeed = baseAttackSpeed;
    }

    private void Update()
    {
        if(resetStim)
        {
            currentStimDelay += Time.deltaTime;
            if(currentStimDelay >= stimDelay)
            {
                ResetStimStats();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage - (damage * damageReduction);
        healthText.text = Mathf.Round(health).ToString();
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        healthText.text = Mathf.Round(health).ToString();
    }

    public void StimPlayer(float attackSpeedBuff, float damageReductionBuff, float movementSpeedBuff, float healChance, float healAmount, float vulnerableChance, float vulnerableDuration)
    {
        resetStim = false;
        attackSpeed = baseAttackSpeed + attackSpeedBuff;
        damageReduction = baseDamageReduction + damageReductionBuff;
        movementSpeed = baseMovementSpeed + movementSpeedBuff;

        onHitHealChance = healChance;
        onHitHealAmount = healAmount;
        onHitVulnerableChance = vulnerableChance;
        onHitVulnerableDuration = vulnerableDuration;
    }

    public void ResetStimStatsAfterTime(float delay)
    {
        stimDelay = delay;
        currentStimDelay = 0;
        resetStim = true;
    }

    private void ResetStimStats()
    {
        resetStim = false;
        attackSpeed = baseAttackSpeed;
        damageReduction = baseDamageReduction;
        movementSpeed = baseMovementSpeed;
        onHitHealChance = 0;
        onHitHealAmount = 0;
        onHitVulnerableChance = 0;
        onHitVulnerableDuration = 0;
    }
}
