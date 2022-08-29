using System;
using System.Collections.Generic;
using UnityEngine;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using Unity.Mathematics;

namespace MCScripts
{
    public class IdleLevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

       // [Header("Data")] public CD_LevelBuildingData Data;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject IdlelevelHolder;

        #endregion

        #region Private Variables

        private int _newLevelData;
        private List<int> deneme = new List<int>();
        [SerializeField]private List<int> deneme2 = new List<int>();

        private int _IdlelevelID;

        #endregion

        #endregion

        private void Awake()
        {
           // _newLevelData = GetLevelCount();
            //Data = GetLevelBuildingData();
            deneme.Add(20);
            deneme.Add(30);
            deneme.Add(40);


            //_IdlelevelClearer = new ClearActiveLevelCommand();
            //_IdlelevelLoader = new LevelLoaderCommand();
        }
        
        //private CD_LevelBuildingData GetLevelBuildingData() => Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel").Levels[_newLevelData];
        
        
        // private int GetLevelCount()
        // {
        //     return _IdlelevelID % Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel").Levels.Count;
        // }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onGetRunnerLevelID += OnGetLevelID;
            SaveSignals.Instance.onLoadIdleGame += OnLoadIdleGame;
            SaveSignals.Instance.onSaveIdleParams += OnGetCurrentScore;
            
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onGetRunnerLevelID -= OnGetLevelID;
            SaveSignals.Instance.onLoadIdleGame -= OnLoadIdleGame;
            SaveSignals.Instance.onSaveIdleParams -= OnGetCurrentScore;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        

        private void OnLoadIdleGame(SaveIdleDataParams saveIdleDataParams)
        {
            deneme2 = saveIdleDataParams.SideCurrentScore;
        }

        private int OnGetLevelID()
        {
            return 0;
        }
        
        private void OnInitializeLevel()
        {

           // _IdlelevelLoader.InitializeLevel(newLevelData, IdlelevelHolder.transform);
     //      InitializeLevel(IdlelevelHolder.transform);
        }

        public SaveIdleDataParams OnGetCurrentScore()
        {
            return new SaveIdleDataParams()
            {
                SideCurrentScore = deneme
            };
        }



        private void OnClearActiveLevel()
        {
           // _IdlelevelClearer.ClearActiveLevel(IdlelevelHolder.transform);
        }
        
        
        // public void InitializeLevel(Transform levelHolder)//command yapacaz
        // {
        //     for (int i = 0; i < Data.LevelBuildingDatas.Count; i++)
        //     {
        //         Instantiate(Data.LevelBuildingDatas[i].mainBuildingData.Building, 
        //             new Vector3(
        //                 levelHolder.position.x + Data.LevelBuildingDatas[i].mainBuildingData.InstantitatePos.x,
        //                 0,
        //                 levelHolder.position.z + Data.LevelBuildingDatas[i].mainBuildingData.InstantitatePos.y),Quaternion.identity,levelHolder);
        //
        //         for (int j = 0; j < Data.LevelBuildingDatas[i].sideBuildindData.Count; j++)
        //         {
        //             Instantiate(Data.LevelBuildingDatas[i].sideBuildindData[j].Building, 
        //                 new Vector3(
        //                     levelHolder.position.x + Data.LevelBuildingDatas[i].sideBuildindData[j].InstantitatePos.x,
        //                     0,
        //                     levelHolder.position.z + Data.LevelBuildingDatas[i].sideBuildindData[j].InstantitatePos.y),
        //                 Quaternion.identity,levelHolder);
        //         }
        //     }
            //Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {_levelID}"), levelHolder);
        //}
    }
}