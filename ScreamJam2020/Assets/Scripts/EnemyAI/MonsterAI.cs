using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{
    public delegate void OnMonsterAction();
    public static OnMonsterAction OnMonsterKillPlayer;


    private GameObject playerRef;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Player player;

    [Header("Enemy Variables"), Space]
    public float attackDistance;
    public float minimumLookAroundDistance;
    public float lookAroundTime;
    public Vector3 lastPosition;
    public float spawnRadiusMin = 3f;
    public float spawnRadiusMax = 6f;

    [Header("Sight"), Space]

    [SerializeField, Tooltip("The range that the enemy can detect the player")]
    private float sightRadius = 20f;

    [SerializeField, Range(0f,180f), Tooltip("Monster FOV, angle starts at their front side.")]
    private float halfViewAngle = 60f;

    void Awake()
    {
        //Get NavMeshAgent component for navigation
        playerRef = GameManager.Get().playerRef;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = playerRef.GetComponent<Player>();
    }

    void Start()
    {
        //Start the visibility check coroutine
        StartCoroutine(CheckPlayerVisibility());
    }

    IEnumerator LookForPlayer()
    {
        yield return new WaitForSeconds(lookAroundTime);

        animator.SetTrigger("Disappear");

        //Test, maybe execute some kind of vanish animation before disabling the gameobject
        gameObject.SetActive(false);
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
            if (PlayerDistance < sightRadius && PlayerAngle < halfViewAngle && !player.isHiding)
            {
                animator.SetBool("LostSight", false);

                //Do a line trace to see if there is an object between the enemy and the player
                RaycastHit RayHit;
                if (Physics.Raycast(transform.position, Direction, out RayHit, sightRadius))
                {
                    if (RayHit.collider.tag == "Player")
                    {
                        navAgent.SetDestination(RayHit.collider.transform.position);
                        lastPosition = RayHit.collider.transform.position;
                    }

                    Debug.DrawLine(transform.position, RayHit.point, Color.red, 0.5f);
                }
                else
                {
                    Debug.DrawLine(transform.position, RayHit.point, Color.green, 0.5f);
                }

                //If its in attack distance, kill the player
                if(PlayerDistance <= attackDistance)
                {
                    animator.SetTrigger("Attack");
                    if(OnMonsterKillPlayer != null)
                    {
                        OnMonsterKillPlayer();
                    }
                    navAgent.isStopped = true;
                    Debug.Log("Player Died");
                }
            }
            else
            {
                //Go to last seen position
                animator.SetBool("LostSight", true);
                navAgent.SetDestination(lastPosition);
                if(Vector3.Distance(transform.position, lastPosition) <= minimumLookAroundDistance)
                {
                    StartCoroutine(LookForPlayer());
                }
            }
        }
        
    }

    private void OnEnable()
    {
        StartCoroutine(CheckPlayerVisibility());
        if (player.isHiding)
        {
            lastPosition = player.currentHidingPlace.transform.GetComponentInParent<Door>().enemyStandPoint.transform.position;
        }
        else
        {
            lastPosition = player.transform.position;
        }
        
        Debug.Log("Appeared Again");
    }

    
    
}
