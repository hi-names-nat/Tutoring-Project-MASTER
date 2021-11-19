/********************************************************************************************************
 * 
 * ***NAME***
 * snapToFloor
 * 
 * ***DESCRIPTION***
 * basic script to ensure whatever object attached starts on the floor
 * 
 * ***USAGE***
 * Place onto any gameobject
 * 
 * ***AUTHOR***
 * Natalie Soltis
 * 
 * 
 * ***GENERAL TODO***
 * 
 * 
 * * none as-of 2/28
 * 
 * 
 *******************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class snapToFloor : MonoBehaviour
{
    [Tooltip("If to or not destroy this object if it cannot find a surface to snap itself to")]
    [SerializeField]
    private bool destroyIfCantSnap = false;

    // Start is called before the first frame update
    private void Awake()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, new Vector3(0, -200, 0));
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 200))
        {
            print("got here");
            GetComponent<CharacterController>().transform.position = new Vector3(transform.position.x, hit.collider.bounds.ClosestPoint(transform.position).y + GetComponent<CharacterController>().bounds.extents.y, transform.position.z);
        }
        else if (destroyIfCantSnap) Destroy(this.gameObject);
    }


}
