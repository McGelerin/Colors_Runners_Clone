using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using TMPro;

namespace Managers
{
    public class IdleAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public BuildingState BuildState
        {
            get => _buildingState;
            set
            {
                _buildingState = value;

                if (BuildState == BuildingState.Completed)
                {
                    if (_isMain)
                    {
                        IdleSignals.Instance.onMainSideComplete?.Invoke(_buildId);
                    }
                    meshController.ChangeBuildingGradient(1.5f);
                    tmp.gameObject.SetActive(false);
                    StopAllCoroutines();
                    IdleSignals.Instance.onIteractionBuild?.Invoke(false, transform);

                }
                else if (BuildState == BuildingState.Uncompleted)
                {
                    if (_isTextOpen)
                    {
                        tmp.gameObject.SetActive(true);
                    }
                    meshController.ChangeBuildingGradient(
                        Mathf.Clamp((_currentScore / (float)_buildingPrice * 2), 0, 1.5f));
                }
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;

                if (_currentScore == _buildingPrice)
                {
                    IdleSignals.Instance.onIteractionBuild?.Invoke(false, transform);
                    BuildState = BuildingState.Completed;
                }
                else
                {
                    SetText();
                    meshController.ChangeBuildingGradient(
                        Mathf.Clamp((_currentScore / (float)_buildingPrice * 2), 0, 1.5f));
                }
            }
        }


        #endregion
        #region Serializefield Variables

        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private IdleAreaMeshController meshController;

        #endregion

        #region Private Variables

        [ShowInInspector] private int _buildId;
        [ShowInInspector] private bool _isMain;
        [ShowInInspector] private bool _isTextOpen;
        [ShowInInspector] private int _buildingPrice;
        [ShowInInspector] private int _currentScore;
        [ShowInInspector] private BuildingState _buildingState;
        private IdleManager _idleManager;

        #endregion

        #endregion

        private void Awake()
        {
            tmp.gameObject.SetActive(false);
        }


        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            //IdleSignals.Instance.onScoreAdd += OnScoreAdd;
        }

        private void UnsubscribeEvents()
        {
            // IdleSignals.Instance.onScoreAdd -= OnScoreAdd;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion


        private void Start()
        {
            SetText();
        }

        public void SetBuildRef(int buildID,bool isMain, int buildingPrice, int currentPrice, BuildingState buildingState, IdleManager idleManager)
        {
            _buildId = buildID;
            _isMain = isMain;
            _isTextOpen = _isMain;
            _buildingPrice = buildingPrice;
            _currentScore = currentPrice;
            BuildState = buildingState;
            _idleManager = idleManager;
            
        }

        public void SendReftoIdleManager()
        {
            _idleManager.SetSaveDatas(_buildId, CurrentScore, BuildState);
        }

        private void SetText()
        {
            tmp.text = _currentScore.ToString() + "/" + _buildingPrice.ToString();
        }

        public void MainSideComplete()
        {
            _isTextOpen = true;
            BuildState = _buildingState;
        }
        
        public void ScoreAdd(bool interactionPlayer)
        {
            if (interactionPlayer)
            {
                int score = ScoreSignals.Instance.onGetIdleScore();
                if (score > 0)
                {
                    IdleSignals.Instance.onIteractionBuild?.Invoke(true, tmp.transform);
                    StartCoroutine(StayCondition(score));
                }
            }
            else
            {
                IdleSignals.Instance.onIteractionBuild?.Invoke(false, transform);
                StopAllCoroutines();
            }
        }
        
        private IEnumerator StayCondition(int score)
        {
            if (score > 0)
            {
                ScoreSignals.Instance.onSetScore?.Invoke(-1);
                CurrentScore++;
                yield return new WaitForSeconds(0.1f);
                score = ScoreSignals.Instance.onGetIdleScore();

                if (CurrentScore <= _buildingPrice)
                {
                    StartCoroutine(StayCondition(score));
                }
            }
            else
            {
                IdleSignals.Instance.onIteractionBuild(false, transform);
            }
        }
    }
}