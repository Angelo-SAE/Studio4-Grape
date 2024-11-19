using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject paused;
    [SerializeField] private BoolObject playerIsShooting;

    [Header("Woop")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform shotPosition;

    [Header("Primary Fire")]
    [SerializeField] private float primaryFireRange;
    [SerializeField] private float primaryFireDelay;
    [SerializeField] private float primaryFireDamage;

    private float pFireCurrentDelay;
    private bool pFireOffCooldown;

    private void Update()
    {
        if (paused.value) return;

        CoolDowns();
        GetInputs();
    }

    private void CoolDowns()
    {
        if (!pFireOffCooldown)
        {
            pFireCurrentDelay += Time.deltaTime;
            if (pFireCurrentDelay >= primaryFireDelay / playerStats.AttackSpeed)
            {
                pFireOffCooldown = true;
            }
        }
    }

    private void GetInputs()
    {
        if (Input.GetKey(keyBindings.primaryFire))
        {
            if (!playerIsShooting.value)
            {
                playerIsShooting.value = true; // Only set when changing state
            }
            PrimaryFire();
        }
        else
        {
            if (playerIsShooting.value)
            {
                playerIsShooting.value = false; // Only set when changing state
            }
        }
    }

    private void PrimaryFire()
    {
        if (pFireOffCooldown)
        {
            RaycastHit hit;
            Vector3 shotDirection = playerCamera.transform.forward;

            // Perform a raycast to determine hit point
            bool hitSomething = Physics.Raycast(playerCamera.transform.position, shotDirection, out hit, primaryFireRange);

            // Get a pooled object
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();

            if (bullet != null)
            {
                bullet.transform.position = shotPosition.position;

                // Set rotation based on whether something was hit
                if (hitSomething)
                {
                    bullet.transform.rotation = Quaternion.LookRotation(hit.point - shotPosition.position);
                }
                else
                {
                    bullet.transform.rotation = shotPosition.rotation;
                }

                bullet.SetActive(true); // Activate the bullet

                // Optionally, reset any bullet-specific properties here
            }

            // Damage enemy if hit
            if (hitSomething && ((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
            {
                EnemyStats enemyStats = hit.collider.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(primaryFireDamage);
                }
            }

            // Reset cooldown
            pFireCurrentDelay = 0;
            pFireOffCooldown = false;
        }
    }

    
}
