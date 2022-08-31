using System;
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
                    meshController.ChangeBuildingGradient(1.5f);
                    tmp.gameObject.SetActive(false);
                }
                else if (BuildState == BuildingState.Uncompleted )
                {
                    if (_isTextOpen)
                    {
                        tmp.gameObject.SetActive(true);
                        meshController.ChangeBuildingGradient(
                            Mathf.Clamp((_currentScore/(float)_buildingPrice*2),0,1.5f));
                    }

                }
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                if (CurrentScore == _buildingPrice)
                {
                    BuildState = BuildingState.Completed;
                }
                else
                {
                    meshController.ChangeBuildingGradient(
                        Mathf.Clamp((_currentScore/(float)_buildingPrice*2),0,1.5f));
                }
            }
        }
        
        
        #endregion
        #region Serializefield Variables

        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private IdleAreaMeshController meshController;
        
        #endregion

        #region Private Variables

        [ShowInInspector]private int _buildId;
        [ShowInInspector] private bool _isTextOpen;
        [ShowInInspector]private int _buildingPrice;
        [ShowInInspector]private int _currentScore;
        [ShowInInspector]private BuildingState _buildingState;
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

        }

        private void UnsubscribeEvents()
        {

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
        
        public void SetBuildRef(int buildID,bool isTextOpen,int buildingPrice,int currentPrice,BuildingState buildingState,IdleManager idleManager)
        {
            _buildId = buildID;
            _isTextOpen = isTextOpen;
            _buildingPrice = buildingPrice;
            _currentScore = currentPrice;
            BuildState = buildingState;
            Debug.Log(_buildingState.ToString());
            _idleManager = idleManager;
        }

        public void SendReftoIdleManager()
        {
            _idleManager.SetSaveDatas(_buildId,CurrentScore,BuildState);
        }

        private void SetText()
        {
            tmp.text = _currentScore.ToString() + "/" + _buildingPrice.ToString();
        }
    }
}