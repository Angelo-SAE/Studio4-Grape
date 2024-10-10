using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenadeBehaviour : MonoBehaviour
{
    
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

    public GameObject fireSpherePrefab;
    public Material fireSphereMaterial;

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
        yield return new WaitForSeconds(explosionDelay);
        
        hasExploded = false;
        Explode();
    }



    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;
        
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
                    Vector3 knockbackDirection = (enemy.transform.position - grenadeBodyTransform.position).normalized;
                    float knockbackForce = damage * knockbackForceMultiplier;
                    enemy.ApplyKnockback(knockbackDirection * knockbackForce, knockbackForce);
                    enemy.Stun(stunDuration);
                }
            }
        }

        if (spawnFire)
        {
            SpawnFireSpheres();
        }

        Destroy(gameObject);
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

    

    void SpawnFireSpheres()
    {
        
        int sphereCount = Mathf.CeilToInt(Mathf.PI * blastRadius * blastRadius * sphereDensity);

        Debug.Log("Spawning " + sphereCount + " fireballs.");

        for (int i = 0; i < sphereCount; i++)
        {
            
            Vector2 randomCircle = Random.insideUnitCircle * blastRadius;
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
