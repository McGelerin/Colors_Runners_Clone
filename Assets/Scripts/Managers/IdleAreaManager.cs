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
//                    tmp.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else if (BuildState == BuildingState.Uncompleted)
                {
                    Debug.Log("satate girdi");
                    meshController.ChangeBuildingGradient(
                        Mathf.Clamp((_currentScore/(float)_buildingPrice),0,1.5f));
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
        [ShowInInspector]private int _buildingPrice;
        [SerializeField]private int _currentScore;
        [ShowInInspector]private BuildingState _buildingState;
        private IdleManager _idleManager;
        
        #endregion

        #endregion

        private void Start()
        {
            //BuildState = _buildingState;
            //Debug.Log(BuildState.ToString());

        }

        public void SetBuildRef(int buildID,int buildingPrice,int currentPrice,BuildingState buildingState,IdleManager idleManager)
        {
            _buildId = buildID;
            _buildingPrice = buildingPrice;
            _currentScore = currentPrice;
            BuildState = buildingState;
            Debug.Log(_buildingState.ToString());
            _idleManager = idleManager;
        }

        public void SendReftoIdleManager()
        {
            _idleManager.SetSaveDatas(_buildId,_currentScore,_buildingState);
        }

        private void SetText()
        {
            tmp.text = _currentScore.ToString() + "/" + _buildingPrice.ToString();
        }
    }
}