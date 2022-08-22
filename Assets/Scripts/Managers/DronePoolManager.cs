using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Threading.Tasks;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using Controllers;

public class DronePoolManager : MonoBehaviour
{
    #region Self Variables
    #region Public Variables
    public ColorEnum ColorState;
    [HideInInspector]public bool SelectedArea=false;
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
        _droneArrivesCommand = new DroneArrivesCommand(ref drone, ref colliders,transform);
    }

    #region Event Subscription
    private void OnEnable()
    {
        SubscribeEvents();
    }
    
    private void SubscribeEvents()
    {
        DronePoolSignals.Instance.onUnstackFull += DroneArrives;
        DronePoolSignals.Instance.onDroneArrives += _droneArrivesCommand.Execute;
        DronePoolSignals.Instance.onDroneGone += OnDroneGone;
    }
    
    private void UnSubscribeEvents()
    {
        DronePoolSignals.Instance.onUnstackFull -= DroneArrives;
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
    
    private void GetDroneData()
    {
        _dronePoolData = Resources.Load<CD_Drone>("Data/CD_Drone").Data;
    }
    
    private void SetColors()
    {
        dronePoolMeshController.SetColors(areaColorEnum, ColorState);
    }

    private void OnDroneGone()
    {
        drone.SetActive(false);
    }
    
    private async void DroneArrives()
    {
        if (SelectedArea)
        {
            await Task.Delay(500);
            DronePoolSignals.Instance.onOutlineBorder?.Invoke(true);
            await Task.Delay(1500);
            DronePoolSignals.Instance.onDroneArrives?.Invoke(transform);
            await Task.Delay(1000);
            DronePoolSignals.Instance.onOutlineBorder?.Invoke(false);
            await Task.Delay(1500);
            DronePoolSignals.Instance.onDroneGone?.Invoke();
        }
    }
}
