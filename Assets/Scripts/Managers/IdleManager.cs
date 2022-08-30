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

        [CanBeNull] private List<int> _mainCurrentScore;
        [CanBeNull] private List<int> _sideCurrentScore;
        [CanBeNull] private List<BuildingState> _mainBuildingState;
        [CanBeNull] private List<BuildingState> _sideBuildingState;

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
            // foreach (var levelBuilding in _levelsData.LevelBuildings)
            // {
            //     if (_currentLevel == levelBuilding.LevelNumber )
            //     {
            //         _levelBuildingDatas = levelBuilding.LevelBuildingDatas;
            //         _planeGO = levelBuilding.LevelPlane;
            //     }
            // }
            _levelBuildingDatas = _levelsData.LevelBuildings[_idleLevel].LevelBuildingDatas;
            _planeGO = _levelsData.LevelBuildings[_idleLevel].LevelPlane;
        }

        private void EmptyListChack()
        {
            if (_mainCurrentScore.IsNullOrEmpty())
            {
                foreach (var levelBuildingDatas in _levelBuildingDatas)
                {
                    if (_mainCurrentScore != null) _mainCurrentScore.Add(0);
                    Debug.LogFormat("fore");

                    if (_mainBuildingState != null) _mainBuildingState.Add(BuildingState.Uncompleted);
                    foreach (var VARIABLE in levelBuildingDatas.sideBuildindData)
                    {
                        if (_sideCurrentScore != null) _sideCurrentScore.Add(0);
                        if (_sideBuildingState != null) _sideBuildingState.Add(BuildingState.Uncompleted);
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
            //var sideText = levelBuildingData.mainBuildingData.BuildingScoreTMP;
            var buildingPrice = levelBuildingData.mainBuildingData.MainBuildingScore;
            MainSetReferance(mainBuild, buildingPrice);

            var position = idleLevelHolder.position;
            Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,mainBuild.transform.rotation,idleLevelHolder);
            // Instantiate(sideText,position+ levelBuildingData.mainBuildingData.OffsetTMP,
            //     Quaternion.identity, idleLevelHolder);

//            SetText(sideText, maxScore, _currentScore);

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
                //var sideText = sideBuilding.BuildingScoreTMP;
                var buildingPrice = sideBuilding.SideBuildingScore;
                SideSetReferance(sideBuild, buildingPrice);
                var position = idleLevelHolder.position;
                Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                // Instantiate(sideText,position+ sideBuilding.InstantitatePos+ sideBuilding.OffsetTMP, Quaternion.identity,idleLevelHolder);
                //
                // SetText(sideText, maxScore, _currentScore);
                _sideCache++;
            }
        }

        private void SideSetReferance(GameObject sideBuild, int buildingPrice)
        {
            if (_sideCurrentScore[_mainCache]!=0)
            {
                sideBuild.GetComponent<IdleAreaManager>().SetBuildRef(_sideCache, buildingPrice,_sideCurrentScore[_sideCache]);
            }
        }


        // private void SetText(TextMeshPro text, int maxScore, int currentScore)
        // {
        //     text.text = currentScore.ToString() + "/" + maxScore.ToString();
        // }
    }
}