using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Collections;
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
    private DronePoolData _dronePoolData;
    private Material _currentMaterial;
    #endregion

    #endregion

    private void SubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives += OnDroneArrives;
        DronePoolSignals.Instance.onDroneGone += OnDroneGone;
    }
    private void UnSubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
    }
    private void Awake()
    {
        _currentMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        GetColorData();
        GetDroneData();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void GetDroneData()
    {
        _dronePoolData = Resources.Load<CD_Drone>("Data/CD_Drone").Data;
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
    
    public void OnPlayerCollideWithDronePool(Transform selectedPoolTransform)
    {
        if (selectedPoolTransform.parent.Equals(transform))
        {
            StartCoroutine(DroneArrives());
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

    private IEnumerator DroneArrives()
    {
        yield return new WaitForSeconds(_dronePoolData.DroneArriveDelay);
        DronePoolSignals.Instance.onDroneArrives?.Invoke(transform);
        yield return new WaitForSeconds(_dronePoolData.DroneGoneDelay);
        DronePoolSignals.Instance.onDroneGone?.Invoke();
    }


}
