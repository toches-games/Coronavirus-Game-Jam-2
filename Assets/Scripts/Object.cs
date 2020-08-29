using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float speed;

    Rigidbody rig;

    void Awake(){
        rig = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start(){
        rig.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(){
        rig.useGravity = true;
        Destroy(gameObject, 1f);
    }
}
