using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatObject baseHealth;
    [SerializeField] private FloatObject baseDamage;

    [Header("Enemy UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Enemy Stats")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedMultiplier;


    public float health; //will make private after changing most scripts
    private float damage;

    public float MovementSpeed => movementSpeed * movementSpeedMultiplier; //idk if i will need if will deelte if not used

    public float Damage => baseDamage.value * damageMultiplier;

    public Vector3 pullForce; //set to vector3 0 after using


    [Header("Status Effects")] //will probs remove serialization from most of the variables here
    [SerializeField] private bool isSlowed;
    private float slowDuration;
    private float currentSlowDuration;
    private float slowStrength;

    [SerializeField] private bool isStunned;
    private float stunDuration;
    private float currentStunDuration;

    [SerializeField] private bool isOnFire;
    private float fireTickSpeed;
    private float currentFireTick;
    private float fireDuration;
    private float currentFireDuration;
    private float fireDamagePerTick;

    [SerializeField] private bool isWeakened;
    [SerializeField] private float weakenMultiplier;
    private float weakenDuration;
    private float currentWeakenDuration;

    [SerializeField] private bool isVulnerable;
    [SerializeField] private float damageMultiplier;
    private float vulnerableDuration;
    private float currentVulnerableDuration;


    [Header("Enemy Variables")]
    [SerializeField] private Rigidbody rb;



    private void Start()
    {
        SetBaseHealthAndDamage();
    }

    private void SetBaseHealthAndDamage()
    {
        health = baseHealth.value;
        healthSlider.maxValue = health;
        healthSlider.value = health;

        damage = baseDamage.value;
    }

    private void Update()
    {
        if(isOnFire)
        {
            EnemyOnFire();
        }

        if(isSlowed)
        {
            RemoveSlow();
        }

        if(isStunned)
        {
            RemoveStun();
        }

        if(isVulnerable)
        {
            RemoveVunerable();
        }

        if(isWeakened)
        {
            RemoveWeaken();
        }
    }

    private void EnemyOnFire()
    {
        currentFireDuration += Time.deltaTime;
        if(currentFireDuration >= fireDuration)
        {
            isOnFire = false;
        }
        DealFireDamage();
    }

    private void DealFireDamage() //will need to change how damage is applied if vunerable takes effect on fire damage
    {
        currentFireTick += Time.deltaTime;
        if(currentFireTick >= fireTickSpeed)
        {
            currentFireTick -= fireTickSpeed;
            TakeDamage(fireDamagePerTick);
        }
    }

    public void GetFireData(float tickSpeed, float duration, float damage)
    {
        isOnFire = true;
        currentFireDuration = 0;
        fireTickSpeed = tickSpeed;
        fireDuration = duration;
        fireDamagePerTick = damage;
    }

    //public void ApplyKnockback(Vector3 forceVector, float forceStrength)
    //{
    //    rb.AddForce(forceVector, ForceMode.Impulse);
    //    rb.AddForce(new Vector3(0, 1.5f * forceStrength, 0), ForceMode.Impulse);
    //}

    public void ApplySlow(float duration, float strength)
    {
        isSlowed = true;
        currentSlowDuration = 0;
        slowDuration = duration;
        slowStrength = strength;
    }

    private void RemoveSlow()
    {
        if(movementSpeedMultiplier > slowStrength)
        {
            movementSpeedMultiplier = slowStrength;
        }
        currentSlowDuration += Time.deltaTime;
        if(currentSlowDuration >= slowDuration)
        {
            movementSpeedMultiplier = 1;
            isSlowed = false;
        }
    }

    public void ApplyStun(float duration)
    {
        isStunned = true;
        currentStunDuration = 0;
        stunDuration = duration;
        movementSpeedMultiplier = 0;
    }

    private void RemoveStun()
    {
        currentStunDuration += Time.deltaTime;
        if(currentStunDuration >= stunDuration)
        {
            movementSpeedMultiplier = 1;
            isStunned = false;
        }
    }

    public void ApplyVulnerable(float increasePercentage, float duration)
    {
        isVulnerable = true;
        currentVulnerableDuration = 0;
        vulnerableDuration = duration;
        damageMultiplier = 1 + (increasePercentage / 100f);
    }

    private void RemoveVunerable()
    {
        currentVulnerableDuration += Time.deltaTime;
        if(currentVulnerableDuration >= vulnerableDuration)
        {
            damageMultiplier = 1f;
            isVulnerable = false;
        }
    }

    public void ApplyWeaken(float weakenPercentage, float duration)
    {
        isWeakened = true;
        currentWeakenDuration = 0;
        weakenDuration = duration;
        weakenMultiplier = 1 - (weakenPercentage / 100f);
    }

    private void RemoveWeaken()
    {
        currentWeakenDuration += Time.deltaTime;
        if(currentWeakenDuration >= weakenDuration)
        {
            weakenMultiplier = 1f;
            isWeakened = false;
        }
    }

    public void AddForce(Vector3 force)
    {
        transform.position += force * Time.deltaTime;
    }

    public void TakeDamage(float damageTaken)
    {
        float effectiveDamage = damageTaken * damageMultiplier;
        health -= effectiveDamage;
        UpdateHealthSlider();

        Debug.Log("Enemy has taken " + effectiveDamage + " damage");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = health;
    }
}
