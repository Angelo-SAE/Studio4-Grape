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
    [SerializeField] private bool isVulnerable = false;
    [SerializeField] private float damageMultiplier = 1f;

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        UpdateHealthSlider();
        if(health <= 0)
        {
            Destroy(gameObject);
        }
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

    public void AddForce(Vector3 force)
    {
        transform.position += force * Time.deltaTime;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
