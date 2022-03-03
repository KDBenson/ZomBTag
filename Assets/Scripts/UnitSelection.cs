using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*This script handles selecting human units and the patrol points for zombie units
 */
public class UnitSelection : MonoBehaviour
{
    public Camera camera;
    [SerializeField] LayerMask unitLayer;
    [SerializeField] Unit[] units;

    [SerializeField] Transform[] patrolWayPoints;

    [SerializeField] TimerLogic gameTimer;
    [SerializeField] DisplayUIControl playerDisplay;

    public Unit[] GetUnitArray()
    {
        return units;
    }

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastFromCamera();
        }
        CheckWinCondition();
    }
    
    //This is for the user when selecting human units or deselecting all humans
    void RaycastFromCamera()
    {
        RaycastHit _hit;
        Ray _ray = camera.ScreenPointToRay(Input.mousePosition);
        //only game objects that are the friendly human unit layer will be recognized
        if (Physics.Raycast(_ray, out _hit, 99999f, unitLayer))
        {
            Unit _unit = _hit.transform.GetComponentInParent<Unit>();
            if(_unit != null && !_unit.CheckIfEnemy())
            {
                _unit.SelectUnit();
            }
        }
        else
        {
            //print("clicked nothing therefore unselect");
            for (int i = 0; i < units.Length; i++)
            {
                units[i].DeselectUnit();
            }
        }
    }

    public Transform RandomPatrolWayPointTransform()
    {
        Transform _trans = null;
        _trans = patrolWayPoints[Random.Range(0, patrolWayPoints.Length)].transform;

        return _trans;
    }

    public bool AreAllUnitsCaught()
    {
        bool _allEnemies = true; //true if every unit is an enemy
        for (int i = 0; i < units.Length; i++)
        {
            //CheckIfEnemy returns true when a unit is an enemy/zombie, false means it's an active target = game goes on
            if(!units[i].CheckIfEnemy())
            {
                //break beacuse as long as one returns false means at least one target is in play
                _allEnemies = false;
                break;
            }
        }
        return _allEnemies;
    }

    //This only has 2 references; both game over conditions.
    public void AllUnitsStopMoving()
    {
        Unit _unit = null;
        for (int i = 0; i < units.Length; i++)
        {
            _unit = units[i];
            _unit.StopMoving();
        }
        DisplayGameMessage();
    }

    public void CheckWinCondition()
    {
        if(gameTimer.GameOverCondition())
        {
            //we go in here if they won by beating the clock
            gameTimer.SetTimerActive(false); 
            AllUnitsStopMoving();            
        }
    }

    public void DisplayGameMessage()
    {
        playerDisplay.SetGameOverMessage();
        playerDisplay.ShowHideUI(true);
    }

    //This is used for terminator style zombie chasing, any random human unit is set as the target
    //Not currently used in this game
    public Transform RandomUnitTarget()
    {
        Unit _unit = null;
        Transform _trans = null;

        for (int i = 0; i < units.Length; i++)
        {
            _unit = units[Random.Range(0, units.Length)];
            if (_unit.CheckIfEnemy() == false)
            {
                _trans = _unit.transform;
                //print("UnitSelection -> RandomUnitTarget is targeted at: " + _unit.transform.name);
                break;
            }
        }
        return _trans;
    }

}
