using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]


public class Unit : MonoBehaviour
{    
    [SerializeField] Transform destinationTrans; //where it go
    NavMeshAgent agent;                         //what is move

    [SerializeField] GameObject selectionObject; //visual ref
    bool isSelected = false;                    //hidden unless selected

    [SerializeField] bool isEnemy = false;      //player unit or enemy unit
    [SerializeField] Material enemyMat;         //visual change
    private int enemyLayer;                     //player unit objects layer changes to enemy

    //don't forget unitSelection is hooked to the camera
    [SerializeField] UnitSelection unitSelection;       //all the player units
    [SerializeField] LayerMask lineOfSightLayerMask;    //the layer the navigatable world sits

    Animator anim;          //make em groovy



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CR_NavigationTick());
        SetSelectionObjectVis(false);
    }

    private void Update()
    {

        //if this unit is designated an enemy...
        if (isEnemy)
        {
            EnemyAI();
        }

        if (anim != null)
        {
            anim.SetFloat("Velocity", agent.velocity.magnitude / 4f);
        }
        
    }

    public void DramaticPause(int secs, string animation)
    {
        IEnumerator cr = CR_DramaticPause(secs, animation);
        StartCoroutine(cr);
    }


    IEnumerator CR_DramaticPause(int secs, string animation)
    {
        //print("start the dramatic pause for " + gameObject.name);        
        StopMoving();                           //stay where you are,
        anim.SetTrigger(animation);             //perform your animation
        yield return new WaitForSeconds(secs);      //then in secs# of seconds
        //print(gameObject.name + " calling GoOnPatrol from CR_DramaticPause");
        ResumeMoving();                         //resume being able to move
        GoOnPatrol();                           //where is it going to
        //print("end the dramatic pause for " + gameObject.name);
    }

    IEnumerator CR_NavigationTick()
    {
        while (true)
        {
            if(isSelected || isEnemy)
            {
                if (destinationTrans != null)
                {
                    agent.SetDestination(destinationTrans.position);
                }
                else
                {
                    Debug.LogError("Unit->CR_NavigationTick has no destinationTrans ref: " + gameObject.name);
                }
                if (unitSelection.AreAllUnitsCaught())
                {
                    StopMoving();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void SetSelectionObjectVis(bool _isVisible)
    {
        selectionObject.SetActive(_isVisible);
    }
    public void SelectUnit()
    {
        SetSelectionObjectVis(true);
        isSelected = true;
    }
    public void DeselectUnit()
    {
        SetSelectionObjectVis(false);
        isSelected = false;
    }


    //when a unit is caught, it becomes an enemy and will chase other units
    public void ChangeUnitToEnemy()
    {

        //don't go anywhere yet, play the death animation
        //print("calling dramatic pause ZombieTransformation for " + gameObject.name);
        DramaticPause(5, "Death");

        //transform the unit into a proper zombie
        DeselectUnit();         //hide the highlighter
        isEnemy = true;         //set status to Zombie
        //put all of the game object and it's children to be the layer for zombies
        gameObject.layer = enemyLayer;
        Transform[] _childList = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _childList.Length; i++)
        {
            _childList[i].gameObject.layer = enemyLayer;
        }
        //change the material on the unit to be enemy colors
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = enemyMat;


        //Game logic- did all the humans get eaten
        if(unitSelection.AreAllUnitsCaught())
        {
            //print("//if this comes back true, then every unit is a 'zombie'");
            FindObjectOfType<TimerLogic>().SetTimerActive(false);
            unitSelection.AllUnitsStopMoving();
            
        }
        else
        {           
            GoOnPatrol();  //the game continues
        }
    }

    public void EnemyAI()
    {
        /* whee, I'm a zombie unit, yay, do I know where to go?*/
        if (destinationTrans != null)
        {
            /*yep I know where I'm going and now I need to know how close I get to there*/
            Vector3 _vecToTarget = destinationTrans.position - transform.position;
            float _distToTarget = _vecToTarget.magnitude;
            /*am I close enough to catch a human or be at the destination?*/
            if (_distToTarget <= 1.0f)
            {
                /*awesome I'm close enough to where I was going to count as I got there*/
                Unit _unit = destinationTrans.transform.GetComponentInParent<Unit>();
                if (_unit != null)
                {
                    /*this means that I caught someone*/
                    if (!_unit.CheckIfEnemy())
                    {
                        /*make tasty meatsack into new zombie friend*/
                        _unit.ChangeUnitToEnemy();  //the caught human needs to become a zombie now

                        /*zombie should pause moving here, to enjoy making new friends*/
                        //print("calling dramatic pause Zombie Attack for " + gameObject.name);
                        DramaticPause(3, "ZombieAttack");
                    }
                    else
                    {
                        if (!unitSelection.AreAllUnitsCaught())
                        {
                            GoOnPatrol();
                        }
                        else
                        {
                            StopMoving();   //the game's over
                        }
                    }
                }
                else
                {
                    GoOnPatrol();       //start by going on patrol
                }
            }
            else
            {
                GoOnPatrol();
            }
        }
        else
        {
            GoOnPatrol();
            //are there any targets at all? Are all the units caught? t/f
            if (unitSelection.AreAllUnitsCaught())
            {
                StopMoving();
            }
        }

    }

    public void GoOnPatrol()
    {
        //get a patrol waypoint to shuffle to
        //print(gameObject.name + "calling new patrol point from GoOnPatrol");
        destinationTrans = unitSelection.RandomPatrolWayPointTransform();
        LookForTarget();
    }

    public bool CheckIfEnemy()
    {
        return isEnemy;
    }
    public void StopMoving()
    {
        //print(gameObject.name + "setting right here as destination STOPMOVING");
        agent.SetDestination(gameObject.transform.position); //stay right here where you are unit
        agent.isStopped = true;
        
    }

    public void ResumeMoving()
    {
        //print(gameObject.name + "setting destination trans as desination RESUME MOVING");
        agent.SetDestination(destinationTrans.position);        
        agent.isStopped = false;
    }

    //The zombie enemy units will only chase after targets that they can directly see.
    public void LookForTarget()
    {
        //goal is to find appropr destinationTrans
        /*zombie hive brain knows all the humans a zombie can eat*/
        //declare list for all possible targets from unitSelection
        List<Unit> _allTargetsList = new List<Unit>();
        //go thru the list of all targets, and if they're not enemy units add tothe list
        for (int i = 0; i < unitSelection.GetUnitArray().Length; i++)
        {            
            if(!unitSelection.GetUnitArray()[i].isEnemy)
            {
                _allTargetsList.Add(unitSelection.GetUnitArray()[i]);
            }
        }

        /*this is all the humans the zombie can sense*/
        //go through list of player unit targets and make collection of visibile in-vision-cone targets
        List<Unit> _allVisibleTargetsList = new List<Unit>();
        for (int i = 0; i < _allTargetsList.Count; i++)
        {
            if (TargetIsInVisionCone(_allTargetsList[i].transform))
            {
                _allVisibleTargetsList.Add(_allTargetsList[i]);
                //print("adding to target list");
            }
        }

        /*this is all the humans the zombie can SEE!*/
        //this is where line of sght goes
        //lists are narrowed down
        List<Unit> _allVisibleTargetsNotBlockedByObstacleList = new List<Unit>();
        for (int i = 0; i < _allVisibleTargetsList.Count; i++)
        {   
            //if Linecast returns false, there is nothing blocking a line between these two points,
            //Layermask is looking for any objects sitting on that layer between the points of the line 
                ////ie; the walls of the maze are on the default layer, so set mask of game object to default
            if(!Physics.Linecast(transform.position, _allVisibleTargetsList[i].transform.position, lineOfSightLayerMask)) 
            {                
                _allVisibleTargetsNotBlockedByObstacleList.Add(_allVisibleTargetsList[i]);
            }

        }
        //set destination trans as closest visible enemy
        Unit _closestVisibleUnit = null;
        float _closestdistance = Mathf.Infinity;

        //lists go all targets -> all cone of xraysight visible targets -> all visible targets with clear line of sight
        for (int i = 0; i < _allVisibleTargetsNotBlockedByObstacleList.Count; i++)
        {
            //if the distance between this Enemy units position and the Target units position is less than the closest distance
            if(Vector3.Distance(this.transform.position,_allVisibleTargetsNotBlockedByObstacleList[i].transform.position)<_closestdistance)
            {
                _closestdistance = Vector3.Distance(this.transform.position, _allVisibleTargetsNotBlockedByObstacleList[i].transform.position);
                _closestVisibleUnit = _allVisibleTargetsNotBlockedByObstacleList[i];
            }
        }

        /*rawr zombie chases this human it sees!*/
        //all success, set the target
        if (_closestVisibleUnit!=null)
        {
            //make the zombie know it's chasing the human closest to it
            destinationTrans = _closestVisibleUnit.transform;
            //print(gameObject.name +" Zombie is chasing :" +_closestVisibleUnit.name);
        }

    }

    bool TargetIsInVisionCone( Transform _targetTrans ,float _sightWidth = 0.5f, bool _debugPrints = false)
    {
        //https://docs.unity3d.com/ScriptReference/Vector3.Dot.html this function relies on this
        Vector3 _v3ToTarget = (_targetTrans.position - transform.position).normalized;
        //Target - Current Transform -> normalized (normalized means magnitude of 1)
        if (_debugPrints)
        {
            print("forward when > 0) || back when < 0) " + Vector3.Dot(transform.forward, _v3ToTarget));
        }
        return Vector3.Dot(transform.forward, _v3ToTarget) > 0.5f ? true : false;
    }

}
