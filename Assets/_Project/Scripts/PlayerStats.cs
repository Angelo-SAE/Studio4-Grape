using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Player Stats")]
    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
