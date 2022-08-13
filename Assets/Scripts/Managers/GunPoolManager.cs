using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;
using Data.UnityObject;
using Signals;
using Controllers;

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
    [SerializeField] private List<Collider> Colliders;

    [SerializeField] private TaretController TaretController;

    #endregion
    #region privateVars
    private ColorData _colorData;
    private Material _currentMaterial;
    #endregion

    #endregion

    private void SubscribeEvents()
    {
        GunPoolSignals.Instance.onWrongGunPool += TaretController.RotateToPlayer;
        GunPoolSignals.Instance.onWrongGunPoolExit += TaretController.OnTargetDisappear;

    }
    private void UnSubscribeEvents()
    {
        GunPoolSignals.Instance.onWrongGunPool -= TaretController.RotateToPlayer;
        GunPoolSignals.Instance.onWrongGunPoolExit -= TaretController.OnTargetDisappear;

    }
    private void Awake()
    {
        _currentMaterial = GetComponent<MeshRenderer>().material;
        GetColorData();
        SetCollidersActiveness();

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


    private void SetCollidersActiveness()
    {
        for (int i = 0; i < areaColorEnum.Count; i++)
        {
            if (colorEnum.Equals(areaColorEnum[i]))
            {
                Colliders[i].enabled = false;
            }
        }
    }
}
