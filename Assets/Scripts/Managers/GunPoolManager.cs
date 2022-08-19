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
    public ColorEnum ColorEnum = ColorEnum.Kirmizi;
    
    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();
    #endregion
    #region serializeVars
    [SerializeField] private List<MeshRenderer> colorBlocks;
    [SerializeField] private List<Collider> colliders;

    #endregion
    #region privateVars
    private ColorData _colorData;
    private Material _currentMaterial;
    #endregion

    #endregion

    private void SubscribeEvents()
    {
        //GunPoolSignals.Instance.onWrongGunPool += TaretController.RotateToPlayer;
        //GunPoolSignals.Instance.onWrongGunPoolExit += TaretController.OnTargetDisappear;

    }
    private void UnSubscribeEvents()
    {
        //GunPoolSignals.Instance.onWrongGunPool -= TaretController.RotateToPlayer;
        //GunPoolSignals.Instance.onWrongGunPoolExit -= TaretController.OnTargetDisappear;

    }
    private void Awake()
    {
        _currentMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
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

        _currentMaterial.color = _colorData.color[(int)ColorEnum];
        
        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            colorBlocks[i].material.color = _colorData.color[(int)AreaColorEnum[i]];
        }
    }


    private void SetCollidersActiveness()
    {
        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            if (ColorEnum.Equals(AreaColorEnum[i]))
            {
                colliders[i].enabled = false;
            }
        }
    }
}
