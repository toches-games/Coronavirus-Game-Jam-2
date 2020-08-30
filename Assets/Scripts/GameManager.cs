 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//Aqui realizamos todo el manejo de los estados del juego, cambio
//de escenas cuando pierde, menu principal y modo jugando.

public enum GameState
{
    menu,
    lvl1,
    lvl2,
    lvl3,
    lvl4,
    lvl5,
    lvl6,
    gameOver
}

public class GameManager : MonoBehaviour
{

    public GameState currentGameState = GameState.lvl1;
    public static GameManager sharedInstance;
    public int collectedObject = 0;
    PlayerController controller;
    public float gameSpeed = 7f;
    AudioSource backgroundAudioSource;

    public List<CinemachineVirtualCamera> cameras;
    public List<PlayableDirector> playableDirector;
    public GameObject hud;
    public GameObject gameOver_Display;

    //Guarda las coroutinas de las herramientas para detenerlas en cada cambio de nivel
    Coroutine hammerCoroutine;
    Coroutine drillCoroutine;
    //------------------------------------------

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        controller = GameObject.Find("Player").
            GetComponent<PlayerController>();
        //backgroundAudioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        /**
        if(!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
        **/
        NextLevel(1);

    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    public void NextLevel(int lvl)
    {
        /*if(hammerCoroutine != null){
            StopCoroutine(hammerCoroutine);
            EnemyManager.sharedInstance.start = false;
        }

        if(drillCoroutine != null){
            EnemyManager.sharedInstance.start = false;
            StopCoroutine(drillCoroutine);
        }*/

        StopAllCoroutines();
        EnemyManager.sharedInstance.start = false;

        SetGameState((GameState)lvl);
    }

    void SetGameState(GameState newGameState)
    {
        //Actualiza el nivel en el EnemyManager
        EnemyManager.sharedInstance.currentState = newGameState;

        if (newGameState == GameState.menu)
        {

        }
        else if (newGameState == GameState.lvl1)
        {
            cameras[(int)newGameState - 1].gameObject.SetActive(true);
            cameras[(int)newGameState - 1].Priority += (int)newGameState;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ
                                                                | RigidbodyConstraints.FreezeRotation;
            Invoke("ShowHUD", 2f);
        }
        else if (newGameState == GameState.lvl2)
        {
            //Inicia y guarda la coroutina del martillo
            hammerCoroutine = StartCoroutine(EnemyManager.sharedInstance.HammerMovement());

            //Destroy(cameras[0]);
            //cameras[0] = null;
            //cameras.RemoveAt(0);
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.lvl3)
        {
            //Inicia y guarda la coroutina del taladro
            drillCoroutine = StartCoroutine(EnemyManager.sharedInstance.DrillMovement());

            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.lvl4)
        {
            //Inicia y guarda la coroutina del martillo y taladro
            hammerCoroutine = StartCoroutine(EnemyManager.sharedInstance.HammerMovement());
            drillCoroutine = StartCoroutine(EnemyManager.sharedInstance.DrillMovement());

            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if(newGameState == GameState.lvl5)
        {
            EnemyManager.sharedInstance.objectShooter.SetActive(true);
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.gameOver)
        {
            StopAllCoroutines();
            cameras[cameras.Count-1].gameObject.SetActive(true);
            Invoke("ShowGameOver", 2.5f);
            Invoke("HideHUD", 0.5f);

        }


        if ((int)newGameState > 1 && newGameState !=GameState.gameOver)
        {
            cameras[(int)newGameState - 1].gameObject.SetActive(true);
            cameras[(int)newGameState - 1].Priority += (int)newGameState;
            playableDirector[(int)newGameState - 2].Play();
            cameras[(int)newGameState-2].gameObject.SetActive(false);
        }
        this.currentGameState = newGameState;
    }
    
    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowHUD()
    {
        hud.SetActive(true);
    }
    public void HideHUD()
    {
        hud.SetActive(false);
    }
    public void ShowGameOver()
    {
        gameOver_Display.SetActive(true);
    }
}
