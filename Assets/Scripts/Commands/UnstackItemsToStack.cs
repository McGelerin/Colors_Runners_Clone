using System.Collections.Generic;
using UnityEngine;
using Commands;

namespace Commands
{
    public class UnstackItemsToStack
    {
        #region Self Variables
        #region Private Variables
        private List<GameObject> _collectableStack;
        private List<GameObject> _unStackList;
        private DublicateStateItemsCommand _dublicateStateItemsCommand;
        #endregion
        #endregion
        
        public UnstackItemsToStack(ref List<GameObject> collectableStack,ref List<GameObject> unStackList,ref DublicateStateItemsCommand dublicateStateItemsCommand)
        {
            _collectableStack = collectableStack;
            _unStackList = unStackList;
            _dublicateStateItemsCommand = dublicateStateItemsCommand;
        }
        public void Execute()
        {
            foreach (var i in _unStackList)
            {
                _collectableStack.Add(i);
            }
            _unStackList.Clear();
            _dublicateStateItemsCommand.Execute();
        }
    }
}