 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)
        //{
        //    StartGame();
        //}
    }

    public void StartGame()
    {
        //setGameState(GameState.inGame);
        collectedObject = 0;

        if(!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }

    }

    public void GameOver()
    {
        setGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
        setGameState(GameState.menu);
    }

    void setGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            //MenuManager.sharedInstance.ShowMainMenu();
            //MenuManager.sharedInstance.HideGameScore();
            //MenuManager.sharedInstance.HideGameOverMenu();

        }
        else if (newGameState == GameState.lvl1)
        {
            //LevelManager.sharedInstance.RemoveAllLevelBlocks();
            //LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();
            //MenuManager.sharedInstance.HideMainMenu();
            //MenuManager.sharedInstance.ShowGameScore();
            //MenuManager.sharedInstance.HideGameOverMenu();
            
        }
        else if (newGameState == GameState.gameOver)
        {
            //MenuManager.sharedInstance.Invoke("ShowGameOverMenu",1.8f);
            //MenuManager.sharedInstance.Invoke("HideGameScore",1.8f);
        }

        this.currentGameState = newGameState;
    }
    /**
    public void CollectedObject(Collectables collectable)
    {
        collectedObject += collectable.value;
    }
    **/

}
