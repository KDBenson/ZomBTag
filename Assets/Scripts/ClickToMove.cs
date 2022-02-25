using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class ClickToMove : MonoBehaviour
{

    public Camera camera;

    [SerializeField] Transform destinationTrans;


    private int LayerGround;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        //camera.orthographic = true;

        LayerGround = LayerMask.NameToLayer("RaycastableDestination");

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastFromCamera();
        }
        
    }

    void RaycastFromCamera()
    {
        RaycastHit _hit;
        Ray _ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if(_hit.transform.gameObject.layer == LayerGround)
            {
                destinationTrans.position = _hit.point;
            }
            //Transform objectHit = _hit.transform;
            //print(_hit.point);
        }
    }


}
