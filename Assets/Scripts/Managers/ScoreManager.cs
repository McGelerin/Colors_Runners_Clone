using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private GameObject stackGO;

        #endregion

        #region Private Variables

        private int _score;
        [ShowInInspector] private GameObject _playerGO;
        private InputStates _currentState;

        #endregion

        #endregion

        private void Awake()
        {
            _playerGO = GameObject.Find("PlayerManager v2");
        }

        private void Start()
        {
            _score = stackGO.transform.childCount;
            ScoreSignals.Instance.onSetScore?.Invoke(_score);
        }
        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            ScoreSignals.Instance.onSetScore += OnSetScore;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            ScoreSignals.Instance.onSetScore -= OnSetScore;
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
                transform.position = stackGO.transform.GetChild(0).position + new Vector3(0, 2f, 0);
            }
        }

        private void OnChangeGameState()
        {
            _currentState = InputStates.NewInputSystem;
            transform.parent = _playerGO.transform;
            transform.localPosition = new Vector3(0, 2f, 0);
        }

        private void OnSetScore(int value)
        {
            _score += value;
            UISignals.Instance.onSetScoreText?.Invoke(_score);
        }
    }
}