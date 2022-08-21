using System;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class ColorGateController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        private ColorEnum ColorEnum
        {
            get => this._colorEnum;
            set
            {
                this._colorEnum = value;
                SetColor();
            }
        }
        

        #endregion
        #region SerializeField Variables

        [SerializeField]
        private ColorEnum _colorEnum;

        private ColorData colorData;

        #endregion

        #endregion


        private void Awake()
        {
            colorData = GetColorData();
        }

        private void Start()
        {
            ColorEnum = _colorEnum;
        }
        
        private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;
        
        private void SetColor()
        {
            if (ColorEnum == ColorEnum.Rainbow)
            {
                gameObject.GetComponent<Renderer>().material = colorData.RainbowGageMaterial;
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