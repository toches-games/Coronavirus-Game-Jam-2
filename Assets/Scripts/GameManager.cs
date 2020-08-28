﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

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
        SetGameState((GameState)lvl);
    }

    void SetGameState(GameState newGameState)
    {

        if (newGameState == GameState.menu)
        {

        }
        else if (newGameState == GameState.lvl1)
        {
            cameras[(int)newGameState - 1].gameObject.SetActive(true);
            cameras[(int)newGameState - 1].Priority += (int)newGameState;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.lvl2)
        {
            //Destroy(cameras[0]);
            //cameras[0] = null;
            //cameras.RemoveAt(0);
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.lvl3)
        {
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.lvl4)
        {
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX
                                                                | RigidbodyConstraints.FreezeRotation;
        }
        else if (newGameState == GameState.gameOver)
        {
            //MenuManager.sharedInstance.Invoke("ShowGameOverMenu",1.8f);
            //MenuManager.sharedInstance.Invoke("HideGameScore",1.8f);
        }
        if((int)newGameState > 1)
        {
            cameras[(int)newGameState - 1].gameObject.SetActive(true);
            cameras[(int)newGameState - 1].Priority += (int)newGameState;
            playableDirector[(int)newGameState - 2].Play();
            cameras[(int)newGameState].gameObject.SetActive(false);
        }
        this.currentGameState = newGameState;
    }

}
