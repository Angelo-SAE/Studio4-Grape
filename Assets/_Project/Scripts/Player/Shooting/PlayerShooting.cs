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

    public AudioSource hitMarkerSound;

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

            // Get a pooled bullet object
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Gunshot");

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

                bullet.SetActive(true);
            }

            // Play shooting sound using a pooled audio source
            GameObject audioSourceObj = ObjectPooler.SharedInstance.GetPooledObject("Shotsound");
            if (audioSourceObj != null)
            {
                AudioSource audioSource = audioSourceObj.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSourceObj.SetActive(true); // Enable the audio source object before playing
                    audioSource.transform.position = shotPosition.position;
                    audioSource.Play();
                    StartCoroutine(DisableAfterPlay(audioSourceObj, audioSource.clip.length)); // Disable the audio source after it finishes playing
                }
            }

            // Damage enemy if hit
            if (hitSomething && ((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
            {
                EnemyStats enemyStats = hit.collider.GetComponent<EnemyStats>();

                if (!enemyStats.EnemyDead)
                {
                    hitMarkerSound.Play();
                }
                if (enemyStats != null)
                {
                    if (Random.Range(0f, 1f) <= playerStats.OnHitHealChance)
                    {
                        playerStats.Heal(playerStats.OnHitHealAmount);
                    }
                    if (Random.Range(0f, 1f) <= playerStats.OnHitVulnerableChance)
                    {
                        enemyStats.ApplyVulnerable(playerStats.OnHitVulnerableDuration);
                    }
                    enemyStats.TakeDamage(primaryFireDamage * playerStats.DamageMultiplier, Color.red);
                }
            }

            pFireCurrentDelay = 0;
            pFireOffCooldown = false;
        }
    }

    private IEnumerator DisableAfterPlay(GameObject audioSourceObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSourceObj.SetActive(false);
    }
}

