using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject bullet;

    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private Vector2 stickDirection;
    private bool isOnGround = false;
    private bool isFacingLeft = true;

    private void OnEnable()
    {
        var playerController = new PlayerController();
        playerController.Enable();
        playerController.Main.Move.performed += MoveOnPerformed;
        playerController.Main.Move.canceled += MoveOnCanceled;
        playerController.Main.Jump.performed += JumpOnPerformed;
        playerController.Main.Shoot.performed += ShootOnperformed;

    }

    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        if (isOnGround)
        {
            myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
        }

    }

    private void MoveOnPerformed(InputAction.CallbackContext obj)
    {
        stickDirection = obj.ReadValue<Vector2>();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    {
        stickDirection = Vector2.zero;
    }


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };

        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }

        var isWalking = isOnGround && Mathf.Abs(myRigidbody2D.velocity.x) > 0.1f;
        myAnimator.SetBool("isWalking", isWalking);

        var isJumping = !isOnGround && myRigidbody2D.velocity.y != 0;
        myAnimator.SetBool("isJumping", isJumping);

        Flip();

    }

    private void Flip()
    {
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
        var touchingGround = ground == (ground | (1 << other.gameObject.layer));

        var touchFromAbove = other.contacts[0].normal.y > other.contacts[0].normal.x;

        if (touchingGround && touchFromAbove)
        {
            isOnGround = true;
        }
    }
}
