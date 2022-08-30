using System;
using Controllers;
using UnityEngine;
using Enums;
using Signals;
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
                    tmp.gameObject.transform.parent.gameObject.SetActive(false);
                }
                
            }
        }

        #endregion
        #region Serializefield Variables

        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private bool isMainSide;
        [SerializeField] private IdleAreaMeshController meshController;
        
        #endregion

        #region Private Variables

        private int _buildId;
        private int _buildingPrice;
        private int _currentScore;
        private BuildingState _buildingState;
        
        #endregion

        #endregion
        
        private void SetLoadReferance()//burası hata veriyor yarın save managerdan düzeltilmesi lazım
        {
            if (isMainSide)
            {

            //    _currentScore = SaveSignals.Instance.onIdleLoad().MainCurrentScore[_buildId]; 
            //    _buildingState = SaveSignals.Instance.onIdleLoad().MainBuildingState[_buildId];

            }
            else
            {
            //    _currentScore = SaveSignals.Instance.onIdleLoad().SideCurrentScore[_buildId];
             //  _buildingState = SaveSignals.Instance.onIdleLoad().SideBuildingState[_buildId];
            }
        }

        public void SetBuildRef(int buildID,int buildingPrice)
        {
            _buildId = buildID;
            _buildingPrice = buildingPrice;
            SetLoadReferance();
        }
        
        
        private void SetText()
        {
            tmp.text = _currentScore.ToString() + "/" + _buildingPrice.ToString();
        }
    }
}