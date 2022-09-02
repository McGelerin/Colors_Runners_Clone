using System.Collections.Generic;
using Enums;
using UnityEngine;
using Keys;
using Signals;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private int _idleLevel, _collectablesCount;
        private List<int> _mainCurrentScore,
            _sideCurrentScore = new List<int>();
        private List<BuildingState> _mainBuildingState=new List<BuildingState>(), 
            _sideBuildingState = new List<BuildingState>();

        private SaveIdleDataParams _saveParameters;
        
        #endregion

        #endregion
        
        private void Awake()
        {
            SetLoadIdleGameData();
        }
        
        #region EventSubscribtion
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onRunnerSaveData += OnRunnerSaveData;
            SaveSignals.Instance.onIdleSaveData += OnIdleSaveData;
            SaveSignals.Instance.onLoadIdle += OnIdleGameLoad;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onRunnerSaveData -= OnRunnerSaveData;
            SaveSignals.Instance.onIdleSaveData -= OnIdleSaveData;
            SaveSignals.Instance.onLoadIdle -= OnIdleGameLoad;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        } 
        #endregion

        #region Event Methods

        private void OnRunnerSaveData()
        {
            RunnerSaveGame(
                new SaveRunnerDataParams()
                {
                    Level = SaveSignals.Instance.onGetRunnerLevelID(),
                    Collectable =  ScoreSignals.Instance.onGetIdleScore()
                }
            );
        }
        private void OnIdleSaveData()
        {
            _saveParameters = SaveSignals.Instance.onSaveIdleParams();
            IdleSaveGame(new SaveIdleDataParams()
            {
                IdleLevel = _saveParameters.IdleLevel,
                CollectablesCount = _saveParameters.CollectablesCount,
                MainCurrentScore = _saveParameters.MainCurrentScore,
                SideCurrentScore = _saveParameters.SideCurrentScore,
                MainBuildingState = _saveParameters.MainBuildingState,
                SideBuildingState = _saveParameters.SideBuildingState
            });
        }
        private void OnIdleGameLoad()
        {
            SaveSignals.Instance.onLoadIdleGame?.Invoke(new SaveIdleDataParams()
                {
                    IdleLevel = _idleLevel,
                    CollectablesCount = _collectablesCount,
                    MainCurrentScore = _mainCurrentScore,
                    SideCurrentScore = _sideCurrentScore,
                    MainBuildingState = _mainBuildingState,
                    SideBuildingState = _sideBuildingState
                }
            );
        }

        #endregion
        private void RunnerSaveGame(SaveRunnerDataParams saveDataParams)
        {
            if (saveDataParams.Level != 0) ES3.Save("Level", saveDataParams.Level);
            if (saveDataParams.Collectable != 0) ES3.Save("Collectable", saveDataParams.Collectable);
        }
        
        private void IdleSaveGame(SaveIdleDataParams saveIdleDataParams)
        {
            if (saveIdleDataParams.IdleLevel != 0) ES3.Save("IdleLevel", saveIdleDataParams.IdleLevel,"IdleGame.es3");
            if (saveIdleDataParams.CollectablesCount != 0) ES3.Save("CollectablesCount", saveIdleDataParams.CollectablesCount,"IdleGame.es3");
            if (saveIdleDataParams.MainCurrentScore != default) ES3.Save("MainCurrentScore", saveIdleDataParams.MainCurrentScore,"IdleGame.es3");
            if (saveIdleDataParams.SideCurrentScore != default) ES3.Save("SideCurrentScore", saveIdleDataParams.SideCurrentScore,"IdleGame.es3");
            if (saveIdleDataParams.MainBuildingState != default) ES3.Save("MainBuildingState", saveIdleDataParams.MainBuildingState,"IdleGame.es3");
            if (saveIdleDataParams.SideBuildingState != default) ES3.Save("SideBuildingState", saveIdleDataParams.SideBuildingState,"IdleGame.es3");
        }

        private void SetLoadIdleGameData()
        {
            _idleLevel = ES3.KeyExists("IdleGame", "IdleGame.es3") 
                ? ES3.Load<int>("IdleGame", "IdleGame.es3") 
                : 0;
            _collectablesCount = ES3.KeyExists("CollectablesCount", "IdleGame.es3")
                ? ES3.Load<int>("CollectablesCount", "IdleGame.es3")
                : 0;
            _mainCurrentScore = ES3.KeyExists("MainCurrentScore", "IdleGame.es3")
                ? new List<int>(ES3.Load<List<int>>("MainCurrentScore", "IdleGame.es3"))
                : new List<int>();
            _sideCurrentScore = ES3.KeyExists("SideCurrentScore", "IdleGame.es3")
                ? new List<int>(ES3.Load<List<int>>("SideCurrentScore", "IdleGame.es3"))
                : new List<int>();
            _mainBuildingState = ES3.KeyExists("MainBuildingState", "IdleGame.es3")
                ? new List<BuildingState>(ES3.Load<List<BuildingState>>("MainBuildingState", "IdleGame.es3"))
                : new List<BuildingState>();
            _sideBuildingState = ES3.KeyExists("SideBuildingState", "IdleGame.es3")
                ? new List<BuildingState>(ES3.Load<List<BuildingState>>("SideBuildingState", "IdleGame.es3"))
                : new List<BuildingState>();
        }
    }
}