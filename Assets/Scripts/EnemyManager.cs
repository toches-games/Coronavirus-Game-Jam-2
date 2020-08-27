using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Hammer Settings")]

    //Velocidad con la que el martillo golpeará
    [Range(0, 5)]
    public float hammerSpeedAttack = 5f;

    //Tiempo que tardará en realizar el ataque el martillo
    [Range(0, 5)]
    public float hammerDelayAttack = 1f;

    [Header("Drill Settings")]

    //Velocidad con la que el taladro golpeará
    [Range(0, 5)]
    public float drillSpeedAttack = 1f;

    //Tiempo que tardará en realizar el ataque el taladro
    [Range(0, 5)]
    public float drillDelayAttack = 1f;

    [Header("Decals")]

    //Prefab del agujero que se colocará en la pared
    public GameObject[] decals;

    [Header("Others")]

    //Referencia a painter
    public GameObject painter;

    //Referencia al player
    Transform player;

    //Estado actual del juego
    GameState currentState;

    //Referencia al objeto que se moverá para atacar en esa posición
    Transform instantiatePosition;

    void Awake(){
        player = GameObject.Find("Player").transform;
        instantiatePosition = transform.GetChild(0);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
            //Comprueba el estado del juego para saber que enemigo será el que ataca
            //dependiendo del nivel
        while(true){
            //Se guarda el estado actual del juego
            currentState = GameManager.sharedInstance.currentGameState;

            //Mientras que el juego esté en el menú y en el nivel 1 o el jugador muere
            //no hace nada y vuelve a comprobar el estado 2 segundos despues
            while(currentState == GameState.menu || currentState == GameState.gameOver /*|| currentState == GameState.lvl1*/){
                yield return new WaitForSeconds(2f);
                currentState = GameManager.sharedInstance.currentGameState;
            }

            //Mientras se hace la animación no hará nada por 4 segundos
            if(painter.activeSelf){
                yield return new WaitForSeconds(4f);
            }

            if(currentState == GameState.lvl1){

                yield return StartCoroutine(HammerAttack());
            }

            /*if(currentState == GameState.lvl3){
                yield return StartCoroutine(DrillAttack());
            }*/
        }
    }

    IEnumerator HammerAttack(){
        Vector3 targetPosition = instantiatePosition.position;

        yield return new WaitForSeconds(hammerDelayAttack);

        //Se crea el agujero en la pared
        Transform temp = Instantiate(decals[Random.Range(3, decals.Length)], targetPosition, instantiatePosition.rotation * Quaternion.Euler(0, 180, Random.Range(0f, 360f))).transform;
        temp.SetParent(transform);

        yield return new WaitForSeconds(hammerSpeedAttack);
    }

    IEnumerator DrillAttack(){
        Vector3 targetPosition = instantiatePosition.position;

        yield return new WaitForSeconds(drillDelayAttack);

        //Se crea el agujero en la pared
        Transform temp = Instantiate(decals[Random.Range(0, 3)], targetPosition, instantiatePosition.rotation * Quaternion.Euler(0, 180, Random.Range(0f, 360f))).transform;
        temp.SetParent(transform);

        yield return new WaitForSeconds(drillSpeedAttack);
    }

    //Hacemos que el collider donde se crearán los ataques al rededor del jugador
    //tenga la misma posición y rotación que el jugador
    //para que pueda estar siempre donde el jugador, y colocar los agujeros en dirección
    //a la pared (que es la misma rotación que tendrá el jugador)
    void FixedUpdate(){
        instantiatePosition.position = player.position;
        instantiatePosition.rotation = player.rotation;
    }
}
