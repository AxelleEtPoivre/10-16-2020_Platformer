
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;


public class movedplayer : MonoBehaviour
{

    private Inputs inputs;
    private Vector2 direction;

    private void OnEnable() 
    {
        inputs = new Inputs();
        inputs.Enable();
        inputs.Player.Move.performed += OnMovedperformed;
    }


    private void OnMovedperformed(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>();
        Debug.Log(direction);
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
        myRigidBody.MovePosition(direction);
    }
}
