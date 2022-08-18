using System.Collections;
using UnityEngine;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using TMPro;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public PlayerData Data;

        #endregion

        #region Serialized Variables

        [Space] [SerializeField] private PlayerMovementController movementController;

        //[SerializeField] private PlayerPhysicsController playerPhysicsController;
        [SerializeField] private PlayerAnimationController animationController;
        //[SerializeField] private TextMeshPro scoreText;

        #endregion
        #region private vars
        private Transform _dronePoolTransform;
        #endregion
        #endregion

        private void Awake()
        {
            SetStackPosition();
            Data = GetPlayerData();
            SendPlayerDataToControllers();
            animationController.SetAnimState(CollectableAnimStates.Idle);
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(Data.MovementData);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += OnActivateMovement;
            InputSignals.Instance.onInputReleased += OnDeactiveMovement;
            InputSignals.Instance.onRunnerInputDragged += OnGetRunnerInputValues;
            InputSignals.Instance.onJoystickDragged += OnGetIdleInputValues;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
            StackSignals.Instance.onBoostArea += OnJump;
            // ScoreSignals.Instance.onSetTotalScore += OnSetScoreText;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += movementController.DeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone += movementController.UnDeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone += OnDroneGone;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnActivateMovement;
            InputSignals.Instance.onInputReleased -= OnDeactiveMovement;
            InputSignals.Instance.onRunnerInputDragged -= OnGetRunnerInputValues;
            InputSignals.Instance.onJoystickDragged -= OnGetIdleInputValues;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
            StackSignals.Instance.onBoostArea -= OnJump;
            // ScoreSignals.Instance.onSetTotalScore -= OnSetScoreText;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= movementController.DeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone -= movementController.UnDeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone -= OnDroneGone;


        }


        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Movement Controller

        private void OnActivateMovement()
        {
            movementController.EnableMovement();
            
        }

        private void OnDeactiveMovement()
        {
            movementController.DeactiveMovement();
        }

        private void OnGetRunnerInputValues(RunnerInputParams inputParams)
        {
            movementController.UpdateRunnerInputValue(inputParams);

        }
        
        private void OnGetIdleInputValues(IdleInputParams inputParams)
        {
            movementController.UpdateIdleInputValue(inputParams);
        }

        private void OnChangeGameState()
        {
            movementController.ChangeGameState();
        }

        #endregion

        private void OnPlay()
        {
            SetStackPosition();
            movementController.IsReadyToPlay(true);
            animationController.SetAnimState(CollectableAnimStates.Runner);
        }

        private void OnLevelSuccessful()
        {
            movementController.IsReadyToPlay(false);

        }
        private void OnLevelFailed()
        {
            movementController.IsReadyToPlay(false);
        }

        public void SetStackPosition()
        {
            StackSignals.Instance.onPlayerGameObject?.Invoke(gameObject);
        }

        private void OnReset()
        {
            gameObject.SetActive(true);
            movementController.OnReset();
            SetStackPosition();
            //animationController.OnReset();
        }

        private void OnJump()
        {
            //playerPhysicsController.Jump(Data.MovementData.JumpForce);
            //rigidbody.AddForce(0,jumpForce,0,ForceMode.Impulse);
            movementController.Jump(Data.MovementData.JumpDistance,Data.MovementData.JumpDuration);
        }
        
        //private void OnSetScoreText(int Values)
        //{
        //    scoreText.text = Values.ToString();
        //}


        IEnumerator WaitForFinal()
        {
            //animationController.Playanim(animationStates:PlayerAnimationStates.Idle);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
           // CoreGameSignals.Instance.onMiniGameStart?.Invoke();
        }

        private void OnDroneGone()
        {
            //Transform target = DronePoolSignals.Instance.onGetTruePoolTransform(); ->Kullan�labilir ancak e�er sahnede birden fazla drone k�sm� varsa d�nen de�erlerde sorun olabilir.
            transform.position = new Vector3(_dronePoolTransform.position.x, transform.position.y, transform.position.z + 15);
        }

        public void GetDronePoolTransform(Transform dronePoolTransform)
        {
            _dronePoolTransform = dronePoolTransform;
            Transform target = DronePoolSignals.Instance.onGetTruePoolTransform();
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z + 15);
            animationController.SetAnimState(CollectableAnimStates.Runner);

        }
    }

    
}
