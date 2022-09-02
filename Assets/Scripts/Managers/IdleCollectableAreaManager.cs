using System;
using System.Collections.Generic;
using System.Text;
using Keys;
using Signals;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class IdleCollectableAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private List<GameObject> collectableSpawnPoint = new List<GameObject>();
        [SerializeField] private GameObject collectable;

        #endregion

        #region Private Variables

        private int _colectableScore;
        private List<GameObject> _collectablePool = new List<GameObject>();

        #endregion

        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onLoadIdleGame += LoadIdleDatas;
            IdleSignals.Instance.onColectableScore += OnGetIdleSaveDatas;
          //  SaveSignals.Instance.onSaveIdleParams += OnGetIdleSaveDatas;
          IdleSignals.Instance.onIdleCollectableValue += OnCollectableValue;
            IdleSignals.Instance.onCollectableAreaNextLevel += OnNextLevel;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onLoadIdleGame -= LoadIdleDatas;
            IdleSignals.Instance.onColectableScore -= OnGetIdleSaveDatas;
            IdleSignals.Instance.onIdleCollectableValue -= OnCollectableValue;
            //SaveSignals.Instance.onSaveIdleParams -= OnGetIdleSaveDatas;
            IdleSignals.Instance.onCollectableAreaNextLevel -= OnNextLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion


        private void Awake()
        {
            _colectableScore = 0;
            for (int i = 0; i < collectableSpawnPoint.Count; i++)
            {
                GameObject collectableCache = Instantiate(collectable, collectableSpawnPoint[i].transform.position, quaternion.identity,transform);
                collectableCache.SetActive(false);
                _collectablePool.Add(collectableCache);
            }
        }

        private void Start()
        {
            SaveSignals.Instance.onLoadIdle?.Invoke();
            CollectableStartOpen(_colectableScore);
        }

        private void LoadIdleDatas(SaveIdleDataParams saveIdleDataParams)
        {
            _colectableScore = saveIdleDataParams.CollectablesCount;
        }
        
        private void OnCollectableValue(int collectableCount)
        {
            if (_colectableScore+collectableCount <= _collectablePool.Count &&_colectableScore+collectableCount >=0)
            {
                _colectableScore += collectableCount;
            }
            else if (_colectableScore+collectableCount > _collectablePool.Count)
            {
                _colectableScore = _collectablePool.Count;
            }
            else if (_colectableScore+collectableCount < 0)
            {
                _colectableScore = 0;
            }
        }

        private void CollectableStartOpen(int collectableCount)
        {
            foreach (var VARIABLE in _collectablePool)
            {
                VARIABLE.SetActive(false);
            }

            for (int i = 0; i < collectableCount; i++)
            {
                _collectablePool[i].SetActive(true);
            }
        }
        
        private int OnGetIdleSaveDatas()
        {
            return _colectableScore;
        }
        
        private void OnNextLevel()
        {
            CollectableStartOpen(_colectableScore);
        }
    }
}