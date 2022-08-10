using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Controllers
{
    public class StackMoveController
    {
        #region Self Variables

        #region Private Veriables

        private StackData _stackData;
        #endregion
        #endregion

        public void InisializedController(StackData Stackdata)
        {
            _stackData = Stackdata;
        }

        public void StackItemsMoveOrigin(float directionX, float directionY, List<GameObject> _collectableStack)
        {
            float directx = Mathf.Lerp(_collectableStack[0].transform.localPosition.x, directionX,_stackData.LerpSpeed);
            float directy = Mathf.Lerp(_collectableStack[0].transform.localPosition.y, directionY,_stackData.LerpSpeed);
            _collectableStack[0].transform.localPosition = new Vector3(directx, directy, 0);
            StackItemsLerpMove(_collectableStack);
        }

        public void StackItemsLerpMove(List<GameObject> _collectableStack)
        {
            for (int i = 1; i < _collectableStack.Count; i++)
            {
                Vector3 pos = _collectableStack[i].transform.localPosition;
                pos.x = _collectableStack[i - 1].transform.localPosition.x;
                pos.y = _collectableStack[i - 1].transform.localPosition.y;
                float directx = Mathf.Lerp(_collectableStack[i].transform.localPosition.x, pos.x, _stackData.LerpSpeed);
                float directy = Mathf.Lerp(_collectableStack[i].transform.localPosition.y, pos.y, _stackData.LerpSpeed);
                _collectableStack[i].transform.localPosition = new Vector3(directx, directy, pos.z);
            }
        }
    }
}