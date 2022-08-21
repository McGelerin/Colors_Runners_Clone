using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using UnityEngine;
namespace Controllers
{
    public class GunPoolMeshController: MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private List<MeshRenderer> colorBlocks;
        [SerializeField] private MeshRenderer trueColorBlock;


        #endregion
        #region Private Variables
        private ColorData _colorData;

        #endregion
        #endregion

        private void Awake()
        {
            _colorData = GetColorData();
        }

        private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;

        public void SetColors(List<ColorEnum> areaColorEnum, ColorEnum trueBlockEnum)
        {
            trueColorBlock.material.color = _colorData.color[(int)trueBlockEnum];

            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                colorBlocks[i].material.color = _colorData.color[(int)areaColorEnum[i]];
            }            
        }
    }
}