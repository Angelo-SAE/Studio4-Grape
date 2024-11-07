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

    [SerializeField]private float attackSpeed;
    [SerializeField]private float damageReduction; //temp serialization for testing
    [SerializeField]private float movementSpeed;

    //Idk if i wil use later so i will leave these here for now
    private float BaseAttackSpeed => baseAttackSpeed;
    private float BaseDamageReduction => baseDamageReduction;
    private float BaseMovementSpeed => baseMovementSpeed;

    public float AttackSpeed {get => attackSpeed;}
    public float DamageReduction {get => damageReduction;}
    public float MovementSpeed {get => movementSpeed;}

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
        healthText.text = health.ToString();
    }

    public void StimPlayer(float attackSpeedBuff, float damageReductionBuff, float movementSpeedBuff)
    {
        resetStim = false;
        attackSpeed = baseAttackSpeed + (baseAttackSpeed * attackSpeedBuff);
        damageReduction = baseDamageReduction + (baseDamageReduction * damageReductionBuff);
        movementSpeed = baseMovementSpeed + (baseMovementSpeed * movementSpeedBuff);
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
    }
}
