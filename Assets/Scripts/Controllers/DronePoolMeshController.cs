using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using UnityEngine;
namespace Controllers
{
    public class DronePoolMeshController : MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private List<MeshRenderer> colorBlocks;
        #endregion
        #region Private Variables
        private ColorData _colorData;
        private List<ColorEnum> areaColorEnum;
        private ColorEnum trueColorState;
        #endregion
        #endregion

        private void Awake()
        {
            _colorData = GetColorData();
        }

        private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;

        public void SetColors(List<ColorEnum> areaColorEnum)
        {
            this.areaColorEnum = areaColorEnum;
            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                colorBlocks[i].material.color = _colorData.color[(int)areaColorEnum[i]];
            }            
        }

        public ColorEnum GetColor(Transform poolTransform)
        {
            for (int i = 0; i < areaColorEnum.Count; i++)
            {
                if (colorBlocks[i].transform.Equals(poolTransform))
                {
                    return areaColorEnum[i];
                }
            }
            return ColorEnum.Rainbow;
        }

        public void SetTrueColor(ColorEnum trueColor)
        {
            trueColorState = trueColor;
        }
        
        public void PoolScaleReduction()
        {
            for (int i = 0; i < colorBlocks.Count; i++)
            {
                if (areaColorEnum[i]!=trueColorState)
                {
                    colorBlocks[i].transform.DOScaleZ(0, 1f);
                }
            }
        }

        public Transform GetTruePool()
        {
            for (int i = 0; i < colorBlocks.Count; i++)
            {
                if (areaColorEnum[i]==trueColorState)
                {
                    return colorBlocks[i].transform;
                }
            }
            return colorBlocks[0].transform;
        }
    }
}