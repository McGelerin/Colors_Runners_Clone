using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Keys;
using Signals;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region EventSubscribtion
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onRunnerSaveData += OnRunnerSaveData;
            SaveSignals.Instance.onIdleSaveData += OnIdleSaveData;
          //  SaveSignals.Instance.onLoadIdleGame += OnIdleGameLoad;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onRunnerSaveData -= OnRunnerSaveData;
            SaveSignals.Instance.onIdleSaveData -= OnIdleSaveData;
          //  SaveSignals.Instance.onLoadIdleGame -= OnIdleGameLoad;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        } 
        #endregion

        private void Awake()
        {
            OnIdleGameLoad();
        }


        private void OnRunnerSaveData()
        {
            RunnerSaveGame(
                new SaveRunnerDataParams()
                {
                   // Money = SaveSignals.Instance.onGetMoney(),
                    Level = SaveSignals.Instance.onGetRunnerLevelID(),
                }
            );
        }

        private void RunnerSaveGame(SaveRunnerDataParams saveDataParams)
        {
            if (saveDataParams.Level != null) ES3.Save("Level", saveDataParams.Level);
            //if (saveDataParams.Money != null) ES3.Save("Money", saveDataParams.Money);
        }

        private void OnIdleSaveData()
        {
            IdleSaveGame(new SaveIdleDataParams()
            {
                IdleLevel = SaveSignals.Instance.onSaveIdleParams().IdleLevel,
                CollectablesCount = SaveSignals.Instance.onSaveIdleParams().CollectablesCount,
                MainCurrentScore = SaveSignals.Instance.onSaveIdleParams().MainCurrentScore,
                SideCurrentScore = SaveSignals.Instance.onSaveIdleParams().SideCurrentScore,
                MainBuildingState = SaveSignals.Instance.onSaveIdleParams().MainBuildingState,
                SideBuildingState = SaveSignals.Instance.onSaveIdleParams().SideBuildingState
            });
        }
        
        private void IdleSaveGame(SaveIdleDataParams saveIdleDataParams)
        {
            if (saveIdleDataParams.IdleLevel != 0) ES3.Save("IdleLevel", saveIdleDataParams.IdleLevel,"IdleGame.es3");
            if (saveIdleDataParams.CollectablesCount != 0) ES3.Save("CollectablesCount", saveIdleDataParams.CollectablesCount,"IdleGame.es3");
            if (saveIdleDataParams.MainCurrentScore != null) ES3.Save("MainCurrentScore", saveIdleDataParams.MainCurrentScore,"IdleGame.es3");
            if (saveIdleDataParams.SideCurrentScore != null) ES3.Save("SideCurrentScore", saveIdleDataParams.SideCurrentScore,"IdleGame.es3");
            if (saveIdleDataParams.MainBuildingState != null) ES3.Save("MainBuildingState", saveIdleDataParams.MainBuildingState,"IdleGame.es3");
            if (saveIdleDataParams.SideBuildingState != null) ES3.Save("SideBuildingState", saveIdleDataParams.SideBuildingState,"IdleGame.es3");
        }

        
        private void OnIdleGameLoad()
        {
            SaveSignals.Instance.onLoadIdleGame?.Invoke(new SaveIdleDataParams()
                {
                    IdleLevel = ES3.KeyExists("IdleGame","Idlegame.es3")? ES3.Load<int>("IdleGame","IdleGame.es3"):0,
                    CollectablesCount = ES3.KeyExists("CollectablesCount","Idlegame.es3")? ES3.Load<int>("CollectablesCount","IdleGame.es3"):0,
                    MainCurrentScore = ES3.KeyExists("MainCurrentScore","Idlegame.es3")? ES3.Load<List<int>>("MainCurrentScore","IdleGame.es3"):default,
                    SideCurrentScore = ES3.KeyExists("SideCurrentScore","Idlegame.es3")? ES3.Load<List<int>>("SideCurrentScore","IdleGame.es3"):default,
                    MainBuildingState = ES3.KeyExists("MainBuildingState","Idlegame.es3")? ES3.Load<List<BuildingState>>("MainBuildingState","IdleGame.es3"):default,
                    SideBuildingState = ES3.KeyExists("SideBuildingState","Idlegame.es3")? ES3.Load<List<BuildingState>>("SideBuildingState","IdleGame.es3"):default,
                }
            );
        }
    }
}