using System.Collections.Generic;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Managers
{
    public class IdleLevelManager : MonoBehaviour
    {
        #region Self Variables
        #region Serialized Variables
        [SerializeField] private Transform idleLevelHolder;
        #endregion
        #region Private Variables
        private int _idleLevel;
        private LevelData _levelsData;
        private List<LevelBuildingData> _levelBuildingDatas;
        private GameObject _planeGO;
        [ShowInInspector]private SaveIdleDataParams _saveIdleDataParams;
        private int _completeSides;
        private bool _isMainSide;
        private ClearActiveLevelCommand _levelClearer;
        #endregion
        #endregion

        private void Awake()
        {
            _levelsData = GetIdleLevelBuildingData();
            _levelClearer = new ClearActiveLevelCommand();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onLoadIdleGame += LoadIdleDatas;
            SaveSignals.Instance.onIdleLevel += OnIdleLevel;
            IdleSignals.Instance.onNextIdleLevel += OnNextIdleLevel;
            IdleSignals.Instance.onClearIdle += OnClearActiveLevel;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onLoadIdleGame -= LoadIdleDatas;
            SaveSignals.Instance.onIdleLevel -= OnIdleLevel;
            IdleSignals.Instance.onNextIdleLevel -= OnNextIdleLevel;
            IdleSignals.Instance.onClearIdle -= OnClearActiveLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            SaveSignals.Instance.onLoadIdle?.Invoke();
            GetCurrentLevelData();
            InstantiateLevelItems();
        }
        
        private void LoadIdleDatas(SaveIdleDataParams saveIdleDataParams)
        {
            _saveIdleDataParams = saveIdleDataParams;
            _idleLevel = _saveIdleDataParams.IdleLevel % _levelsData.LevelBuildings.Count;
        }

        private LevelData GetIdleLevelBuildingData() => Resources.Load<CD_LevelBuildingData>("Data/CD_IdleLevelBuild").Levels;
        
        private void GetCurrentLevelData()
        {
            _levelBuildingDatas = _levelsData.LevelBuildings[_idleLevel].LevelBuildingDatas;
            _planeGO = _levelsData.LevelBuildings[_idleLevel].LevelPlane;
        }
        
        private void InstantiateLevelItems()
        {
            CreateLevelPlane();
            foreach (var levelBuildingData in _levelBuildingDatas)
            {
                CreateLevelBuildings(levelBuildingData);
            }
        }

        private void CreateLevelPlane()
        {
            Instantiate(_planeGO, idleLevelHolder.position,_planeGO.transform.rotation, idleLevelHolder);
        }
        
        private void CreateLevelBuildings(LevelBuildingData levelBuildingData)
        {
            CreateSideBuilding(levelBuildingData);
            CreateMainBuilding(levelBuildingData);
        }
        
        private void CreateMainBuilding(LevelBuildingData levelBuildingData)
        {
            var mainBuild = levelBuildingData.mainBuildingData.Building;
            var buildingPrice = levelBuildingData.mainBuildingData.MainBuildingScore;
            var position = idleLevelHolder.position;
            GameObject obj= Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,mainBuild.transform.rotation,idleLevelHolder);
            IdleSignals.Instance.onMainSideObjects?.Invoke(obj,buildingPrice);
        }

        private void CreateSideBuilding(LevelBuildingData levelBuildingData)
        {
            List<GameObject> sideCache = new List<GameObject>();
            List<int> sidePriceCache = new List<int>();
            foreach (var sideBuilding in levelBuildingData.sideBuildindData)
            {
                var sideBuild = sideBuilding.Building;
                var buildingPrice = sideBuilding.SideBuildingScore;
                var position = idleLevelHolder.position;
                GameObject obj = Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                sideCache.Add(obj);
                sidePriceCache.Add(buildingPrice);
            }
            IdleSignals.Instance.onSideObjects?.Invoke(sideCache,sidePriceCache);
        }

        private void OnClearActiveLevel()
        {
            _levelClearer.ClearActiveLevel(idleLevelHolder.transform);
            Init();
        }

        private int OnIdleLevel()
        {
            return _idleLevel;
        }
        
        private void OnNextIdleLevel()
        {
            _idleLevel++;
        }
    }
}