using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    //Se manda a destruir el collider del agujero cuando apenas salga.
    //Como el Destroy se ejecuta al final del frame actual, sigue comprobando
    //si choca con el jugador en el instante que es creado
    void Start(){
        Destroy(GetComponent<BoxCollider>());
    }

    void OnTriggerEnter(Collider other){
        if(!EnemyManager.sharedInstance.start){
            return;
        }
        
        //Si choca con el jugador
        if(other.name == "Player"){
            if(other.GetComponent<PlayerController>().GetHealth() > 0)
            {
                other.GetComponent<PlayerController>().CollectHealth(-1);
            }
        }
    }
}
