using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Enemy Stats")]
    [SerializeField] public float health;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed = 5f;
    private float originalSpeed;
    [SerializeField] private bool isVulnerable = false;
    [SerializeField] private float damageMultiplier = 1f;
    private bool isStunned = false;
    private bool isSlowed = false;
    public bool isInFire;
    public bool isOnFire;
    public bool isWeakened = false;
    private float weakenMultiplier = 1f;

    private float tickInterval, fireDuration, fireDPS;

    Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        originalSpeed = moveSpeed;
    }

    private void Update()
    {
        if (isInFire && !isOnFire)
        {
            StartCoroutine(ApplyDamageTicks(tickInterval, fireDuration, fireDPS));
        }

    }

    public void GetFireData(float ti, float fd, float dps)
    {
        tickInterval = ti;
        fireDuration = fd;
        fireDPS = dps;
    }

    public void TakeDamage(float damageTaken)
    {
        float effectiveDamage = Mathf.RoundToInt(damageTaken * (isVulnerable ? damageMultiplier : 1f) * weakenMultiplier);
        health -= effectiveDamage;
        UpdateHealthSlider();

        Debug.Log("Enemy has taken " + effectiveDamage + " damage");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyKnockback(Vector3 forceVector, float forceStrength)
    {
        rb.AddForce(forceVector, ForceMode.Impulse);
        rb.AddForce(new Vector3(0, 1.5f * forceStrength, 0), ForceMode.Impulse);
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        moveSpeed = 0f; // Stop enemy movement while stunned
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        isStunned = false;
    }

    public void ApplySlow(float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowCoroutine(duration));
        }
    }

    IEnumerator ApplyDamageTicks(float tickInterval, float fireDuration, float fireDPS)
    {
        Debug.Log("DPS Coroutine Running. Fire duration: " + fireDuration);
        isOnFire = true;
        while (fireDuration > 0 && isInFire)
        {
            yield return new WaitForSeconds(tickInterval);

            if (!isInFire) break;

            float damagePerTick = fireDPS * tickInterval;
            TakeDamage(damagePerTick);
            Debug.Log("Damage Ticking at " + damagePerTick + " dmg per tick. Each tick is " + tickInterval + " seconds.");

            fireDuration -= tickInterval;
        }
        isOnFire = false;
    }

    IEnumerator SlowCoroutine(float duration)
    {
        isSlowed = true;
        moveSpeed *= 0.5f;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        isSlowed = false;
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = health;
    }

    public void ApplyVulnerable(float increasePercentage, float duration)
    {
        isVulnerable = true;
        damageMultiplier = 1 + (increasePercentage / 100f);
        if (duration > 0)
            StartCoroutine(VulnerableDuration(duration));
    }

    IEnumerator VulnerableDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isVulnerable = false;
        damageMultiplier = 1f;
    }

    public void ApplyWeaken(float weakenPercentage)
    {
        if (!isWeakened)
        {
            isWeakened = true;
            weakenMultiplier = 1 - (weakenPercentage / 100f);
        }
    }

    public void RemoveWeaken()
    {
        isWeakened = false;
        weakenMultiplier = 1f;
    }

    public void ApplyDamageOverTime(float damagePerSecond, float duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damagePerSecond, duration));

    }

    IEnumerator DamageOverTimeCoroutine(float damagePerSecond, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(damagePerSecond * tickInterval);
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

    public void AddForce(Vector3 force)
    {
        transform.position += force * Time.deltaTime;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}

