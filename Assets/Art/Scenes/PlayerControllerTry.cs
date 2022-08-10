using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTry : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private CharacterController controller;

    #endregion

    #region Private Variables

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private float _playerSpeed = 4.0f;
    private float _jumpHeight = 1.0f;
    private float _gravityValue = -9.81f;

    #endregion

    #endregion

    void Update()
    {
        _groundedPlayer = controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
    
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(move * Time.deltaTime * _playerSpeed);
    
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
    
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }
}
