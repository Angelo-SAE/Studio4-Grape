using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenadeBehaviour : MonoBehaviour
{
    // Ability parameters
    private float blastRadius;
    private float baseDamage;
    private float minDamage;
    private float damageFalloffStart;
    public float explosionDelay;
    private bool spawnFire;
    private float fireDuration;
    private float fireDPS;
    private float sphereDensity;
    private bool knockbackAndStun;
    private float stunDuration;
    private bool enemiesInFireSlowed;
    private float knockbackForceMultiplier;

    public GameObject fireSpherePrefab;  // Prefab for the small orange spheres
    public Material fireSphereMaterial;  // Material for the spheres, set to orange color

    public Transform grenadeBodyTransform;


    private bool hasExploded = false;
    private List<GameObject> spawnedSpheres = new List<GameObject>();

    public void Initialize(float blastRadius, float baseDamage, float minDamage, float damageFalloffStart, bool spawnFire, float fireDuration, float fireDPS, float sphereDensity, bool knockbackAndStun, float stunDuration, bool enemiesInFireSlowed, float knockbackForceMultiplier)
    {
        this.blastRadius = blastRadius;
        this.baseDamage = baseDamage;
        this.minDamage = minDamage;
        this.damageFalloffStart = damageFalloffStart;
        this.spawnFire = spawnFire;
        this.fireDuration = fireDuration;
        this.fireDPS = fireDPS;
        this.sphereDensity = sphereDensity;
        this.knockbackAndStun = knockbackAndStun;
        this.stunDuration = stunDuration;
        this.enemiesInFireSlowed = enemiesInFireSlowed;
        this.knockbackForceMultiplier = knockbackForceMultiplier;

        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        // Wait for the delay before exploding
        yield return new WaitForSeconds(explosionDelay);

        // Trigger the explosion
        hasExploded = false;
        Explode();
    }



    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        // Explosion effects (particles, sound, etc.) can be added here

        // Damage enemies within the blast radius using the Rigidbody's position
        Collider[] colliders = Physics.OverlapSphere(grenadeBodyTransform.position, blastRadius);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(grenadeBodyTransform.position, enemy.transform.position);
                float damage = CalculateDamage(distance);

                enemy.TakeDamage(damage);

                if (knockbackAndStun)
                {
                    // Knockback force scales with damage dealt
                    Vector3 knockbackDirection = (enemy.transform.position - grenadeBodyTransform.position).normalized;
                    float knockbackForce = damage * knockbackForceMultiplier;  // Adjust multiplier as needed
                    enemy.ApplyKnockback(knockbackDirection * knockbackForce, knockbackForce);
                    enemy.Stun(stunDuration);
                }
            }
        }

        if (spawnFire)
        {
            SpawnFireSpheres();  // Spawn fireballs without worrying about their behavior
        }

        Destroy(gameObject);  // Grenade destroys itself after explosion
    }

    float CalculateDamage(float distance)
    {
        // Calculate damage based on distance from explosion center
        if (distance <= damageFalloffStart)
        {
            return baseDamage;
        }
        else if (distance >= blastRadius)
        {
            return minDamage;
        }
        else
        {
            float t = (distance - damageFalloffStart) / (blastRadius - damageFalloffStart);
            return Mathf.Lerp(baseDamage, minDamage, t);
        }
    }

    /*IEnumerator SpawnFireArea()
    {
        
        SpawnFireSpheres();

        float elapsed = 0f;

        
        while (elapsed < fireDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider collider in colliders)
            {
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    
                    enemy.TakeDamage(fireDPS * Time.deltaTime);

                    // Slow enemies if Upgrade BA is active
                    if (enemiesInFireSlowed)
                    {
                        enemy.ApplySlow(fireDuration - elapsed);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Fire expired. Destroying fire spheres.");
        
        foreach (GameObject sphere in spawnedSpheres)
        {
            if (sphere != null)
            {
                Destroy(sphere);
            }
        }

        
        spawnedSpheres.Clear();
    }*/

    void SpawnFireSpheres()
    {
        
        int sphereCount = Mathf.CeilToInt(Mathf.PI * blastRadius * blastRadius * sphereDensity);  // Use appropriate density

        Debug.Log("Spawning " + sphereCount + " fireballs.");  // Debug to check how many spheres are being created

        for (int i = 0; i < sphereCount; i++)
        {
            // Generate a random position within the blast radius
            Vector2 randomCircle = Random.insideUnitCircle * blastRadius;  // Inside a circle for random position
            Vector3 randomPosition = new Vector3(grenadeBodyTransform.position.x + randomCircle.x, grenadeBodyTransform.position.y, grenadeBodyTransform.position.z + randomCircle.y);

            

            
            GameObject fireball = Instantiate(fireSpherePrefab, randomPosition, Quaternion.identity);

            
            FireballBehaviour fireballBehaviour = fireball.GetComponent<FireballBehaviour>();
            if (fireballBehaviour != null)
            {
                fireballBehaviour.Initialize(fireDuration, fireDPS, blastRadius, enemiesInFireSlowed);
            }
            else
            {
                Debug.LogError("Fireball prefab is missing!");
            }
        }
    }
}
