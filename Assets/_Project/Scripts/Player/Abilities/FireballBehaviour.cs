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

    [Header("Pulsating Settings")]
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    private Vector3 originalScale;

    private List<Enemy> enemiesInFire = new List<Enemy>();

    public void Initialize(float duration, float dps, float radius, bool slowEnemies, bool stayOnFire, float stayOnFireDuration)
    {
        fireDuration = duration;
        fireDPS = dps;
        blastRadius = radius;
        enemiesInFireSlowed = slowEnemies;
        enemiesStayOnFire = stayOnFire;
        onFireDuration = stayOnFireDuration;

        originalScale = transform.localScale;

        StartCoroutine(HandleFireballLifetime());
    }

    void Update()
    {
        Pulsate();
        UpdateEnemiesInFire();
    }

    void Pulsate()
    {
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * scaleMultiplier;
    }

    void UpdateEnemiesInFire()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        List<Enemy> currentEnemies = new List<Enemy>();

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                currentEnemies.Add(enemy);

                if (!enemiesInFire.Contains(enemy))
                {
                    enemiesInFire.Add(enemy);
                    enemy.isInFire = true;
                    enemy.isOnFire = false;
                    enemy.GetFireData(tickInterval, fireDuration, fireDPS);

                    if (enemiesInFireSlowed)
                    {
                        enemy.ApplySlow(fireDuration);
                    }
                }
            }
        }

        for (int i = enemiesInFire.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemiesInFire[i];
            if (!currentEnemies.Contains(enemy))
            {
                enemy.isInFire = false;
                if (enemiesStayOnFire && enemy != null)
                {
                    enemy.ApplyDamageOverTime(fireDPS, onFireDuration);
                }
                else
                {
                    enemy.isOnFire = false;
                }

                enemiesInFire.RemoveAt(i);
            }
        }
    }

    IEnumerator HandleFireballLifetime()
    {
        float elapsed = 0f;
        float initialRadius = blastRadius;

        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (Enemy enemy in enemiesInFire)
        {
            enemy.isInFire = false;
            enemy.isOnFire = false;
        }

        enemiesInFire.Clear();

        Destroy(gameObject);
    }
}

