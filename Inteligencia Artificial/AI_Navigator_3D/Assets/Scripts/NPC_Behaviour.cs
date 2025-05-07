<<<<<<< HEAD
using System.Collections;
using Unity.VisualScripting;
=======
>>>>>>> 665d1d49142c0b675bea71f6d5e3b57fd729884c
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behaviour : MonoBehaviour
{

    [SerializeField] private Vector3 destination;
<<<<<<< HEAD
    [SerializeField] private Transform path;
    [SerializeField] private int childrenIndex = 0;
    [SerializeField] private Vector3 min, max;

    [Header("PlayerTracking")]
    [SerializeField] private GameObject player;
    [SerializeField] private float playerDetectionRange = 5f;
    [SerializeField] private float angleVision = 45f;
    [SerializeField] private bool isPlayerTracking = false;

    private bool isWaiting = false;

    void Start()
    {
        destination = path.GetChild(childrenIndex).position;
        // destination = RandomDestination();
        GetComponent<NavMeshAgent>().SetDestination(destination);
        // StartCoroutine(FollowPlayer());
        StartCoroutine(RangeDetection());
        StartCoroutine(AngleVisionDetection());
=======

    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(destination);
>>>>>>> 665d1d49142c0b675bea71f6d5e3b57fd729884c
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                destination = hit.point;
            }
            GetComponent<NavMeshAgent>().SetDestination(destination);
        }
<<<<<<< HEAD

        // if (!isPlayerTracking && !isWaiting && Vector3.Distance(transform.position, destination) < 1f)
        // {
        //     StartCoroutine(WaitAndMoveToNextPoint());
        // }

        // if (Vector3.Distance(transform.position, destination) < 1f)
        // {
        //     destination = RandomDestination();
        //     GetComponent<NavMeshAgent>().SetDestination(destination);
        // }
    }

    private IEnumerator WaitAndMoveToNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        childrenIndex++;
        childrenIndex = childrenIndex % path.childCount;

        destination = path.GetChild(childrenIndex).position;
        GetComponent<NavMeshAgent>().SetDestination(destination);
        isWaiting = false;
    }

    private Vector3 RandomDestination() // Esto solo funciona para planos rectangulares
    {
        return new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
    }

    IEnumerator FollowPlayer()
    {

        while (true)
        {
            destination = player.transform.position;
            GetComponent<NavMeshAgent>().SetDestination(destination);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator RangeDetection()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < playerDetectionRange)
            {
                isPlayerTracking = true;
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                isPlayerTracking = false;
                GetComponent<NavMeshAgent>().SetDestination(destination);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator AngleVisionDetection()
    {
        while (true)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            if (Vector3.Angle(directionToPlayer, transform.forward) < angleVision)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, playerDetectionRange))
                {
                    if (hit.collider.gameObject == player)
                    {
                        isPlayerTracking = true;
                        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                    }
                    else
                    {
                        isPlayerTracking = false;
                        GetComponent<NavMeshAgent>().SetDestination(destination);
                    }
                }
            }
            else
            {
                isPlayerTracking = false;
                GetComponent<NavMeshAgent>().SetDestination(destination);
            }
            yield return new WaitForSeconds(1f);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, player.transform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Vector3 forward = transform.forward * playerDetectionRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -angleVision / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angleVision / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

=======
    }
>>>>>>> 665d1d49142c0b675bea71f6d5e3b57fd729884c
}
