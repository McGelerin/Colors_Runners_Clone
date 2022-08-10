using System;
using System.Collections;
using Cinemachine;
using Controllers;
using Signals;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        //[SerializeField] private Animator animator;
        //[SerializeField] private CinemachineVirtualCamera PreStartCam;
        [SerializeField] private CinemachineVirtualCamera InGameCam;
        //[SerializeField] private CinemachineVirtualCamera EndGameCam;
        //[SerializeField] private CinemachineVirtualCamera EndGameSideCam;

        #endregion

        #region Private Variables

        private Vector3 _preStartCamPos;
        private Vector3 _inGameCamPos;
        private Vector3 _endGameCamPos;
        private Vector3 _endGameSideCamPos;
        
        
        #endregion

        #endregion

        private void Start()
        {
            //_preStartCamPos = PreStartCam.transform.position;
            _inGameCamPos = InGameCam.transform.position;
            //_endGameCamPos = EndGameCam.transform.position;
            //_endGameSideCamPos = EndGameSideCam.transform.position;

        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            //CoreGameSignals.Instance.onLevelInitialize += OnLevelInitiliaze;
            CoreGameSignals.Instance.onPlay += OnPlay;
            //CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            //CoreGameSignals.Instance.onCameraInitialized += OnCameraInitialized;
            //CoreGameSignals.Instance.onGameEnd += OnEndGameSideCamera;
        }

        private void UnsubscribeEvents()
        {
            //CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitiliaze;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            //CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            //CoreGameSignals.Instance.onCameraInitialized -= OnCameraInitialized;
            //CoreGameSignals.Instance.onGameEnd -= OnEndGameSideCamera;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void OnLevelInitiliaze()
        {
            //animator.Play("InGameCam");
            SetAllCameraToTarget();
        }
        
        private void OnCameraInitialized()
        {
            //PreStartCam.transform.position = _preStartCamPos;
            InGameCam.transform.position = _inGameCamPos;
            //EndGameCam.transform.position = _endGameCamPos;
            //EndGameSideCam.transform.position = _endGameSideCamPos;

            SetAllCameraToTarget();
        }

        private void SetAllCameraToTarget()
        {
            //SetCameraTargetToPlayer(PreStartCam);
            SetCameraTargetToPlayer(InGameCam);
            //var playerFinish = FindObjectOfType<MiniGameManager>().transform.GetChild(0).transform;
            //EndGameCam.Follow = playerFinish;
            //EndGameSideCam.Follow = playerFinish;

            #region SORULACAK
            /*
           EndGameCam.Follow = GameObject.FindWithTag("PlayerFinish").transform;
           EndGameCam.Follow = GameObject.Find("PlayerFinish").transform;*/
            #endregion
        }
        
        private void OnPlay()
        {
            //animator.Play("InGameCam");
            OnLevelInitiliaze();

        }

        private void SetCameraTargetToPlayer(CinemachineVirtualCamera Camera)
        {
            Camera.Follow = GameObject.FindObjectOfType<PlayerManager>().transform;
        }
        
        private void OnLevelSuccessful()
        {
            StartCoroutine(EndGameCamera(1));
        }
        private IEnumerator EndGameCamera(float delay)
        {
            yield return new WaitForSeconds(delay);
            //animator.Play("EndGameCam");
        }

        private void OnEndGameSideCamera()
        {
            //animator.Play("EndGameSideCam");
        }
    }
}