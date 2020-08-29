﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Hammer Settings")]

    //Tiempo que tardará en realizar el ataque el martillo
    [Range(1, 20)]
    public float hammerDelayAttack = 1f;

    [Range(1, 20)]
    public float hammerSpeed = 1f;

    [Header("Drill Settings")]

    //Velocidad con la que el taladro golpeará
    [Range(0, 5)]
    public float drillDelayAttack = 1f;

    //Tiempo que tardará en realizar el ataque el taladro
    [Range(0, 5)]
    public float drillSpeed = 1f;

    [Header("Decals")]

    //Prefabs del agujero que se colocarán en la pared
    public GameObject[] decals;

    //Prefab de las grietas que se crearán para el taladro
    public GameObject[] crack;

    [Header("Others")]

    //Referencia al martillo que se moverá para atacar en esa posición
    public Transform hammerPosition;

    //Referencia al taladro que se moverá para atacar en esa posición
    public Rigidbody drillPosition;

    //Referencia al player
    public Transform player;

    //Estado actual del juego
    [HideInInspector]
    public GameState currentState;

    bool drillAttack = false;
    bool close = false;

    public static EnemyManager sharedInstance;

    [HideInInspector]
    public bool start;

    //----- Audio -----
    public AudioClip[] hammerClips;
    public AudioClip[] drillClips;

    AudioSource hammerSource;
    AudioSource drillSource;

    Vector3 drillAttackPosition;

    void Awake()
    {
        if (sharedInstance == null){
            sharedInstance = this;
        }

        else{
            Destroy(this);
            return;
        }

        hammerSource = GetComponents<AudioSource>()[0];
        drillSource = GetComponents<AudioSource>()[1];
    }

    public IEnumerator HammerMovement()
    {
        yield return new WaitForSeconds(5f);
        hammerPosition.position = player.position + player.right * -4f;

        start = true;

        while(true){
            Vector3 targetPosition = hammerPosition.position;

                yield return StartCoroutine(AnimateHammer(targetPosition));

                hammerSource.clip = hammerClips[Random.Range(0, hammerClips.Length)];
                hammerSource.Play();
                Transform temp = Instantiate(decals[Random.Range(3, decals.Length)], targetPosition, hammerPosition.rotation * Quaternion.Euler(0, 180, Random.Range(0f, 360f))).transform;
                temp.SetParent(transform);

            yield return null;
        }
    }

    IEnumerator AnimateHammer(Vector3 targetPosition){
        float angle = 0f;
        Transform animate = hammerPosition.GetChild(0);

        do{
            float distance = Vector3.Distance(animate.position, targetPosition - Vector3.up * 2f);
            animate.localRotation = Quaternion.Euler(new Vector3(0, -90f, Mathf.Sin(angle * Mathf.Deg2Rad * hammerDelayAttack) * 50f));
            animate.position = Vector3.MoveTowards(animate.position, targetPosition - Vector3.up * 2f, distance * hammerSpeed * Time.deltaTime);
            angle++;
            yield return null;
        }while(angle <= 180/hammerDelayAttack);
    }

    public IEnumerator DrillMovement(){
        yield return new WaitForSeconds(5f);
        drillPosition.position = player.position + player.right * -4f;
        start = true;

        while(true){
            Vector3 targetPosition = drillPosition.position;
            float playerDistance = Vector2.Distance(new Vector2(targetPosition.x, targetPosition.z), new Vector2(player.position.x, player.position.z));

                if(playerDistance >= 3f){
                    if(Random.Range(1, 101) < 2){
                        close = false;
                        drillAttack = true;
                        drillSource.clip = drillClips[Random.Range(0, drillClips.Length)];
                        drillSource.Play();
                        yield return StartCoroutine(AnimateDrill(targetPosition));
                    }
                }

                else{
                    close = true;
                    drillSource.clip = drillClips[Random.Range(0, drillClips.Length)];
                    drillSource.Play();
                    yield return StartCoroutine(AnimateDrill(targetPosition)); 
                }

            yield return null;
        }
    }

    IEnumerator AnimateDrill(Vector3 targetPosition){
        float angle = 0f;
        Transform animate = drillPosition.transform.GetChild(0);
        int crackCount = 0;

        do{
            animate.localPosition = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad * drillDelayAttack) * 10f, -0.95f, -0.64f);
            angle++;

            if(angle <= 90/drillDelayAttack){
                //animate.localRotation = Quaternion.Euler(new Vector3(-90, 0, Random.Range(0f, 10f)));
                //Vector3 particlePosition = new Vector3(animate.position.x + 2.1f, targetPosition.y, drillPosition.position.z);
                //Vector3 particlePosition = tr animate.position.x + 2.1f, targetPosition.y, drillPosition.position.z);

                Transform temp = Instantiate(crack[crackCount++], drillAttackPosition, drillPosition.rotation * Quaternion.Euler(0, 180, 0)).transform;
                temp.SetParent(transform);
            }

            if(crackCount >= crack.Length){
                crackCount = 0;
            }

            yield return null;
        }while(angle <= 180/drillDelayAttack);

        drillAttack = false;
    }

    //Hacemos que el collider donde se crearán los ataques al rededor del jugador
    //tenga la misma posición y rotación que el jugador
    //para que pueda estar siempre donde el jugador, y colocar los agujeros en dirección
    //a la pared (que es la misma rotación que tendrá el jugador)
    void FixedUpdate(){
        if((currentState == GameState.lvl2 || currentState == GameState.lvl4) && start){
            hammerPosition.position = player.position;
            hammerPosition.rotation = player.rotation;
        }

        if((currentState == GameState.lvl3 || currentState == GameState.lvl4) && start){
            if(!drillAttack){
                float yDistance = Mathf.Abs(drillPosition.position.y - player.position.y);

                drillPosition.position = new Vector3(drillPosition.position.x,
                    Mathf.MoveTowards(drillPosition.position.y, player.position.y, yDistance * drillSpeed * 2 * Time.deltaTime),
                    drillPosition.position.z);
            }

            if(!close){
                drillPosition.MovePosition(drillPosition.position + drillPosition.transform.right * drillSpeed * Time.deltaTime);
                drillPosition.rotation = player.rotation;
            }
            
            Debug.DrawRay(drillPosition.position, drillPosition.transform.forward, Color.green);

            RaycastHit hit;
            if(Physics.Raycast(drillPosition.transform.GetChild(0).position + drillPosition.transform.forward * -2f, drillPosition.transform.forward, out hit)){
                drillAttackPosition = hit.point;
            }
        }
    }
}
