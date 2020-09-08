using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float gravity = 10f;
    public float jumpForce = 3f;
    public AudioSource fallDown;
    [SerializeField]
    float runningSpeed;
    Rigidbody playerRb;
    Animator animator;
    Vector3 startPosition;
    const string IS_ALIVE = "isAlive";
    const string IS_ON_THE_GROUND = "isOnTheGround";
    const string VERTICAL_FORCE = "verticalForce";
    const string HORIZONTAL_FORCE = "horizontalForce";
    const string DAMAGE_ENEMY = "damageEnemy";
    const string LAST_HORIZONTAL = "LastHorizontal";
    const string HORIZONTAL = "Horizontal";
    const string WALKING_STATE = "Walking";
    const string VERTICAL = "Vertical";
    const string IS_TOUCHING_GROUND = "isTouchingGround";
    int healthPoints;
    public const int INITIAL_HEALTH = 3,
        MIN_HEALTH = 1, MAX_HEALTH = 3;

    public float jumpRaycastDistance = 0.6f;
    public float positionSidesRaycast = 0.3f;
    Vector3 positionLeft;
    Vector3 positionRight;
    public List<Image>lives;

    //Variables para el conseguir el swipe up y saltar
    Vector2 startTouchPosition, endTouchPosition;

    //variable publica donde meteremos la referencia a la capa o layer 
    //con la que vamos a referenciar los rayos o raycast
    public LayerMask groundMask;

    GameState currentState;
    SpriteRenderer sprite;
    float lastMovement = 1f;
    bool walking = false;
    SFXManager sfx;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sfx = FindObjectOfType<SFXManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        runningSpeed = GameManager.sharedInstance.gameSpeed;
        positionLeft = Vector3.zero;
        positionRight = Vector3.zero;
        healthPoints = INITIAL_HEALTH;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        currentState = GameManager.sharedInstance.currentGameState;
        if (currentState == GameState.menu || currentState == GameState.gameOver)
        {
            return;
        }

        switch (currentState)
        {
            case GameState.lvl1:
                playerRb.velocity = new Vector3(lastMovement,
                                                playerRb.velocity.y, 0);
                positionLeft = new Vector3(transform.position.x - positionSidesRaycast,
                                    transform.position.y, transform.position.z);
                positionRight = new Vector3(transform.position.x + positionSidesRaycast,
                                            transform.position.y, transform.position.z);
                jumpForce = 10;

                break;

            case GameState.lvl2:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                -lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);

                break;

            case GameState.lvl3:
                playerRb.velocity = new Vector3(-lastMovement,
                                                playerRb.velocity.y, 0);
                positionLeft = new Vector3(transform.position.x + positionSidesRaycast,
                                            transform.position.y, transform.position.z);
                positionRight = new Vector3(transform.position.x - positionSidesRaycast,
                                            transform.position.y, transform.position.z);

                break;

            case GameState.lvl4:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);
                
                break;

            case GameState.lvl5:
                playerRb.velocity = new Vector3(0, playerRb.velocity.y,
                                                lastMovement);
                positionLeft = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z + positionSidesRaycast);
                positionRight = new Vector3(transform.position.x, transform.position.y,
                                            transform.position.z - positionSidesRaycast);
                jumpForce = 7;

                break;
        }

        
        
        Debug.DrawRay(positionLeft, -transform.up * jumpRaycastDistance, Color.red);
        Debug.DrawRay(positionRight, -transform.up * jumpRaycastDistance, Color.red);

        walking = false;
        
        if (Input.GetAxis(HORIZONTAL) >= 0.2 || Input.GetAxis(HORIZONTAL) <= -0.2)
        {
            walking = true;
            lastMovement = Input.GetAxis(HORIZONTAL) * runningSpeed;
        }

        if (!walking)
        {
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            sfx.paso.Stop();
        }

        //Si está en el aire
        if (!IsTouchingTheGround()){
            //Se le aplica una fuerza hacia abajo para que caiga
            playerRb.AddForce(transform.up * -gravity, ForceMode.Acceleration);
        }

        else{
            //Si está en el suelo se pone la velocidad en y a 0 para que
            //no se ralentice por el peso de la gravedad que traia al caer
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
            if (walking)
            {
                if (!sfx.paso.isPlaying)
                {
                    sfx.paso.Play();
                }
            }
        }

        animator.SetFloat(HORIZONTAL, Input.GetAxis(HORIZONTAL));
        animator.SetBool(WALKING_STATE, walking);
        animator.SetFloat(LAST_HORIZONTAL, lastMovement);
        animator.SetFloat(VERTICAL, playerRb.velocity.y);
        animator.SetBool(IS_TOUCHING_GROUND, IsTouchingTheGround());
    }

    public void Jump()
    {
        if (IsTouchingTheGround())
        {
            //ForceMode2D me dos opciones, force que seria como una 
            //fuerza constante e impulse que seria como aplicar una
            //fuerza en instante nada mas.
            
            playerRb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //GetComponent<AudioSource>().Play();
            sfx.salto.Play();
        }
    }

    bool IsTouchingTheGround()
    {
        if (Physics.Raycast(positionRight,
            -transform.up, jumpRaycastDistance, groundMask))
        {

            return true;
        }else
        if (Physics.Raycast(positionLeft,
            -transform.up, jumpRaycastDistance, groundMask))
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    public void Die()
    {
        sfx.muerte.Play();
        GameManager.sharedInstance.GameOver();
        sfx.paso.Stop();
        sfx.salto.Stop();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        lives[healthPoints].gameObject.SetActive(false);

        if (this.healthPoints > MAX_HEALTH)
        {
            healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <= 0)
        {
            Die();
        }
        else
        {
            sfx.herida.Play();
        }
        
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public void StopAllSFX()
    {
        sfx.salto.Stop();
        sfx.paso.Stop();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("initAnimation"))
        {
            GameManager.sharedInstance.NextLevel(((int)GameManager.sharedInstance.currentGameState) + 1);
            playerRb.freezeRotation = false;
            other.GetComponent<BoxCollider>().enabled = false;
            sfx.alerta.Play();
        }
        if (other.CompareTag("finishAnimation"))
        {
            playerRb.freezeRotation = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("finishAnimation"))
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (other.CompareTag("KillZone"))
        {
            Die();
        }
    }

}