using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using JetBrains.Annotations;
using Signals;
using Sirenix.Utilities;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class IdleManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        

        #endregion

        #region Serialized Variables

        [SerializeField] private Transform idleLevelHolder;

        #endregion

        #region Private Variables

        private int _idleLevel;
        private int _currentLevel;
        private int _currentScore;
        private LevelData _levelsData;
        private List<LevelBuildingData> _levelBuildingDatas;
        private GameObject _planeGO;

        private List<int> _mainCurrentScore;
        private List<int> _sideCurrentScore;
        private List<BuildingState> _mainBuildingState;
        private List<BuildingState> _sideBuildingState;

        int _mainCache=0;
        int _sideCache=0;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Init()
        {   _levelsData = GetIdleLevelBuildingData();
            LoadIdleDatas();
            GetCurrentLevelData();
            EmptyListChack();
            InstantiateLevelItems();
        }


        private void LoadIdleDatas()
        {
            _idleLevel = LoadLevelData() % _levelsData.LevelBuildings.Count;
            _mainCurrentScore = SaveSignals.Instance.onIdleLoad.Invoke().MainCurrentScore;
            _sideCurrentScore = SaveSignals.Instance.onIdleLoad.Invoke().SideCurrentScore;
            _mainBuildingState = SaveSignals.Instance.onIdleLoad.Invoke().MainBuildingState;
            _sideBuildingState = SaveSignals.Instance.onIdleLoad.Invoke().SideBuildingState;
        }

        private LevelData GetIdleLevelBuildingData() => Resources.Load<CD_LevelBuildingData>("Data/CD_IdleLevelBuild").Levels;

        private int LoadLevelData()
        {
            return SaveSignals.Instance.onIdleLoad.Invoke().IdleLevel;
        }

        private void GetCurrentLevelData()
        {
            _levelBuildingDatas = _levelsData.LevelBuildings[_idleLevel].LevelBuildingDatas;
            _planeGO = _levelsData.LevelBuildings[_idleLevel].LevelPlane;
        }

        private void EmptyListChack()
        {
            if (_mainCurrentScore.IsNullOrEmpty())
            {
                _mainCurrentScore = new List<int>();
                _sideCurrentScore = new List<int>();
                _mainBuildingState = new List<BuildingState>();
                _sideBuildingState = new List<BuildingState>();
                foreach (var levelBuildingDatas in _levelBuildingDatas)
                {
                    _mainCurrentScore.Add(0);
                    _mainBuildingState.Add(BuildingState.Uncompleted);
                    foreach (var VARIABLE in levelBuildingDatas.sideBuildindData)
                    {
                        _sideCurrentScore.Add(0);
                        _sideBuildingState.Add(BuildingState.Uncompleted);
                    }
                }
            }
        }
        
        private void InstantiateLevelItems()
        {
            CreateLevelPlane();
            foreach (var levelBuildingData in _levelBuildingDatas)
            {
                CreateLevelBuildings(levelBuildingData);
                _mainCache++;
            }
        }

        private void CreateLevelPlane()
        {
            Instantiate(_planeGO, idleLevelHolder.position,_planeGO.transform.rotation, idleLevelHolder);
        }
        
        private void CreateLevelBuildings(LevelBuildingData levelBuildingData)
        {
            CreateMainBuilding(levelBuildingData);
            CreateSideBuilding(levelBuildingData);
        }
        
        private void CreateMainBuilding(LevelBuildingData levelBuildingData)
        {
            var mainBuild = levelBuildingData.mainBuildingData.Building;
            var buildingPrice = levelBuildingData.mainBuildingData.MainBuildingScore;
            MainSetReferance(mainBuild, buildingPrice);

            var position = idleLevelHolder.position;
            Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,mainBuild.transform.rotation,idleLevelHolder);
        }

        private void MainSetReferance(GameObject mainBuild, int buildingPrice)
        {
            if (_mainCurrentScore != null)
            {
                mainBuild.GetComponent<IdleAreaManager>().SetBuildRef(_mainCache, buildingPrice,_mainCurrentScore[_mainCache]);
            }
        }

        private void CreateSideBuilding(LevelBuildingData levelBuildingData)
        {
            foreach (var sideBuilding in levelBuildingData.sideBuildindData)
            {
                var sideBuild = sideBuilding.Building;
                var buildingPrice = sideBuilding.SideBuildingScore;
                SideSetReferance(sideBuild, buildingPrice);
                var position = idleLevelHolder.position;
                Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                _sideCache++;
            }
        }
        private void SideSetReferance(GameObject sideBuild, int buildingPrice)
        {
            sideBuild.GetComponent<IdleAreaManager>().SetBuildRef(_sideCache, buildingPrice,_sideCurrentScore[_sideCache]);
        }
    }
}