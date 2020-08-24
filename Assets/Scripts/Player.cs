using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool forward, right, left;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        //Si el raycast hacia adelande del jugador detecta una pared
        if(Physics.Raycast(transform.position, transform.forward, out hit)){
            //Hace que la posición del jugador sea igual al de esa pared detectada
            //manteniendo su posición en y por si salta en plena transición
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
    }
}
