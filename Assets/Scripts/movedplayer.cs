
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

    private void OnEnable() 
    {
        inputs = new Inputs();
        inputs.Enable();
        inputs.Player.Move.performed += OnMovedPerformed;
        inputs.Player.Move.canceled += OnMoveCanceled;
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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var myRigidBody = GetComponent<Rigidbody2D>();
        direction.y = 0;
        if (myRigidBody.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidBody.AddForce(direction * speed);
        }
        
    }
}
