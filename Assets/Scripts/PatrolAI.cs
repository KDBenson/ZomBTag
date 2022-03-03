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
    NavMeshAgent agent;             //the agent for this object in motion
    private Transform destinationTrans;     //where it's going
    int wayPointIndex;                      //for travelling in a loop
    
    public Transform[] wayPoints;           //all the places it can go

    //gets navmesh agent for this object
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //sets destination and begins navigation tick CR
    private void Start()
    {
        destinationTrans = wayPoints[0];
        StartCoroutine(CR_NavigationTick());
    }

    //every frame, get the distance between this object and the set destination,
    //if this object is close enough to be 'there', find the next place to go.
    private void Update()
    {
        Vector3 _vecToTarget = destinationTrans.position - transform.position;
        float _distToTarget = _vecToTarget.magnitude;
        if ( _distToTarget <= 1.0f )
        {
            UpdateDestination();
        }
    }

    //update where the unit will go to, loop or random type patrol
    void UpdateDestination()
    {
        //loop patrol                   ////random patrol
        IterateWayPointIndex();        ////RandomWayPointIndex();
        //set the destination to a waypoint
        destinationTrans = wayPoints[wayPointIndex].transform;
        //set the agents destination
        agent.SetDestination(destinationTrans.position);
    }

    //use this if doing 'loop' style patrol.
    //loops through waypoints in the order of the array
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

    //Every half second this executes
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
