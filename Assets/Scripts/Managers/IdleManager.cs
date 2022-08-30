using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
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
        
        int _mainCache=1;
        int _sideCache=1;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
            Debug.Log(_idleLevel);
        }

        private void Init()
        {   _levelsData = GetIdleLevelBuildingData();
            _idleLevel = LoadLevelData() % _levelsData.LevelBuildings.Count;
            GetCurrentLevelData();
            InstantiateLevelItems();
        }

        private LevelData GetIdleLevelBuildingData() => Resources.Load<CD_LevelBuildingData>("Data/CD_IdleLevelBuildTest").Levels;

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
            var sideText = levelBuildingData.mainBuildingData.BuildingScoreTMP;
            var buildingPrice = levelBuildingData.mainBuildingData.MainBuildingScore;
            mainBuild.GetComponent<IdleAreaManager>().SetBuildRef(_mainCache,buildingPrice);

            var position = idleLevelHolder.position;
            Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,mainBuild.transform.rotation,idleLevelHolder);
            // Instantiate(sideText,position+ levelBuildingData.mainBuildingData.OffsetTMP,
            //     Quaternion.identity, idleLevelHolder);

//            SetText(sideText, maxScore, _currentScore);

        }

        private void CreateSideBuilding(LevelBuildingData levelBuildingData)
        {
            foreach (var sideBuilding in levelBuildingData.sideBuildindData)
            {
                var sideBuild = sideBuilding.Building;
                //var sideText = sideBuilding.BuildingScoreTMP;
                var buildingPrice = sideBuilding.SideBuildingScore;
                sideBuild.GetComponent<IdleAreaManager>().SetBuildRef(_sideCache,buildingPrice);
                var position = idleLevelHolder.position;
                Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                // Instantiate(sideText,position+ sideBuilding.InstantitatePos+ sideBuilding.OffsetTMP, Quaternion.identity,idleLevelHolder);
                //
                // SetText(sideText, maxScore, _currentScore);
                _sideCache++;
            }
        }

        // private void SetText(TextMeshPro text, int maxScore, int currentScore)
        // {
        //     text.text = currentScore.ToString() + "/" + maxScore.ToString();
        // }
    }
}