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

    //Prefabs del agujero que se colocarán en la pared
    public GameObject[] decals;

    //Prefab de las grietas que se crearán para el taladro
    public GameObject[] crack;

    [Header("Others")]

    //Referencia a painter
    public GameObject painter;

    //Referencia al martillo que se moverá para atacar en esa posición
    public Transform hammerPosition;

    //Referencia al taladro que se moverá para atacar en esa posición
    public Rigidbody drillPosition;

    //Referencia al player
    Transform player;

    //Estado actual del juego
    GameState currentState;

    void Awake(){
        player = GameObject.Find("Player").transform;
    }

    void Start(){
        currentState = GameManager.sharedInstance.currentGameState;

        //StartCoroutine(HammerMovement());
        StartCoroutine(DrillMovement());
    }

    IEnumerator HammerMovement()
    {
        while(true){
            Vector3 targetPosition = hammerPosition.position;

            yield return new WaitForSeconds(hammerDelayAttack);

            Transform temp = Instantiate(decals[Random.Range(3, decals.Length)], targetPosition, hammerPosition.rotation * Quaternion.Euler(0, 180, Random.Range(0f, 360f))).transform;
            temp.SetParent(transform);

            yield return new WaitForSeconds(hammerSpeedAttack);
        }
    }

    IEnumerator DrillMovement(){
        /*bool attack = false;

        drillPosition.position = player.position + drillPosition.transform.right * -2f;

        while(true){
            if(Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(drillPosition.position.x, drillPosition.position.z)) < 1f){
                float distance = Mathf.Abs(drillPosition.position.y - player.position.y);
                int crackCount = 0;
                
                drillPosition.position = new Vector3(drillPosition.position.x,
                                                Mathf.MoveTowards(drillPosition.position.y, player.position.y, 10f * distance * Time.deltaTime),
                                                drillPosition.position.z);

                Transform temp = Instantiate(crack[crackCount++], drillPosition.transform.GetChild(0).position, drillPosition.rotation * Quaternion.Euler(0, 180, 0)).transform;
                temp.SetParent(transform);

                if(crackCount >= crack.Length){
                    crackCount = 0;
                }
                    
                distance = Mathf.Abs(drillPosition.position.y - player.position.y);

                yield return null;
            }

            else{
                if(!attack){
                    float distance = Mathf.Abs(drillPosition.position.y - player.position.y);
                    drillPosition.position = new Vector3(drillPosition.position.x,
                                                        Mathf.MoveTowards(drillPosition.position.y, player.position.y, 2f * distance * Time.deltaTime),
                                                        drillPosition.position.z);
                        
                    float random = Random.Range(1, 101);

                    if(random <= 1){
                        attack = true;
                        yield return new WaitForSeconds(1f);
                    }
                }

                if(attack){
                    float value = 0;
                    int crackCount = 0;
                    float distanceToPlayer = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(drillPosition.position.x, drillPosition.position.z));

                    while(value < 180/2 && distanceToPlayer > 1f){
                        distanceToPlayer = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(drillPosition.position.x, drillPosition.position.z));
                        drillPosition.transform.GetChild(0).localPosition = drillPosition.transform.right * Mathf.Sin(value * Mathf.Deg2Rad * 2) * 10f;
                        value++;

                        if(value <= 180/2/2){
                            Transform temp = Instantiate(crack[crackCount++], drillPosition.transform.GetChild(0).position, drillPosition.rotation * Quaternion.Euler(0, 180, 0)).transform;
                            temp.SetParent(transform);

                            if(crackCount >= crack.Length){
                                crackCount = 0;
                            }
                        }
                        yield return null;
                    }
                    attack = false;
                }
            }
            yield return null;
        }*/

        while(true){
            yield return new WaitForSeconds(drillDelayAttack);

            Transform temp = Instantiate(decals[0], drillPosition.position, hammerPosition.rotation * Quaternion.Euler(0, 180, Random.Range(0f, 360f))).transform;
            temp.SetParent(transform);
        }
    }

    //Hacemos que el collider donde se crearán los ataques al rededor del jugador
    //tenga la misma posición y rotación que el jugador
    //para que pueda estar siempre donde el jugador, y colocar los agujeros en dirección
    //a la pared (que es la misma rotación que tendrá el jugador)
    void FixedUpdate(){
        if(currentState == GameState.lvl1){    
            hammerPosition.position = player.position;
            hammerPosition.rotation = player.rotation;
        }

        if(currentState == GameState.lvl1){
            /*float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(drillPosition.position.x, drillPosition.position.z));
            
            if(distance > 1f){
                drillPosition.MovePosition(drillPosition.position + drillPosition.transform.right * 3f * Time.deltaTime);
            }

            else{
                drillPosition.transform.GetChild(0).position = Vector3.MoveTowards(drillPosition.transform.GetChild(0).position, player.position, 5f * distance * Time.deltaTime);
            }

            drillPosition.rotation = player.rotation;*/

            float distance = Vector3.Distance(drillPosition.position, player.position);
            drillPosition.MovePosition(Vector3.MoveTowards(drillPosition.position, player.position, distance * 5f * Time.deltaTime));
            drillPosition.rotation = player.rotation;
        }
    }
}
