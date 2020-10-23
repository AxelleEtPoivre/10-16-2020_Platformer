using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject bullet;

    private Rigidbody2D myRigidbody2D;
    //this variable refers to a RigidBody2D Component;
    private Animator myAnimator;
    //this variable refers to an Animator Component;
    private SpriteRenderer myRenderer;
    //this variable refers to a SpriteRenderer Component;

    private Vector2 stickDirection;
    private bool isOnGround = false;
    //this bool will verify if the player is on the ground;
    private bool isFacingLeft = true;
    //this bool will check which direction the Player's facing;

    public static bool GameIsPaused = false;
    //public : accessible from other scripts;
    //static : we just want to check if the game is currently paused, we don't want to reference this specific PauseMenu script;
    //bool : true or false :: Is the game Paused or Not?;
    //false by default, our game isn't paused by default;

    private PlayerController playerController;

    public GameObject pauseMenuUI;
    //this GameObject will reffer to our PauseMenu Canvas;

    private void OnEnable()
    {
        playerController = new PlayerController();
        //we're setting up each control from our InputSystem;
        playerController.Enable();
        playerController.Main.Move.performed += MoveOnPerformed;
        playerController.Main.Move.canceled += MoveOnCanceled;
        playerController.Main.Jump.performed += JumpOnPerformed;
        playerController.Main.Shoot.performed += ShootOnperformed;
        playerController.Enable();
        playerController.Main.Pause.performed += PauseOnPerformed;

    }

    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        if (GameIsPaused)
        {
            Resume();
            //Pressing the Pause Key (here "Esc") when the game is already paused will automatically Resume it;
        }
        else
        {
            Pause();
            //Pressing the Pause Key when the game is running will Pause the game;
        }
    }


    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        //this function will be executed when the "Shoot" button is pressed (here "E");
        Instantiate(bullet, transform.position, Quaternion.identity);
        //pressing the following button will create a "bullet" GameObject in our scene;
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        //this function will be executed when the "Jump" button is pressed (here SPACEBAR);
        if (isOnGround)
        //we're checking if the Player's touching the ground in order to prevent it for infinite jumping;
        {
            myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //this is the jump, we can change the force of the jump in the inspector since it's a serialized variable;
            isOnGround = false;
            // since the Player is jumping(and not touching the ground anymore), we set up the "isOnGround" boolean to false;
            //is it on ground ? NO -> False;
        }

    }

    private void MoveOnPerformed(InputAction.CallbackContext obj)
        //this function will be executed when the arrows keys/or the joystick will be used;
    {
        stickDirection = obj.ReadValue<Vector2>();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    //this function will be executed as soon as the arrows keys/the joystick will be released;
    {
        stickDirection = Vector2.zero;
        //the direction is set to 0 which means the Player isn't moving anymore;
    }


    void Start()
    {
        //we assign each Component to it's variable;
        //we're taking them from the GameObject which holds the script;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };

        //we set up the movement by specifying the Player will go faster till it reaches its maximum speed;
        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }

        //the following lines are controlling the animator;
        var isWalking = isOnGround && Mathf.Abs(myRigidbody2D.velocity.x) > 0.1f;
        myAnimator.SetBool("isWalking", isWalking);

        var isJumping = !isOnGround && myRigidbody2D.velocity.y != 0;
        myAnimator.SetBool("isJumping", isJumping);

        Flip();
        //we're calling the Flip(); function so our Player's Sprite will change depending on the direction it's moving towards;

    }

    private void Flip()
    {
        //this function will use the player's position to flip the Sprite accordingly;
        //then, when going left, the sprite will flip so the Player will face left as well.
        if (stickDirection.x < -0.1f)
        {
            isFacingLeft = true;
        }

        if (stickDirection.x > 0.1f)
        {
            isFacingLeft = false;
        }
        myRenderer.flipX = isFacingLeft;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //we're checking if the Player lands on a GameObject being on the "Ground" layer; 
        var touchingGround = ground == (ground | (1 << other.gameObject.layer));

        //we're cheking if the Player lands on a horizontal surface;
        var touchFromAbove = other.contacts[0].normal.y > other.contacts[0].normal.x;

        //if the Player lands on a "Ground" layer which has an horizontal surface,
        //the boolean will be set to true;
        if (touchingGround && touchFromAbove)
        {
            isOnGround = true;
        }
    }

    public void Resume()
    {
        //we want take away PauseMenu;
        //we want to set time back to normal;
        //we want to put GameIsPaused to false;
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        //We set the timespeed as its default value;

        GameIsPaused = false;
    }

    void Pause()
    //we want to bring up PauseMenu;
    //we want to freeze time in our game;
    //we want to put GameIsPaused to true;
    {
        pauseMenuUI.SetActive(true);
        //enables our Game Object, here the whole PauseMenu Canvas;

        Time.timeScale = 0f;
        //Time.timeScale = the speed at which time is passing;
        //it can be used to create slowmotion effects;
        //we set to 0 in order to freeze the game;

        GameIsPaused = true;
    }

    public void MainMenu()
    {
        //this function will allow us to open the Menu scene;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        playerController.Disable();
    }
}
