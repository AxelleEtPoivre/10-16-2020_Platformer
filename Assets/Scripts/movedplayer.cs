
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public class movedplayer : MonoBehaviour
{

    private Inputs inputs;

    private void OnEnable() 
    {
        inputs = new Inputs();
        inputs.Enable();
        inputs.Player.Move.performed += OnMovedperformed;
    }


    private void OnMovedperformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Move fonctionne");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
