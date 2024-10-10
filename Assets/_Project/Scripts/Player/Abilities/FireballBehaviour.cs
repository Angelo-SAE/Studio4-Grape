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
    

    [Header("Pulsating Settings")]
    public float pulseSpeed = 2f;  // Speed of the pulsating effect
    public float minScale = 0.8f;  // Minimum size of the fireball
    public float maxScale = 1.2f;  // Maximum size of the fireball

    private Vector3 originalScale;  // Store the original scale of the fireball

    private List<Enemy> enemiesInFire = new List<Enemy>();  // List to keep track of enemies inside the fire area


    public void Initialize(float duration, float dps, float radius, bool slowEnemies)
    {
        fireDuration = duration;
        fireDPS = dps;
        blastRadius = radius;
        enemiesInFireSlowed = slowEnemies;
        


        // Store the original scale of the fireball
        originalScale = transform.localScale;

        // Start the coroutine to handle the fireball's behavior
        StartCoroutine(HandleFireballLifetime());
        
        
    }

    void Update()
    {
        // Apply pulsating effect
        Pulsate();
        UpdateEnemiesInFire();
    }

    void Pulsate()
    {
        // Use a sine wave to smoothly transition between minScale and maxScale
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        // Apply the scale change to the fireball
        transform.localScale = originalScale * scaleMultiplier;
    }

    void UpdateEnemiesInFire()
    {
        // Update the list of enemies currently within the fire area
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        List<Enemy> currentEnemies = new List<Enemy>();

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                currentEnemies.Add(enemy);

                // Add new enemies to the list if not already in the fire
                if (!enemiesInFire.Contains(enemy))
                {
                    enemiesInFire.Add(enemy);
                    enemy.isInFire = true;  // Mark as in fire
                    enemy.isOnFire = false;  // Reset this, it'll be set to true when damage is applied
                    enemy.GetFireData(tickInterval, fireDuration, fireDPS);

                    if (enemiesInFireSlowed)
                    {
                        enemy.ApplySlow(fireDuration);
                    }
                }
            }
        }

        // Remove enemies that have left the fire area and reset their fire flags
        for (int i = enemiesInFire.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemiesInFire[i];
            if (!currentEnemies.Contains(enemy))
            {
                // Reset the fire status for enemies leaving the fire
                enemy.isInFire = false;
                enemy.isOnFire = false;

                // Remove the enemy from the enemiesInFire list
                enemiesInFire.RemoveAt(i);
            }
        }
    }



    IEnumerator HandleFireballLifetime()
    {
        float elapsed = 0f;

        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // When fireball lifetime ends, stop all enemies from taking damage
        foreach (Enemy enemy in enemiesInFire)
        {
            enemy.isInFire = false;
            enemy.isOnFire = false;
        }

        enemiesInFire.Clear();  // Clear the list to ensure no further tracking

        // Destroy the fireball after its lifetime
        Destroy(gameObject);
    }
}

