using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float speed;

    public int damage;

    public GameObject decal;

    Rigidbody rig;

    [HideInInspector]
    public ObjectShooter objectShooter;

    void Awake(){
        rig = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start(){
        rig.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void Update(){
        if(GameManager.sharedInstance.currentGameState == GameState.gameOver){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col){
        objectShooter.PlayCrak();
        Instantiate(decal, col.GetContact(0).point, Quaternion.FromToRotation(Vector3.forward, col.GetContact(0).normal));
        
        if(col.transform.CompareTag("Player")){
            col.transform.GetComponent<PlayerController>().CollectHealth(-1);
        }

        else{
            objectShooter.Damage(damage);
        }

        Destroy(gameObject);
    }
}
