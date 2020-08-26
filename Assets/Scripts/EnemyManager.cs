using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Velocidad con la que atacarán al jugador
    public float speedAttack = 5;

    //Prefab del agujero que se colocará en la pared
    public GameObject hole;

    //Referencia al collider que seguira al jugador (Este mismo objeto)
    //Se usa para elegir una posición aleatorea dentro de este collider
    //para atacar al jugador.
    //El collider es el area alrededor del jugador donde se podrá atacar
    //para que no se ataque en cualquier posición de la pared si no
    //solo alrededor del jugador
    BoxCollider collider;

    //Referencia al player
    Transform player;

    void Awake(){
        collider = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(true){
            //Se crea la posición donde se atacará
            Vector3 targetPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                transform.position.z);

            //Se crea el agujero en la pared
            Instantiate(hole, targetPoint, transform.rotation * Quaternion.Euler(0, 180, 0));

            yield return new WaitForSeconds(speedAttack);
        }
    }

    //Hacemos que el collider donde se crearán los ataques al rededor del jugador
    //tenga la misma posición y rotación que el jugador
    //para que pueda estar siempre donde el jugador, y colocar los agujeros en dirección
    //a la pared (que es la misma rotación que tendrá el jugador)
    void FixedUpdate(){
        transform.position = player.position;
        transform.rotation = player.rotation;
    }
}
