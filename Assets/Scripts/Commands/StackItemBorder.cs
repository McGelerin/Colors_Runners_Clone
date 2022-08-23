using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Commands
{
    public class StackItemBorder
    {
        #region Self Variables

        #region Private Variables

        [CanBeNull] private List<GameObject> _unstack;
        
        #endregion
        #endregion

        public StackItemBorder(ref List<GameObject> unStack)
        {
            _unstack = unStack;
        }

        public void Execute(bool isOutlineOpen)
        {
            
            for (int i = 0; i < _unstack.Count; i++)
            {
                _unstack[i].GetComponent<CollectableManager>().OutLineBorder(isOutlineOpen);
            }
        }
    }
}