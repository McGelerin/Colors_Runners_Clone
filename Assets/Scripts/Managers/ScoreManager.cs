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
        private OpenScoreText _openScoreText;

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
            _openScoreText = new OpenScoreText(ref scoreTMP, ref spriteTMP,ref textPlane);
        }

        private void GetReferences()
        {
            _playerGO = GameObject.Find("PlayerManager v2");
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
            DronePoolSignals.Instance.onDronePoolExit += _openScoreText.Execute;
            
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            ScoreSignals.Instance.onSetScore -= _setScoreCommand.Execute;
            DronePoolSignals.Instance.onDronePoolExit -= _openScoreText.Execute;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            SetScoreManagerRotation();
            SetScoreManagerPosition();
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
            if (_currentState == GameStates.Runner)
            {
                transform.position = stackGO.transform.GetChild(0).position + new Vector3(0, 2f, 0);
            }
        }

        private void SetScoreManagerRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z * -1f);
        }

        // private void OnGunPoolExit()
        // {
        //     scoreTMP.GetComponent<MeshRenderer>().enabled = true;
        //     spriteTMP.GetComponent<MeshRenderer>().enabled = true;
        //     textPlane.GetComponent<MeshRenderer>().enabled = true;
        // }
    }
}