using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using UnityEngine;
namespace Controllers
{
    public class DronePoolMeshController : MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private List<MeshRenderer> colorBlocks;
        [SerializeField] private MeshRenderer trueColorBlock;



        #endregion
        #region Private Variables
        private ColorData _colorData;
        private List<ColorEnum> areaColorEnum;
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
            this.areaColorEnum = areaColorEnum;
            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                colorBlocks[i].material.color = _colorData.color[(int)areaColorEnum[i]];
            }            
        }

        public ColorEnum OnGetColor(Transform poolTransform)
        {
            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                if (colorBlocks[i].transform.Equals(poolTransform))
                {
                    return areaColorEnum[i];
                }
            }
            return ColorEnum.Kirmizi;
        }

        public Transform GetTruePoolTransform(ColorEnum trueColorEnum)
        {
            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                if (areaColorEnum[i].Equals(trueColorEnum))
                {
                    return colorBlocks[i].transform;
                }
            }
            return transform;
        }
    }
}