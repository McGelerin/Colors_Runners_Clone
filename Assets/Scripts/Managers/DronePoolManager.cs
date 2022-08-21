using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class DronePoolManager : MonoBehaviour
{
    #region vars
    #region publicVars
    public ColorEnum ColorEnum;

    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();
    public List<Collider> colliders = new List<Collider>();
    #endregion

    #region serializeVars
    [SerializeField] private GameObject drone;
    [SerializeField] private DronePoolMeshController dronePoolMeshController;
    #endregion

    #region privateVars
    private DronePoolData _dronePoolData;
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
        GetDroneData();
    }
    private void Start()
    {
        SetColors();
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
    private void SetColors()
    {
        dronePoolMeshController.SetColors(AreaColorEnum, ColorEnum);
    }

    public void OnPlayerCollideWithDronePool(Transform selectedPoolTransform)
    {
        if (selectedPoolTransform.parent.Equals(transform))
        {
            StartCoroutine(DroneArrives());
        }
    }

    private void OnDroneArrives(Transform _poolTransform)
    {
        if (transform.Equals(_poolTransform))
        {
            drone.SetActive(true);

            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = false;
            }
        }
    }
    
    private void OnDroneGone(Transform dronePoolTransform)
    {
        drone.SetActive(false);
        
    }
    private IEnumerator DroneArrives()
    {
        int stackCount = DronePoolSignals.Instance.onGetStackCount();
        yield return new WaitForSeconds(_dronePoolData.DroneArriveDelay + (0.2f * stackCount));
        DronePoolSignals.Instance.onDroneArrives?.Invoke(transform);
        yield return new WaitForSeconds(_dronePoolData.DroneGoneDelay);
        DronePoolSignals.Instance.onDroneGone?.Invoke(dronePoolMeshController.GetTruePoolTransform(ColorEnum));
    }
}
