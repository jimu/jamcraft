using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Directs owner along a NavPath
 * 
 * Usage:
 *    navigator.SetNavPath(navPath);
 *    navigator.SetNavPath(navPath, reverse:false, loop:false, bounce:false);
 *    
 * Where:
 *   navPath is a NavPath GameObject containing NavNodes
 *   reverse = false means go from end to beginning
 *   
 * When a Navigator arrives near its destination,
 * It will direct its NavMeshAgent to the next node
 * 
 * Note: If the NavPath is set to loop, Navigation will never end
 * 
 * Navigator knows about:
 *   NavPath
 *   NavMeshAgent
 */
[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    [SerializeField] NavPath navPath;
    [SerializeField] bool reverse = false;

    Vector3 currentGoalPosition;
    int currentNodeIndex;
    NavMeshAgent navMeshAgent;

    readonly static float minDistanceToGoal = 5f;


    public void SetNavPath(NavPath navPath, bool reverse = false)
    {
        this.navPath = navPath;
        this.reverse = reverse;

        enabled = true;
        navMeshAgent = GetComponent<NavMeshAgent>();

        GoToNodeIndex(reverse ? navPath.Length - 1 : 0);
    }

    private void Update()
    {
        if (enabled && DistanceToGoal() < minDistanceToGoal)
            GoToNextNode();
    }

    private float DistanceToGoal()
    {
        return Vector3.Distance(currentGoalPosition, transform.position);
    }
    /*
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("RallyPoint") && other.transform.position == currentGoalPosition)
            GoToNextNode();
    }*/

    void GoToNextNode()
    {
        int nextNodeIndex = currentNodeIndex + (reverse ? -1 : 1);
        if (navPath.IsComplete(nextNodeIndex))
            EndNavigation();
        GoToNodeIndex(nextNodeIndex);
    }

    void EndNavigation()
    {
        enabled = false;
        navMeshAgent.isStopped = true;
    }

    void GoToNodeIndex(int index)
    {
        currentNodeIndex = index;
        currentGoalPosition = navPath.GetPosition(index);

        // check if we've actually started at the first node
        if (currentGoalPosition == transform.position)
            GoToNextNode();
        else
            navMeshAgent.SetDestination(currentGoalPosition);
    }

}

