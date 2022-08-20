using System;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Keys;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        //[SerializeField] private PlayerManager manager;
        [SerializeField] private Rigidbody rigidbody;
        
        #endregion
        
        #region Private Variables
        
        [Header("Data")] private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay, _isOnDronePool = false;
        private float _inputValue;
        private float _inputValueX;
        private float _inputValueZ;
        private Vector2 _clampValues;
        private bool _isRunner = true;
        
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
        public void DeactiveForwardMovement(Transform poolTriggerTransform)
        {
            _isOnDronePool = true;
        }

        public void UnDeactiveForwardMovement(Transform truePoolTransform)
        {
            _isOnDronePool = false;
        }

        public void UpdateRunnerInputValue(RunnerInputParams inputParam)
        {
            _inputValue = inputParam.XValue;
            _clampValues = inputParam.ClampValues;
        }

        public void UpdateIdleInputValue(IdleInputParams inputParams)
        {
            _inputValueX = inputParams.ValueX;
            _inputValueZ = inputParams.ValueZ;
        }

        public void ChangeGameState()
        {
            _isRunner = false;
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }

        // private void Update() //Degisebilir
        // {
        //     if (_isReadyToPlay)
        //     {
        //         manager.SetStackPosition();
        //
        //     }
        // }
    
        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isOnDronePool)
                {
                    OnlySideways();
                }
                else if (_isReadyToMove)
                {
                    Move();
                }
                else
                {
                    StopPlayer();
                }
            }
            else
                Stop();
        }

        private void Move()
        {
            if (_isRunner)
            {
                RunnerMove();
            }
            else
            {
                IdleMove();
            }
        }

        private void StopPlayer()
        {
            if (_isRunner)
            {
                StopSideways();
            }
            else
            {
                Stop();
            }
        }

        private void RunnerMove()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _movementData.SidewaysSpeed, velocity.y,
                _movementData.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(Mathf.Clamp(rigidbody.position.x, _clampValues.x,
                    _clampValues.y), (position = rigidbody.position).y, position.z);
            rigidbody.position = position;

            var direction = Vector3.forward + Vector3.right * Mathf.Clamp(_inputValue,
                -_movementData.RotateBorder,_movementData.RotateBorder);
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = toRotation;
        }

        private void IdleMove()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValueX * _movementData.ForwardSpeed, velocity.y,
                _inputValueZ*_movementData.ForwardSpeed);
            rigidbody.velocity = velocity;

            var position1 = rigidbody.position;
            var position = new Vector3(position1.x, position1.y, position1.z);
            position1 = position;
            rigidbody.position = position1;
            if (velocity != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(velocity, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,
                                _movementData.IdleRotateSpeed*Time.fixedDeltaTime);
            }
            
        }

        private void StopSideways()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _movementData.ForwardSpeed);
            //rigidbody.angularVelocity = Vector3.zero;
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        private void OnlySideways()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _movementData.SidewaysSpeed, velocity.y,
                0);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(
                Mathf.Clamp(rigidbody.position.x, _clampValues.x,
                    _clampValues.y),
                (position = rigidbody.position).y,
                position.z);
            rigidbody.position = position;
        }
        
        public void Jump(float distance,float duration)
        {
            transform.DOMoveY(distance, duration).SetEase(Ease.InOutCubic).SetAutoKill();
        }
        
        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }

        public void SetPlayerPositionToTrueDronePool(Transform _dronePoolTransform)
        {
            transform.position = new Vector3(_dronePoolTransform.position.x, transform.position.y, transform.position.z + 15);
        }
    }
}