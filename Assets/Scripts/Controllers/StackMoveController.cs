﻿using System.Collections.Generic;
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

        public void StackItemsMoveOrigin(float directionX, float directionY,float directionZ, List<GameObject> _collectableStack)
        {
            float directx = Mathf.Lerp(_collectableStack[0].transform.localPosition.x, directionX,_stackData.LerpSpeed);
            float directy = Mathf.Lerp(_collectableStack[0].transform.localPosition.y, directionY,_stackData.LerpSpeed);
            float directz = Mathf.Lerp(_collectableStack[0].transform.localPosition.z, directionZ + _stackData.DistanceFormPlayer ,_stackData.LerpSpeed);
            
            _collectableStack[0].transform.localPosition = new Vector3(directx, directy, directz);
            StackItemsLerpMove(_collectableStack);
        }

        public void StackItemsLerpMove(List<GameObject> _collectableStack)
        {
            for (int i = 1; i < _collectableStack.Count; i++)
            {
                Vector3 pos = _collectableStack[i].transform.localPosition;
                pos.x = _collectableStack[i - 1].transform.localPosition.x;
                pos.y = _collectableStack[i - 1].transform.localPosition.y;
                pos.z = _collectableStack[i - 1].transform.localPosition.z - _stackData.CollectableOffsetInStack;
                float directx = Mathf.Lerp(_collectableStack[i].transform.localPosition.x, pos.x, _stackData.LerpSpeed);
                float directy = Mathf.Lerp(_collectableStack[i].transform.localPosition.y, pos.y, _stackData.LerpSpeed/2);
                //float directz = Mathf.Lerp(_collectableStack[i].transform.localPosition.z, pos.z, 0);
                _collectableStack[i].transform.localPosition = new Vector3(directx, directy, pos.z);
            }
        }
    }
}