using System.Collections.Generic;
using Managers;
using Signals;
using UnityEngine;

namespace Commands
{
    public class ItemRemoveOnStackCommand
    {
        #region Self Variables

        #region Private Variables
        private List<GameObject> _collectableStack;
        private GameObject _levelHolder;
        private StackManager _manager;


        #endregion

        #endregion
        
        public ItemRemoveOnStackCommand(ref List<GameObject> CollectableStack,ref GameObject levelHolder,
            StackManager manager)
        {
            _collectableStack = CollectableStack;
            _levelHolder = levelHolder;
            _manager = manager;
        }
        public void Execute(GameObject collectableGameObject)
        {
            
            int index = _collectableStack.IndexOf(collectableGameObject);
            collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
            collectableGameObject.SetActive(false);
            _collectableStack.RemoveAt(index);
            _collectableStack.TrimExcess();

            if (DronePoolSignals.Instance.onGetStackCount() <= 0)
            {
                LevelSignals.Instance.onLevelFailed?.Invoke();
                return;
            }

            if (index==0)
            {
                ScoreSignals.Instance.onSetLeadPosition?.Invoke(_collectableStack[0]);
            }
            //_onReBuildListCommand.ReBuildList();
            ScoreSignals.Instance.onSetScore?.Invoke(-1);
            
        }
    }
}