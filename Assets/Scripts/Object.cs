using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float speed;

    public GameObject decal;

    Rigidbody rig;

    GameObject audio;

    void Awake(){
        rig = GetComponent<Rigidbody>();
        audio = GameObject.Find("ObjectShooter");
    }

    // Start is called before the first frame update
    void Start(){
        rig.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col){
        audio.GetComponent<ObjectShooter>().PlayCrak();
        Instantiate(decal, col.GetContact(0).point, Quaternion.FromToRotation(Vector3.forward, col.GetContact(0).normal));
        rig.useGravity = true;
        Destroy(gameObject);
    }
}
