using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    //Grados que girará el jugador cuando toque el trigger en la curva para girar
    public int degrees;

    void OnTriggerEnter(Collider other){
        //Si detecta que el jugador entra en el collider
        if(other.gameObject.CompareTag("Player")){
            //Lo gira los grados que se hayan puesto
            other.transform.rotation *= Quaternion.Euler(0, degrees, 0);

            //Y se elimina el collider para que no vuelva a detectar y girar de nuevo
            Destroy(gameObject);
        }
    }
}
