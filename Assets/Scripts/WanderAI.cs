using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAI : MonoBehaviour
{    
    NavMeshAgent agent;
    private Transform destinationTarget;

    private int LayerGround;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        LayerGround = LayerMask.NameToLayer("Default");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            print("todo message from wanderAI, nothing's been implemented yet");
        }
    }





}
