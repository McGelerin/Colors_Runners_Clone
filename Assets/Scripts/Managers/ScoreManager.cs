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
        [SerializeField] private TextMeshPro scoreTMP,spriteTMP;
        [SerializeField] private GameObject textPlane;

        #endregion

        #region Private Variables

        private int _score;
        [ShowInInspector] private GameObject _playerGO;
        private GameStates _currentState;
        private SetScoreCommand _setScoreCommand;
        private SetVisibilityOfScore _setVisibilityOfScore;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            GetReferences();
            ScoreSignals.Instance.onSetScore?.Invoke(_score);
        }

        private void Init()
        {
            _setScoreCommand = new SetScoreCommand(ref _score);
            _setVisibilityOfScore = new SetVisibilityOfScore(ref scoreTMP, ref spriteTMP, ref textPlane);
        }

        private void GetReferences()
        {
            _score = stackGO.transform.childCount;
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
            ScoreSignals.Instance.onSetScoreManagerPosition += OnSetScoreManagerPosition;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            ScoreSignals.Instance.onSetScore -= _setScoreCommand.Execute;
            ScoreSignals.Instance.onVisibleScore -= _setVisibilityOfScore.Execute;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            ScoreSignals.Instance.onSetScoreManagerPosition -= OnSetScoreManagerPosition;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            SetScoreManagerRotation();
        }
        
        private void OnPlay()
        {
            _playerGO = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnChangeGameState()
        {
            _currentState = GameStates.Idle;
            var transform1 = transform;
            transform1.parent = _playerGO.transform;
            transform1.localPosition = new Vector3(0, 2f, 0);
        }

        private void OnSetScoreManagerPosition()
        {
            if (_currentState == GameStates.Runner)
            {
                var transform1 = transform;
                transform1.parent = stackGO.transform.GetChild(0).transform;
                transform1.localPosition = new Vector3(0, 2f, 0);
            }
        }

        private void SetScoreManagerRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z * -1f);
        }
    }
}