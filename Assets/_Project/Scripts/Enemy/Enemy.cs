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
}
