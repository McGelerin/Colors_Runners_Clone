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

        [SerializeField] private PlayerPhysicsController playerPhysicsController;
        //[SerializeField] private PlayerAnimationController animationController;
        //[SerializeField] private TextMeshPro scoreText;
        
        #endregion
        #endregion

        private void Awake()
        {
            SetStackPosition();
            Data = GetPlayerData();
            SendPlayerDataToControllers();
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
            InputSignals.Instance.onInputDragged += OnGetInputValues;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
            StackSignals.Instance.onBoostArea += OnJump;
            // ScoreSignals.Instance.onSetTotalScore += OnSetScoreText;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnActivateMovement;
            InputSignals.Instance.onInputReleased -= OnDeactiveMovement;
            InputSignals.Instance.onInputDragged -= OnGetInputValues;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
            StackSignals.Instance.onBoostArea -= OnJump;
           // ScoreSignals.Instance.onSetTotalScore -= OnSetScoreText;
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

        private void OnGetInputValues(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputValue(inputParams);
           
        }

        #endregion

        private void OnPlay()
        {
            SetStackPosition();
            movementController.IsReadyToPlay(true);
            //animationController.Playanim(PlayerAnimationStates.Run);
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
            playerPhysicsController.Jump(Data.MovementData.JumpForce);
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
    }
}
