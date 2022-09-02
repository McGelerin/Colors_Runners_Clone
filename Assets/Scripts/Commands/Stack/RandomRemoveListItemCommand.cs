using System.Collections.Generic;
using Managers;
using UnityEngine;
using Signals;

namespace Commands
{
    public class RandomRemoveListItemCommand
    {
        #region Self Variables
        #region Private Variables
        private List<GameObject> _collectableStack;
        private GameObject _levelHolder;
        #endregion
        #endregion
        
        public RandomRemoveListItemCommand(ref List<GameObject> CollectableStack,ref GameObject levelHolder)
        {
            _collectableStack = CollectableStack;
            _levelHolder = levelHolder;
        }

        public void Execute()
        {
            if (_collectableStack.Count < 1)
            {
                return;
            }
            int random = Random.Range(1, _collectableStack.Count);
            GameObject selectedCollectable = _collectableStack[random - 1];
            selectedCollectable.transform.SetParent(_levelHolder.transform.GetChild(0));
            selectedCollectable.SetActive(false);
            _collectableStack.RemoveAt(random - 1);
            _collectableStack.TrimExcess();
            ScoreSignals.Instance.onSetScore?.Invoke(_collectableStack.Count);
            if (DronePoolSignals.Instance.onGetStackCount() <= 0)
            {
                LevelSignals.Instance.onLevelFailed?.Invoke();
                return;
            }
            if (random.Equals(1))
            {
                ScoreSignals.Instance.onSetLeadPosition?.Invoke(_collectableStack[0]);
            }
        }
    }
}