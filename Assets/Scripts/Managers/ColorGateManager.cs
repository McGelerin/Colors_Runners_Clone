using System;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ColorGateManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        private ColorEnum ColorEnum
        {
            get => this._colorEnum;
            set
            {
                this._colorEnum = value;
                GetColor();
            }
        }
        

        #endregion
        #region SerializeField Variables

        [SerializeField]
        private ColorEnum _colorEnum;

        private ColorData colorData;

        #endregion

        #endregion

        
        
        private void Start()
        {
            ColorEnum = _colorEnum;
        }
        
        private void GetColor()
        {
            colorData = Resources.Load<CD_Color>("Data/CD_Color").colorData;

            if (ColorEnum == ColorEnum.Rainbow)
            {
                gameObject.GetComponent<Renderer>().material = colorData.RainbowMaterial;
            }
            else gameObject.GetComponent<Renderer>().material.color = colorData.color[(int) ColorEnum];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            { 
                StackSignals.Instance.ColorType?.Invoke(ColorEnum);
            }
        }
        
    }
}