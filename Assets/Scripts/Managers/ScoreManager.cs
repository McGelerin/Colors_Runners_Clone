using Commands;
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

        [SerializeField] private GameObject stackGO;
        [SerializeField] private TextMeshPro scoreTMP, spriteTMP;
        [SerializeField] private GameObject textPlane;

        #endregion

        #region Private Variables

        private int _score;
        [ShowInInspector] private GameObject _playerGO;
        private GameStates _currentState = GameStates.Runner;
        private SetScoreCommand _setScoreCommand;
        private SetVisibilityOfScore _setVisibilityOfScore;
        private GameObject _parentGO;
        private bool _isActive = false;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            ScoreSignals.Instance.onSetScore?.Invoke(_score);
            GetReferences();
        }

        private void Init()
        {
            _setScoreCommand = new SetScoreCommand(ref _score);
            _setVisibilityOfScore = new SetVisibilityOfScore(ref scoreTMP, ref spriteTMP, ref textPlane);
        }

        private void GetReferences()
        {
            _parentGO = stackGO.transform.GetChild(0).gameObject;
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            ScoreSignals.Instance.onSetScore += _setScoreCommand.Execute;
            ScoreSignals.Instance.onVisibleScore += _setVisibilityOfScore.Execute;
            CoreGameSignals.Instance.onPlay += OnPlay;
            ScoreSignals.Instance.onSetLeadPosition += OnSetLead;
            LevelSignals.Instance.onRestartLevel += OnReset;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            ScoreSignals.Instance.onSetScore -= _setScoreCommand.Execute;
            ScoreSignals.Instance.onVisibleScore -= _setVisibilityOfScore.Execute;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            ScoreSignals.Instance.onSetLeadPosition -= OnSetLead;
            LevelSignals.Instance.onRestartLevel -= OnReset;

        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            SetScoreManagerRotation();
            if (_currentState == GameStates.Runner && _isActive)
            {
                SetScoreManagerPosition();
            }
        }

        private void OnPlay()
        {
            FindPlayerGameObject();
            GetReferences();

        }

        private void OnChangeGameState()
        {
            _currentState = GameStates.Idle;
            var transform1 = transform;
            transform1.parent = _playerGO.transform;
            transform1.localPosition = new Vector3(0, 2f, 0);
        }

        private void SetScoreManagerPosition()
        {
            transform.position = _parentGO.transform.position + new Vector3(0, 2f, 0);
        }

        private void SetScoreManagerRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z * -1f);
        }

        private void OnSetLead(GameObject gO)
        {
            _parentGO = gO;
        }

        private void FindPlayerGameObject()
        {
            _playerGO = GameObject.FindGameObjectWithTag("Player");
            _isActive = true;
        }

        private void OnReset()
        {
            _isActive = false;
        }
    }
}