using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using TMPro;
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

        private int _currentLevel = 1;
        private int _currentScore = 0;
        private LevelData _levelsData;
        private List<LevelBuildingData> _levelBuildingDatas;
        private GameObject _planeGO;

        #endregion

        #endregion

        private void Awake()
        {
            _levelsData = GetIdleLevelBuildingData();
            GetCurrentLevelData();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            InstantiateLevelItems();
        }

        private LevelData GetIdleLevelBuildingData() => Resources.Load<CD_LevelBuildingData>("Data/CD_LevelBuildingData").Levels;

        private void GetCurrentLevelData()
        {
            foreach (var levelBuilding in _levelsData.LevelBuildings)
            {
                if (_currentLevel == levelBuilding.LevelNumber )
                {
                    _levelBuildingDatas = levelBuilding.LevelBuildingDatas;
                    _planeGO = levelBuilding.LevelPlane;
                }
            }
        }

        private void InstantiateLevelItems()
        {
            foreach (var levelBuildingData in _levelBuildingDatas)
            {
                CreateLevelBuildings(levelBuildingData);
            }
        }

        private void CreateLevelBuildings(LevelBuildingData levelBuildingData)
        {
            CreateLevelPlane();
            CreateMainBuilding(levelBuildingData);
            CreateSideBuilding(levelBuildingData);
        }

        private void CreateLevelPlane()
        {
            Instantiate(_planeGO, idleLevelHolder.position, Quaternion.identity, idleLevelHolder);
        }

        private void CreateMainBuilding(LevelBuildingData levelBuildingData)
        {
            var mainBuild = levelBuildingData.mainBuildingData.Building;
            var sideText = levelBuildingData.mainBuildingData.BuildingScoreTMP;
            var maxScore = levelBuildingData.mainBuildingData.MainBuildingScore;

            var position = idleLevelHolder.position;
            Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,
                Quaternion.identity,idleLevelHolder);
            Instantiate(sideText,position+ levelBuildingData.mainBuildingData.OffsetTMP,
                Quaternion.identity, idleLevelHolder);

            SetText(sideText, maxScore, _currentScore);

        }

        private void CreateSideBuilding(LevelBuildingData levelBuildingData)
        {
            foreach (var sideBuilding in levelBuildingData.sideBuildindData)
            {
                var sideBuild = sideBuilding.Building;
                var sideText = sideBuilding.BuildingScoreTMP;
                var maxScore = sideBuilding.SideBuildingScore;

                var position = idleLevelHolder.position;
                Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                Instantiate(sideText,position+ sideBuilding.InstantitatePos+ sideBuilding.OffsetTMP, Quaternion.identity,idleLevelHolder);

                SetText(sideText, maxScore, _currentScore);
            }
        }

        private void SetText(TextMeshPro text, int maxScore, int currentScore)
        {
            text.text = currentScore.ToString() + "/" + maxScore.ToString();
        }
    }
}