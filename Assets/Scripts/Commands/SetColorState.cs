using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Commands
{
    public class SetColorState
    {
        #region Self Variables
        #region Private Variables
        private List<GameObject> _stackList;
        #endregion
        #endregion

        public SetColorState(ref List<GameObject> stackList)
        {
            _stackList = stackList;
        }

        public void Execute(ColorEnum gateColorState)
        {
            foreach (var t in _stackList)
            {
                t.GetComponent<CollectableManager>().ColorState = gateColorState;
            }
        }
    }
}