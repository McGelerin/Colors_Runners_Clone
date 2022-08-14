using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTry : MonoBehaviour
{
    #region Self Variables
    
    #region Serialized Variables
    
    private Vector3 _playerVelocity;
    private float _playerSpeed = 4.0f;
    
    #endregion
    
    #region Private Variables
    
    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private CharacterController controller;
    
    #endregion
    
    #endregion
    
    void Update()
    {
        Debug.Log("new input system");
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(move * Time.fixedDeltaTime * _playerSpeed);
        
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
            
        controller.Move(_playerVelocity * Time.fixedDeltaTime);
    }
}
