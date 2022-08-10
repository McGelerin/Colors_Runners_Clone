using Data.ValueObject;
using Keys;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables
        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private Rigidbody rigidbody;
        #endregion
        #region Private Variables
        [Header("Data")] private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay;
        private float _inputValue;
        private Vector2 _clampValues; 
        #endregion
        #endregion

        public void SetMovementData(PlayerMovementData dataMovementData)
        {   
            _movementData = dataMovementData;
        }

        public void EnableMovement()
        {
            _isReadyToMove = true;
        }

        public void DeactiveMovement()
        {
            _isReadyToMove = false;
        }

        public void UpdateInputValue(HorizontalInputParams inputParam)
        {
            _inputValue = inputParam.XValue;
            _clampValues = inputParam.ClampValues;
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }

        private void Update() //Degisebilir
        {
            if (_isReadyToPlay)
            {
                manager.SetStackPosition();
        
            }
        }
    
        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                
                if (_isReadyToMove)
                {
                    Move();
                    
                }
                else
                {
                    StopSideways();
                }
                
            }
            else
                Stop();
        }

        private void Move()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _movementData.SidewaysSpeed, velocity.y,
                _movementData.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(
                Mathf.Clamp(rigidbody.position.x, _clampValues.x,
                    _clampValues.y),
                (position = rigidbody.position).y,
                position.z);
            rigidbody.position = position;
        }

        private void StopSideways()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _movementData.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }
    }
}