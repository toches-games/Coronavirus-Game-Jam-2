using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7f;
    public AudioSource fallDown;
    float runningSpeed;
    Rigidbody2D playerRb;
    Animator animator;
    Vector3 startPosition;
    const string IS_ALIVE = "isAlive";
    const string IS_ON_THE_GROUND = "isOnTheGround";
    const string VERTICAL_FORCE = "verticalForce";
    const string HORIZONTAL_FORCE = "horizontalForce";
    const string DAMAGE_ENEMY = "damageEnemy";

    int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 200, INITIAL_MANA = 15,
        MIN_HEALTH = 10, MIN_MANA = 0, MAX_HEALTH = 200, MAX_MANA = 30;

    public const int SUPERJUMP_COST = 10;
    public const float SUPERJUMP_FORCE = 2f;

    public float jumpRaycastDistance = 1.5f;

    //Variables para el conseguir el swipe up y saltar
    Vector2 startTouchPosition, endTouchPosition;
    bool jumpSuper = false;
    bool jumpNormal = false;

    //variable publica donde meteremos la referencia a la capa o layer 
    //con la que vamos a refernciar los rayos o raycast
    public LayerMask groundMask; 

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        runningSpeed = GameManager.sharedInstance.gameSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Jump(false);

        //}
        //if (Input.GetButtonDown("SuperJump"))
        //{
        //    Jump(true);
        //}

        SwipeCheck();
        if (jumpNormal)
        {
            Jump(false);

        }
        if (jumpSuper)
        {
            Jump(true);
        }

        //Movimiento con las teclas izq o der
        //playerRb.velocity = new Vector2(Input.GetAxis("Horizontal") * runningSpeed, playerRb.velocity.y);

        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //    GetComponent<SpriteRenderer>().flipX = false;
        //}else if(Input.GetAxis("Horizontal") < 0)
        //{
        //    GetComponent<SpriteRenderer>().flipX = true;
        //}

        animator.SetBool(IS_ON_THE_GROUND, isTouchingTheGround());
        animator.SetFloat(VERTICAL_FORCE, playerRb.velocity.y);
        animator.SetFloat(HORIZONTAL_FORCE, playerRb.velocity.x * 0.23f);
        
        //Debug.Log( (int)GetTraveledDistance() % 10);

        Debug.DrawRay(this.transform.position, Vector3.down * jumpRaycastDistance, Color.red);
    }

    void FixedUpdate()
    {

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (playerRb.velocity.x < runningSpeed)
            {
                playerRb.velocity = new Vector2(runningSpeed,
                                                playerRb.velocity.y
                                                );
            }
            //Aumento de velocidad del jugador
            if ((int)GetTraveledDistance() % 55 == 0 && GetTraveledDistance() > 10f)
            {
                runningSpeed += 0.03f;
            }
        }
        else //si no estamos dentro de la partida
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            runningSpeed = GameManager.sharedInstance.gameSpeed;
        }

        
    }

    public void Jump(bool superJump)
    {
        float jumpForceFactor = jumpForce;

        if (isTouchingTheGround() && GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (superJump && manaPoints >= SUPERJUMP_COST)
            {
                manaPoints -= SUPERJUMP_COST;
                jumpForceFactor *= SUPERJUMP_FORCE;
                
            }
            //ForceMode2D me dos opcionesm force que seria como una 
            //fuerza constante e impulse que seria como aplicar una
            //fuerza en instante nada mas.
            playerRb.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
            jumpSuper = false;
            jumpNormal = false;
        }
    }

    //Swipe up
    void SwipeCheck()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            if (endTouchPosition.y - 200f > startTouchPosition.y)
            {
                jumpSuper = true;
            }
            else
            {
                jumpNormal = true;
            }
        
        }
        //Debug.Log(startTouchPosition.y);
        //Debug.Log(endTouchPosition.y);
    }

    bool isTouchingTheGround()
    {
        //Probamos varias distancias (0.2f) para que funciones bien
        if (Physics2D.Raycast(this.transform.position, 
                                Vector2.down, jumpRaycastDistance, groundMask)) 
        {
            //Solo fue una prueba GameManager.sharedInstance.currentGameState = GameState.inGame;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Die()
    {
        float travelledDistance = GetTraveledDistance();
        //PlayerPrefs funciona como una variable que puede guardar y permite
        //acceder a valores entre sesiones de juego (investigar mas)
        float previousMaxDistance = PlayerPrefs.GetFloat("maxScore", 0f);
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxScore", travelledDistance);
        }

        animator.SetBool(IS_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void StartGame()
    {
        animator.SetBool(IS_ALIVE, true);
        animator.SetBool(IS_ON_THE_GROUND, true);
        //Me permite invocar el metodo un tiempo despues
        Invoke("RestartPosition", 0.2f);
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
    }

    void RestartPosition()
    {
        if(this.transform.position.x >= 0f)
        {
            this.transform.position = startPosition;
        }

        this.playerRb.velocity = Vector2.zero;

        Camera.main.GetComponent<CameraFollow>().ResetCameraPosition();

        //GameObject mainCamera = GameObject.Find("Main Camera");
        //mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints > MAX_HEALTH)
        {
            healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <= 0)
        {
            Die();
        }
        if (points <= 0)
        {
            animator.SetTrigger("damageEnemy");
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints > MAX_MANA)
        {
            manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTraveledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }

    

}