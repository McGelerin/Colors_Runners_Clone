using System.Collections.Generic;
using Managers;
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
        private OnReBuildListCommand _onReBuildListCommand;

        #endregion

        #endregion
        
        public ItemRemoveOnStackCommand(ref List<GameObject> CollectableStack,ref GameObject levelHolder,
            StackManager manager,ref OnReBuildListCommand onReBuildListCommand)
        {
            _collectableStack = CollectableStack;
            _levelHolder = levelHolder;
            _manager = manager;
            _onReBuildListCommand = onReBuildListCommand;
        }
        public void RemoveStackListItems(GameObject collectableGameObject)
        {
            int index = _collectableStack.IndexOf(collectableGameObject);
            collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
            collectableGameObject.SetActive(false);
            _collectableStack.RemoveAt(index);
            _collectableStack.TrimExcess();
            _manager.StackValueUpdateCommand.StackValuesUpdate();
            //_onReBuildListCommand.ReBuildList();
        }

        public void RemoveRandomListItem(Transform transform)
        {
            if (_collectableStack.Count < 1)
            {
                return;
            }
            int random = Random.Range(0, _collectableStack.Count);
            GameObject selectedCollectable = _collectableStack[random];
            selectedCollectable.transform.SetParent(_levelHolder.transform.GetChild(0));
            selectedCollectable.SetActive(false);
            _collectableStack.RemoveAt(random);
            _collectableStack.TrimExcess();
            _manager.StackValueUpdateCommand.StackValuesUpdate();
            _onReBuildListCommand.OnReBuildList();
        }
    }
}