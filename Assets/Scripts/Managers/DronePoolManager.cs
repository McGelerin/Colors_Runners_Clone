using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using Controllers;

public class DronePoolManager : MonoBehaviour
{
    #region Self Variables
    #region Public Variables
    
    public ColorEnum ColorState;
    #endregion

    #region SerializeField Variables
    [SerializeField] private List<ColorEnum> areaColorEnum = new List<ColorEnum>();
    [SerializeField] private List<Collider> colliders = new List<Collider>();
    [SerializeField] private GameObject drone;
    [SerializeField] private DronePoolMeshController dronePoolMeshController;
    #endregion

    #region Private Variables

    private DroneArrivesCommand _droneArrivesCommand;
    private DronePoolData _dronePoolData;
    #endregion

    #endregion
    
    private void Awake()
    {
        GetDroneData();
        Init();

    }

    #region Event Subscription

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        DronePoolSignals.Instance.onDroneArrives += _droneArrivesCommand.Execute;
        DronePoolSignals.Instance.onDroneGone += OnDroneGone;
    }
    
    private void UnSubscribeEvents()
    {
        DronePoolSignals.Instance.onDroneArrives -= _droneArrivesCommand.Execute;
        DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    #endregion
    
    private void Start()
    {
        SetColors();
    }

    private void Init()
    {
        _droneArrivesCommand = new DroneArrivesCommand(ref drone, ref colliders, transform);
    }

    private void GetDroneData()
    {
        _dronePoolData = Resources.Load<CD_Drone>("Data/CD_Drone").Data;
    }
    
    private void SetColors()
    {
        dronePoolMeshController.SetColors(areaColorEnum, ColorState);
    }

    public void DroneArrive(Transform selectedPoolTransform)
    {
        StartCoroutine(DroneArrives());
    }
    
    private void OnDroneGone(Transform dronePoolTransform)
    {
        drone.SetActive(false);
    }
    
    private IEnumerator DroneArrives()
    {
        int stackCount = DronePoolSignals.Instance.onGetStackCount();
        yield return new WaitForSeconds(_dronePoolData.DroneArriveDelay + (0.2f * stackCount));
        DronePoolSignals.Instance.onOutlineBorder?.Invoke(true);
        yield return new WaitForSeconds(1f);
        DronePoolSignals.Instance.onDroneArrives?.Invoke(transform);
        yield return new WaitForSeconds(_dronePoolData.DroneGoneDelay);
        DronePoolSignals.Instance.onOutlineBorder?.Invoke(false);
        yield return new WaitForSeconds(1f);
        DronePoolSignals.Instance.onDroneGone?.Invoke(dronePoolMeshController.GetTruePoolTransform(ColorState));
    }
}
