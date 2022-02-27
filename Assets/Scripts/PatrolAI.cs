using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* This is a stand alone script for a patrol navmesh agent.
 * The game object with a navmesh agent attached will travel
 * in a loop between defined waypoints or to a randomly assigned one.
 */
public class PatrolAI : MonoBehaviour
{
    private Transform destinationTrans; //where it's going
    public Transform[] wayPoints;   //places it can go
    int wayPointIndex;              //for travelling in a loop

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        destinationTrans = wayPoints[0];
        StartCoroutine(CR_NavigationTick());
    }

    private void Update()
    {
        Vector3 _vecToTarget = destinationTrans.position - transform.position;
        float _distToTarget = _vecToTarget.magnitude;
        if ( _distToTarget <= 1.0f )
        {
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        //loop patrol
        IterateWayPointIndex();
        ////random patrol
        ////RandomWayPointIndex();
        //set the destination to a waypoint
        destinationTrans = wayPoints[wayPointIndex].transform;
        agent.SetDestination(destinationTrans.position);
    }

    //use this if doing 'loop' style patrol
    void IterateWayPointIndex()
    {
        wayPointIndex++;
        if(wayPointIndex == wayPoints.Length)
        {
            wayPointIndex = 0;
        }
    }
    //use this for randomly choosing a waypoint destination
    void RandomWayPointIndex()
    {
        //pick a random waypoint to patrol to
        wayPointIndex = Random.Range(0, wayPoints.Length);
    }

    IEnumerator CR_NavigationTick()
    {
        while (true)
        {            
            if (destinationTrans != null)
                agent.SetDestination(destinationTrans.position);
            yield return new WaitForSeconds(0.5f);
        }

    }

}
