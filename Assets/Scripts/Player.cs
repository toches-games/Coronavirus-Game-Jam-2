using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Guarda la pared actual
    RaycastHit currentWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Guarda la siguiente pared a la que va a moverse
        RaycastHit nextWall;

        //Si el raycast hacia adelande del jugador detecta una pared
        if(Physics.Raycast(transform.position, transform.forward, out nextWall) && currentWall.collider != nextWall.collider){
            //Hace que la posición del jugador sea igual al de esa pared detectada
            //manteniendo su posición en y por si salta en plena transición
            //solo si collisiona con una pared (el collider no es nulo)
            if(nextWall.collider){
                transform.position = new Vector3(nextWall.point.x, transform.position.y, nextWall.point.z);
                currentWall = nextWall;
            }
        }
    }
}
