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

    public List<ColorEnum> areaColorEnum = new List<ColorEnum>();
    #endregion
    #region serializeVars
    [SerializeField] private List<MeshRenderer> ColorBlocks;
    [SerializeField] private List<Collider> Colliders;
    [SerializeField] private GameObject Drone;

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
        DronePoolSignals.Instance.onGetTruePoolTransform += OnGetTruePoolTransform;
        DronePoolSignals.Instance.onGetColor += OnGetColor;

    }
    private void UnSubscribeEvents()
    {
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
        DronePoolSignals.Instance.onGetTruePoolTransform -= OnGetTruePoolTransform;
        DronePoolSignals.Instance.onGetColor -= OnGetColor;

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

        _currentMaterial.color = _colorData.color[(int)colorEnum];

        for (int i = 0; i < areaColorEnum.Count; i++)
        {
            ColorBlocks[i].material.color = _colorData.color[(int)areaColorEnum[i]];
        }
    }


    public Transform OnGetTruePoolTransform()
    {
        for (int i = 0; i < areaColorEnum.Count; i++)
        {
            if (areaColorEnum[i].Equals(colorEnum))
            {
                return ColorBlocks[i].transform;
            }
        }
        return transform;
    }

    public ColorEnum OnGetColor(Transform poolTransform)
    {
        for (int i = 0; i < areaColorEnum.Count; i++)
        {
            if (ColorBlocks[i].transform.Equals(poolTransform))
            {
                return areaColorEnum[i];
            }
        }
        return ColorEnum.Kirmizi;
    }

    private void OnDroneArrives()
    {
        Drone.SetActive(true);
    }
    private void OnDroneGone()
    {
        Drone.SetActive(false);
    }
}
