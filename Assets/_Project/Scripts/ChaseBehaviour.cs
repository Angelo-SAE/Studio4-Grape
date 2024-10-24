using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : MonoBehaviour,  IBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Movement Variables")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float steeringSpeed;
    [SerializeField] private LayerMask avoidanceLayers;
    [SerializeField] private float avoidanceDetectionRange;

    private Vector3 currentVelocity;


    private Vector3 finalVector;
    private Vector3 targetVector;
    private float[] goodDirections;
    private float[] badDirections;

    private void OnDrawGizmosSelected()
    {
        if(goodDirections is not null)
        {
            for(int a = 0; a < goodDirections.Length; a++)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 1f);

                Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + (Directions.directions[a].x * (goodDirections[a])), transform.position.y, transform.position.z + (Directions.directions[a].z * (goodDirections[a]))));
            }
            Gizmos.color = Color.white;
            //Gizmos.DrawLine(transform.position, finalVector.normalized * 3);
        }
    }

    public void RunBehaviour()
    {
        GetTargetVector();
        GetGoodDirections();
        GetBadDirections();
        CalculateFinalVector();
        Vector3 tempSteeringForce = finalVector.normalized - transform.forward;
        Vector3 tempFinalVector = (transform.forward + (tempSteeringForce * (Time.deltaTime * steeringSpeed))).normalized * speed;
        //Vector3 tempFinalVector = finalVector.normalized * speed;
        rb.velocity = tempFinalVector;
        transform.rotation = Quaternion.LookRotation(tempFinalVector);
    }

    private void GetTargetVector()
    {
        Vector3 tempVector = (playerObject.value.transform.position - transform.position);
        targetVector = (new Vector3(tempVector.x, 0f, tempVector.z)).normalized;
    }

    private void GetGoodDirections()
    {
        goodDirections = new float[Directions.directions.Count];

        for(int a = 0; a < goodDirections.Length; a++)
        {
            goodDirections[a] = Vector3.Dot(Directions.directions[a], targetVector);
        }
    }

    private void GetBadDirections()
    {
        badDirections = new float[Directions.directions.Count];

        for(int a = 0; a < badDirections.Length; a++)
        {
            RaycastHit hit;
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Directions.directions[a], out hit, avoidanceDetectionRange))
            {
                if(hit.collider.tag != "Player")
                {
                    if(a == 0)
                    {
                        badDirections[badDirections.Length - 1] += 0.6f;
                        badDirections[a + 1] += 0.6f;
                    } else if(a == badDirections.Length - 1)
                    {
                        badDirections[a - 1] += 0.6f;
                        badDirections[0] += 0.6f;
                    } else {
                        badDirections[a - 1] += 0.6f;
                        badDirections[a + 1] += 0.6f;
                    }
                    badDirections[a] += 1f;
                }
            }
        }
    }

    private void CalculateFinalVector()
    {
        finalVector = Vector3.zero;
        for(int a = 0; a < goodDirections.Length; a++)
        {
            goodDirections[a] -= badDirections[a];
            if(goodDirections[a] > 0)
            {
                finalVector += (Directions.directions[a] * goodDirections[a]);
            } else if(goodDirections[a] < -0.5f)
            {
                finalVector += Directions.directions[a] * -0.5f;
            } else {
                finalVector += Directions.directions[a] * goodDirections[a];
            }
        }
    }
}
