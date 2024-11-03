using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    private float fireDuration;
    private float fireDPS;
    private bool enemiesInFireSlowed;
    private float blastRadius;
    public float tickInterval = 0.2f;
    private bool enemiesStayOnFire;
    private float onFireDuration;
    public float slowStrength;

    [Header("Pulsating Settings")]
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    private Vector3 originalScale;

    private List<EnemyStats> enemiesInFire = new List<EnemyStats>();

    public void Initialize(float duration, float dps, float radius, bool slowEnemies, bool stayOnFire, float stayOnFireDuration)
    {
        fireDuration = duration;
        fireDPS = dps;
        blastRadius = radius;
        enemiesInFireSlowed = slowEnemies;
        enemiesStayOnFire = stayOnFire;
        onFireDuration = stayOnFireDuration;

        originalScale = transform.localScale;
    }

    private void Update()
    {
        UpdateEnemiesInFire();
    }


    private void UpdateEnemiesInFire()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        List<EnemyStats> currentEnemies = new List<EnemyStats>();

        foreach (Collider collider in colliders)
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                currentEnemies.Add(enemy);

                if (!enemiesInFire.Contains(enemy))
                {
                    enemiesInFire.Add(enemy);
                    //enemy.isInFire = true;
                    //enemy.isOnFire = false;
                    enemy.GetFireData(tickInterval, fireDuration, fireDPS);

                    if (enemiesInFireSlowed)
                    {
                        enemy.ApplySlow(fireDuration, slowStrength);
                    }
                }
            }
        }

        for (int i = enemiesInFire.Count - 1; i >= 0; i--)
        {
            EnemyStats enemy = enemiesInFire[i];
            if (!currentEnemies.Contains(enemy))
            {
                //enemy.isInFire = false;
                if (enemiesStayOnFire && enemy != null)
                {
                    //enemy.ApplyDamageOverTime(fireDPS, onFireDuration);
                }
                else
                {
                    //enemy.isOnFire = false;
                }

                enemiesInFire.RemoveAt(i);
            }
        }
    }

}
