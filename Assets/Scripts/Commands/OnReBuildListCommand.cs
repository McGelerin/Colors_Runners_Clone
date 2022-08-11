using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class OnReBuildListCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _collectableStack;
        private float _offset = 1f;


        #endregion
        #endregion

        public OnReBuildListCommand(ref List<GameObject> CollectableStack, float offset)
        {
            _collectableStack = CollectableStack;
            _offset = offset;
        }

        public void ReBuildList()
        {
            if (_collectableStack.Count<2)
            {
                return;
            }
            _collectableStack[0].transform.localPosition = new Vector3(_collectableStack[1].transform.localPosition.x,
                _collectableStack[1].transform.localPosition.y, 0);
            
            for (int i = 1; i < _collectableStack.Count; i++)
            {
                _collectableStack[i].transform.localPosition = new Vector3(_collectableStack[i].transform.localPosition.x,
                    _collectableStack[i].transform.localPosition.y, _collectableStack[i - 1].transform.localPosition.z - _offset);
            }
        }
    }
}