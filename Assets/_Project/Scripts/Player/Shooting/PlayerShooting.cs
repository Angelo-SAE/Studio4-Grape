using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject paused;
    [SerializeField] private BoolObject playerIsShooting;

    [Header("Woop")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject gunShot;
    [SerializeField] private Transform shotPosition;

    [Header("Primary Fire")]
    [SerializeField] private float primaryFireRange;
    [SerializeField] private float basePrimaryFireDelay;
    [SerializeField] private float primaryFireDelay;
    [SerializeField] private float primaryFireDamage;

    private float pFireCurrentDelay;
    private bool pFireOffCooldown;

    private void Update()
    {
        if(!paused.value)
        {
            CoolDowns();
            GetInputs();
        }
    }

    private void CoolDowns()
    {
        if(!pFireOffCooldown)
        {
            pFireCurrentDelay += Time.deltaTime;
            if(pFireCurrentDelay >= primaryFireDelay)
            {
                pFireOffCooldown = true;
            }
        }
    }

    private void GetInputs()
    {
        if(Input.GetKey(keyBindings.primaryFire))
        {
            playerIsShooting.value = true;
            PrimaryFire();
        } else {
            playerIsShooting.value = false;
        }
    }

    private void PrimaryFire()
    {
        if(pFireOffCooldown)
        {
            Instantiate(gunShot, shotPosition.position, shotPosition.rotation, shotPosition);

            RaycastHit hit;
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, primaryFireRange, enemyLayer))
            {
                hit.collider.transform.GetComponent<Enemy>().TakeDamage(primaryFireDamage);
            }
            pFireCurrentDelay = 0;
            pFireOffCooldown = false;
        }
    }

    public void ModifyFireRate(float fireRateMultiplier)
    {
        
        primaryFireDelay = fireRateMultiplier * basePrimaryFireDelay;
    }

    public void ResetFireRate()
    {
        primaryFireDamage = basePrimaryFireDelay;
    }
}
