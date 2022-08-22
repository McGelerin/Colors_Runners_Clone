using System.Collections;
using Commands;
using UnityEngine;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;

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
        [SerializeField] private PlayerAnimationController animationController;

        #endregion

        #region Private Variables

        private JumpCommand _jumpCommand;
        private SetPlayerPositionAfterDronePool _setPlayerPositionAfterDronePool;

        #endregion
        #endregion

        private void Awake()
        {
            Init();
            SetStackPosition();
            Data = GetPlayerData();
            SendPlayerDataToControllers();
            animationController.SetAnimState(CollectableAnimStates.Idle);
        }

        private void Init()
        {
            _jumpCommand = new JumpCommand(ref Data,transform);
            _setPlayerPositionAfterDronePool = new SetPlayerPositionAfterDronePool(transform);
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
            StackSignals.Instance.onBoostArea += _jumpCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += movementController.DeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone += movementController.UnDeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone += _setPlayerPositionAfterDronePool.Execute;
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
            StackSignals.Instance.onBoostArea -= _jumpCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= movementController.DeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone -= movementController.UnDeactiveForwardMovement;
            DronePoolSignals.Instance.onDroneGone -= _setPlayerPositionAfterDronePool.Execute;
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
            animationController.SetAnimState(CollectableAnimStates.Run);
        }

        private void OnLevelSuccessful()
        {
            movementController.IsReadyToPlay(false);

        }
        private void OnLevelFailed()
        {
            movementController.IsReadyToPlay(false);
        }

        private void SetStackPosition()
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
    }
}
