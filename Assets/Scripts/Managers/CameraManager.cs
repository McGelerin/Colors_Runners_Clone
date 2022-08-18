using System;
using Cinemachine;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public CameraStates CameraStateController
        {
            get => _cameraStateValue;
            set
            {
                _cameraStateValue = value;
                SetCameraStates();
            }
        }
        
        #endregion
        #region Serialized Variables
        [SerializeField]private CinemachineVirtualCamera virtualCamera;


        #endregion

        #region Private Variables
        
        private Vector3 _initialPosition;
        private CameraStates _cameraStateValue = CameraStates.InitializeCam;
        private Animator _camAnimator;
        
        #endregion

        #endregion

        private void Awake()
        {
            virtualCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            _camAnimator = GetComponent<Animator>();
            GetInitialPosition();
        }
        
        #region Event Subscriptions
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
//            CoreGameSignals.Instance.onIdleStart += OnIdleCam;
            CoreGameSignals.Instance.onPlay += OnSetCameraTarget;
            CoreGameSignals.Instance.onReset += OnReset;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void UnsubscribeEvents()
        {
//            CoreGameSignals.Instance.onIdleStart -= OnIdleCam;
            CoreGameSignals.Instance.onPlay -= OnSetCameraTarget;
            CoreGameSignals.Instance.onReset -= OnReset;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void SetCameraStates()
        {
            if (CameraStateController == CameraStates.InitializeCam)
            {
                _camAnimator.Play(CameraStateController.ToString());
            }
            else if (CameraStateController == CameraStates.RunnerCam)
            {
                _camAnimator.Play(CameraStateController.ToString());
            }
        }
        
        private void GetInitialPosition()
        {
            _initialPosition = virtualCamera.transform.localPosition;
        }

        private void OnMoveToInitialPosition()
        {
            virtualCamera.transform.localPosition = _initialPosition;
        }

        private void OnSetCameraTarget()
        {
            var playerManager = FindObjectOfType<PlayerManager>().transform;
            virtualCamera.Follow = playerManager;
            CameraStateController = CameraStates.RunnerCam;
        }
        
        private void OnNextLevel()
        {
            CameraStateController = CameraStates.InitializeCam;
        }

        private void OnReset()
        {
            CameraStateController = CameraStates.InitializeCam;
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
            virtualCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            OnMoveToInitialPosition();
        }
    }
}