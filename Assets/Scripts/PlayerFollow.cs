using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        /**
        if (Input.GetButton("Horizontal"))
        {
            transform.Rotate(0, Input.GetAxisRaw("Horizontal"), 0);
            
        }
        **/
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);        
        transform.rotation = rotation;
        //rotation = Quaternion.LookRotation(relativePos, Vector3.down);
        //transform.rotation = rotation;
        //rotation = Quaternion.LookRotation(relativePos, Vector3.right);
        //transform.rotation = rotation;
        //rotation = Quaternion.LookRotation(relativePos, Vector3.left);
        //transform.rotation = rotation;



    }


}
