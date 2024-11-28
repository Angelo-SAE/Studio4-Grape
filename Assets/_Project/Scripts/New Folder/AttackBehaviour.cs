using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour, IBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("Attack Variables")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float attackRange;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    public void RunBehaviour()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition.position, attackRange, targetLayers);
        for(int a = 0; a < hitColliders.Length; a++)
        {
            hitColliders[a].GetComponent<IDamageable>().TakeDamage(enemyStats.Damage);
        }
    }
}
