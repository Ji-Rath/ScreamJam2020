using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{
    public GameObject playerRef;

    private NavMeshAgent navAgent;

    [Header("Sight"), Space]

    [SerializeField, Tooltip("The range that the enemy can detect the player")]
    private float sightRadius = 20f;

    [SerializeField, Range(0f,180f), Tooltip("Monster FOV, angle starts at their front side.")]
    private float halfViewAngle = 60f;

    void Awake()
    {
        //Get NavMeshAgent component for navigation
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //Start the visibility check coroutine
        StartCoroutine(CheckPlayerVisibility());
    }

    //Check whether player has been seen
    IEnumerator CheckPlayerVisibility()
    {
        //We want the coroutine to repeat continuously
        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            Vector3 Direction = (playerRef.transform.position - transform.position).normalized;
            float PlayerDistance = Vector3.Distance(transform.position, playerRef.transform.position);
            float PlayerAngle = Vector3.Angle(Direction, transform.forward);

            //Check if player is within viewing distance and within angle of view
            if (PlayerDistance < sightRadius && PlayerAngle < halfViewAngle)
            {

                //Do a line trace to see if there is an object between the enemy and the player
                RaycastHit RayHit;
                if (Physics.Raycast(transform.position, Direction, out RayHit, sightRadius))
                {
                    if (RayHit.collider.tag == "Player")
                    {
                        navAgent.SetDestination(RayHit.collider.transform.position);
                    }

                    Debug.DrawLine(transform.position, RayHit.point, Color.red, 0.5f);
                }
                else
                {
                    Debug.DrawLine(transform.position, RayHit.point, Color.green, 0.5f);
                }
            }
        }
        
    }

    
}
