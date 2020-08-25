using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //objeto a seguir  
    public Transform target;

    //a que distancia debe seguir el objeto
    public Vector3 offset = new Vector3();

    //tiempo de amortiguacion para que no haga un cambio bruzco del movimiento
    public float dampingTime = 0.3f;

    //Va a ser la velocidad a la que tiene que ir la camara
    public Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        //intenta ir la app a 60 frames por segundo
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }

    //La variable smooth me indicará si va a realizar un cambio inmediato
    //o va a ir poco a poco
    void MoveCamera(bool smooth)
    {
        Vector3 destination = new Vector3(
            target.position.x - offset.x,
            offset.y,
            offset.z
        );
        if (smooth)
        {
            //metodo muy utilizado para realizar ese seguimiento
            //de forma suave tipo cinematica a un objeto
            //la velocidad se pasa por referencia porque Unity
            //la saca del script, la lleva a otro y hace ciertos
            //calculos y la regresa, esto nos permite ver en todo
            //momento cual es velocidad de la camara sin yo tener
            //que calcularla si no que todo lo hará el mismo metodo
            //SmoothDamp, siempre y cuando se lo pasemos por referencia,
            //con la palabra clave ref
            this.transform.position = Vector3.SmoothDamp(
                    this.transform.position,
                    destination,
                    ref velocity,
                    dampingTime
                );
        }
        else
        {
            this.transform.position = destination;
        }
    }
}
