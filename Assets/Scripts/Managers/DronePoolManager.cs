using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Collections.Generic;
using UnityEngine;

public class DronePoolManager : MonoBehaviour
{
    #region vars
    #region publicVars
    public ColorEnum ColorEnum = ColorEnum.Kirmizi;

    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();
    #endregion
    #region serializeVars
    [SerializeField] private List<MeshRenderer> colorBlocks;
    [SerializeField] private GameObject drone;

    #endregion
    #region privateVars
    private ColorData _colorData;
    private Material _currentMaterial;
    #endregion

    #endregion

    private void SubscribeEvents()
    {
        DronePoolSignals.Instance.onDroneArrives += OnDroneArrives;
        DronePoolSignals.Instance.onDroneGone += OnDroneGone;
    }
    private void UnSubscribeEvents()
    {
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
    }
    private void Awake()
    {
        _currentMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
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

        _currentMaterial.color = _colorData.color[(int)ColorEnum];

        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            colorBlocks[i].material.color = _colorData.color[(int)AreaColorEnum[i]];
        }
    }
    
    public Transform GetTruePoolTransform()
    {
        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            if (AreaColorEnum[i].Equals(ColorEnum))
            {
                return colorBlocks[i].transform;
            }
        }
        return transform;
    }

    public ColorEnum OnGetColor(Transform poolTransform)
    {
        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            if (colorBlocks[i].transform.Equals(poolTransform))
            {
                return AreaColorEnum[i];
            }
        }
        return ColorEnum.Kirmizi;
    }

    private void OnDroneArrives(Transform _poolTransform)
    {
        if (transform.Equals(_poolTransform))
        {
            drone.SetActive(true);
        }
    }
    
    private void OnDroneGone()
    {
        drone.SetActive(false);
    }
}
