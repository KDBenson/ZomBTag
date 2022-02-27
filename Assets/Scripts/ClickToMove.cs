using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
/* This script handles moving the destination point object for the user
 * A raycast is used to determine if the user selected a valid point,
 * if so then the position of the destination setting object is changed to match
 */
public class ClickToMove : MonoBehaviour
{
    //camera to raycast from
    public Camera camera;
    //the game object that sets human agent destinations
    [SerializeField] Transform destinationTrans;
    //the layer that is navigatable ground
    private int LayerGround;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        //camera.orthographic = true; //set to orthographic
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
