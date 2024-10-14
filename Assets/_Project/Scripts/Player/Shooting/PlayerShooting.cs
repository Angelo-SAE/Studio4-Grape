using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject paused;

    [Header("Woop")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Camera playerCamera;

    [Header("Primary Fire")]
    [SerializeField] private float primaryFireRange;
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
            PrimaryFire();
        }
    }

    private void PrimaryFire()
    {
        if(pFireOffCooldown)
        {
            RaycastHit hit;
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, primaryFireRange, enemyLayer))
            {
                hit.collider.transform.parent.GetComponent<Enemy>().TakeDamage(primaryFireDamage);
            }
            pFireCurrentDelay = 0;
            pFireOffCooldown = false;
        }
    }
}
