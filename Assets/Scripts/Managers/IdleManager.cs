using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using JetBrains.Annotations;
using Keys;
using Signals;
using Sirenix.OdinInspector;
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

        private List<GameObject> _mainAreas=new List<GameObject>();
        [ShowInInspector]private List<List<GameObject>> _sideAreas = new List<List<GameObject>>();

        [ShowInInspector]private List<int> _mainCurrentScore = new List<int>();
        [ShowInInspector]private List<int> _sideCurrentScore = new List<int>();
        [ShowInInspector]private List<BuildingState> _mainBuildingState = new List<BuildingState>();
        [ShowInInspector]private List<BuildingState> _sideBuildingState = new List<BuildingState>();
        [ShowInInspector]private SaveIdleDataParams _saveIdleDataParams;

        private int _mainCache=0;
        private int _sideCache=0;

        private int _completeSides;
        
        //private List<GameObject> sideCache=new List<GameObject>();
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
            SaveSignals.Instance.onSaveIdleParams += OnGetIdleSaveDatas;
            SaveSignals.Instance.onLoadIdleGame += LoadIdleDatas;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            IdleSignals.Instance.onMainSideComplete += OnMainSideComplete;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onSaveIdleParams -= OnGetIdleSaveDatas;
            SaveSignals.Instance.onLoadIdleGame -= LoadIdleDatas;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            IdleSignals.Instance.onMainSideComplete -= OnMainSideComplete;
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
            EmptyListChack();
            InstantiateLevelItems();
        }

        private void LoadIdleDatas(SaveIdleDataParams saveIdleDataParams)
        {
            _saveIdleDataParams = saveIdleDataParams;
            _idleLevel = _saveIdleDataParams.IdleLevel % _levelsData.LevelBuildings.Count;
            _mainCurrentScore = _saveIdleDataParams.MainCurrentScore;
            _sideCurrentScore = _saveIdleDataParams.SideCurrentScore;
            _mainBuildingState = _saveIdleDataParams.MainBuildingState;
            _sideBuildingState = _saveIdleDataParams.SideBuildingState;
        }

        private LevelData GetIdleLevelBuildingData() => Resources.Load<CD_LevelBuildingData>("Data/CD_IdleLevelBuild").Levels;

        private void GetCurrentLevelData()
        {
            _levelBuildingDatas = _levelsData.LevelBuildings[_idleLevel].LevelBuildingDatas;
            _planeGO = _levelsData.LevelBuildings[_idleLevel].LevelPlane;
        }

        private void EmptyListChack()
        {
            if (_mainCurrentScore.IsNullOrEmpty())
            {
                Debug.Log("Null geldi");
                foreach (var levelBuildingDatas in _levelBuildingDatas)
                {
                    Debug.Log("Foreach dondü");
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
            CreateSideBuilding(levelBuildingData);
            CreateMainBuilding(levelBuildingData);
        }
        
        private void CreateMainBuilding(LevelBuildingData levelBuildingData)
        {
            var mainBuild = levelBuildingData.mainBuildingData.Building;
            var buildingPrice = levelBuildingData.mainBuildingData.MainBuildingScore;
            var position = idleLevelHolder.position;
            GameObject obj= Instantiate(mainBuild,position+ levelBuildingData.mainBuildingData.InstantitatePos,mainBuild.transform.rotation,idleLevelHolder);
            _mainAreas.Add(obj);
            MainSetReferance(obj, buildingPrice);
        }

        private void MainSetReferance(GameObject mainBuild, int buildingPrice)
        {
            mainBuild.GetComponent<IdleAreaManager>().SetBuildRef(_mainCache,true, buildingPrice,_mainCurrentScore[_mainCache],_mainBuildingState[_mainCache],this);
        }

        private void CreateSideBuilding(LevelBuildingData levelBuildingData)
        {
            List<GameObject> sideCache = new List<GameObject>();
            foreach (var sideBuilding in levelBuildingData.sideBuildindData)
            {
                var sideBuild = sideBuilding.Building;
                var buildingPrice = sideBuilding.SideBuildingScore;
                var position = idleLevelHolder.position;
                GameObject obj = Instantiate(sideBuild,position+ sideBuilding.InstantitatePos, Quaternion.identity,idleLevelHolder);
                sideCache.Add(obj);
                SideSetReferance(obj, buildingPrice);
                _sideCache++;
            }
            _sideAreas.Add(sideCache);
        }
        private void SideSetReferance(GameObject sideBuild, int buildingPrice)
        {
            sideBuild.GetComponent<IdleAreaManager>().SetBuildRef(_sideCache,false ,buildingPrice,_sideCurrentScore[_sideCache],_sideBuildingState[_sideCache],this);
        }

        private void OnMainSideComplete(int MainID)
        {
            Debug.Log(MainID);
            Debug.Log(_sideAreas[MainID][0]);
            foreach (var VARIABLE in _sideAreas[MainID])
            {
                VARIABLE.GetComponent<IdleAreaManager>().MainSideComplete();
            }
        }
        
        private void SaveParametres()
        {
            _isMainSide = true;
            foreach (var VARIABLE in _mainAreas)
            {
                VARIABLE.GetComponent<IdleAreaManager>().SendReftoIdleManager();
            }
            _isMainSide = false;
            foreach (var VARIABLE in _sideAreas.SelectMany(side => side))
            {
                VARIABLE.GetComponent<IdleAreaManager>().SendReftoIdleManager();
            }
        }


        public void SetSaveDatas(int areaID,int currentScore,BuildingState buildingState)
        {
            if (_isMainSide)
            {
                _mainCurrentScore[areaID] = currentScore;
                _mainBuildingState[areaID] = buildingState;
            }
            else
            {
                _sideCurrentScore[areaID]=currentScore;
                _sideBuildingState[areaID]=buildingState;
            }
        }

        private SaveIdleDataParams OnGetIdleSaveDatas()
        {
            return new SaveIdleDataParams()
            {
                IdleLevel = _idleLevel,
                CollectablesCount = IdleSignals.Instance.onColectableScore(),
                MainCurrentScore = _mainCurrentScore,
                SideCurrentScore = _sideCurrentScore,
                MainBuildingState = _mainBuildingState,
                SideBuildingState = _sideBuildingState
            };
        }

        private void NextIdleLevelChack()
        {
            foreach (var VARIABLE in _mainBuildingState)
            {
                if (VARIABLE == BuildingState.Completed)
                {
                    _completeSides++;
                }
            }

            foreach (var VARIABLE in _sideBuildingState)
            {
                if (VARIABLE == BuildingState.Completed)
                {
                    _completeSides++;
                }
            }
        }

        private void OnClearActiveLevel()
        {
            _levelClearer.ClearActiveLevel(idleLevelHolder.transform);
        }
        private void OnNextLevel()
        {
            SaveParametres();
            NextIdleLevelChack();
            IdleSignals.Instance.onIdleCollectableValue?.Invoke(_completeSides);
            
            if ((_mainCache+_sideCache) - _completeSides <= 0)
            {
                _idleLevel++;
                _mainAreas.Clear();
                _sideAreas.Clear();
                _mainCurrentScore.Clear();
                _sideCurrentScore.Clear();
                _mainBuildingState.Clear();
                _sideBuildingState.Clear();
                _mainCache=0;
                _sideCache=0;
                SaveSignals.Instance.onIdleSaveData?.Invoke();
                OnClearActiveLevel();
                _levelsData = GetIdleLevelBuildingData();
                Init();
            }else SaveSignals.Instance.onIdleSaveData?.Invoke();
            IdleSignals.Instance.onCollectableAreaNextLevel?.Invoke();
            _completeSides = 0;
        }
    }
}