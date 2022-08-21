using System;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private GameObject playerGO;

        #endregion

        #region Private Variables

        private int _score;
        [ShowInInspector] private GameObject _stackGO;
        private InputStates _currentState;

        #endregion

        #endregion

        private void Awake()
        {
            _stackGO = GameObject.Find("StackManager");
        }

        private void Start()
        {
            _score = _stackGO.transform.childCount;
            
        }
        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z * -1f);
            if (_currentState == InputStates.OldInputSystem)
            {
                transform.position = _stackGO.transform.GetChild(0).position + new Vector3(0, 2f, 0);
            }
        }

        private void OnChangeGameState()
        {
            _currentState = InputStates.NewInputSystem;
            transform.parent = playerGO.transform;
            transform.localPosition = new Vector3(0, 2f, 0);
        }
    }
}