using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;
using Data.UnityObject;
using Signals;

public class DronePoolManager : MonoBehaviour
{
    #region vars
    #region publicVars
    public ColorEnum colorEnum = ColorEnum.Kirmizi;

    #endregion
    #region serializeVars
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
        _currentMaterial = GetComponent<MeshRenderer>().material;
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
    }

    public ColorEnum OnGetColor()
    {
        return colorEnum;
    }


}
