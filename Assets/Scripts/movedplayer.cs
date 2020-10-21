
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;


public class movedplayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    private Inputs inputs;
    private Vector2 direction;
   
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private SpriteRenderer myRenderer;


    private void OnEnable() 
    {
        inputs = new Inputs();
        inputs.Enable();
        inputs.Player.Move.performed += OnMovedPerformed;
        inputs.Player.Move.canceled += OnMoveCanceled;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        direction = Vector2.zero;
    }


    private void OnMovedPerformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Animator>().SetBool("isWalking", true); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction.y = 0;
        if (myRigidbody.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody.AddForce(direction * speed);
        }
        var isWalking = direction.x != 0;
        myAnimator.SetBool("isWalking", isWalking);
        if (direction.x < 0)
        {
            myRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            myRenderer.flipX = false;
        }
    }
}
