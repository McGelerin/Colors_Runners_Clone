using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;
using Data.UnityObject;
using Signals;

//[ExecuteInEditMode]
public class GunPoolManager : MonoBehaviour
{
    #region vars
    #region publicVars
    public ColorEnum colorEnum = ColorEnum.Kirmizi;
    
    public List<ColorEnum> areaColorEnum = new List<ColorEnum>();
    #endregion
    #region serializeVars
    [SerializeField] private List<MeshRenderer> ColorBlocks;
    #endregion
    #region privateVars
    private ColorData _colorData;
    private Material _currentMaterial;
    #endregion

    #endregion

    private void SubscribeEvents()
    {
        GunPoolSignals.Instance.onGetColor += OnGetColor;
    }
    private void UnSubscribeEvents()
    {
        GunPoolSignals.Instance.onGetColor -= OnGetColor;
    }
    private void Awake()
    {
        _currentMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        GetColorData();

    }

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void GetColorData()
    {
        _colorData = Resources.Load<CD_Color>("Data/CD_Color").colorData;

        _currentMaterial.color = _colorData.color[(int)colorEnum];
        
        for (int i = 0; i < areaColorEnum.Count; i++)
        {
            ColorBlocks[i].material.color = _colorData.color[(int)areaColorEnum[i]];
        }
    }

    public ColorEnum OnGetColor()
    {
        return colorEnum;
    }
}
