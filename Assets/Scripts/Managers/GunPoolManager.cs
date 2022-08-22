using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Enums;
using Signals;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;

//[ExecuteInEditMode]
public class GunPoolManager : MonoBehaviour
{
    #region Self Variables
    
    #region Public Variables
    
    public ColorEnum ColorEnum;
    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();
    
    #endregion

    #region Serialized Variables
    
    [SerializeField] private List<GunPoolPhysicsController> poolControllers;
    [SerializeField] TurretController turretController;
    [SerializeField] GunPoolMeshController gunPoolMeshController;
    
    #endregion

    #region Private Variables
    
    private bool _isFire = false;
    private GameObject _player;
    private ColorData _colorData;
    
    #endregion

    #endregion

    private void Awake()
    {
        _colorData = GetColorData();
        SetTruePool();
        SendColorDataToControllers();
    }
    
    private void Start()
    {
        SetColors();
    }

    private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;

    private void SendColorDataToControllers()
    {
        gunPoolMeshController.SetColorData(_colorData);
    }

    #region Event Subscription 

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        StackSignals.Instance.onPlayerGameObject += OnSetPlayer;
        GunPoolSignals.Instance.onGunPoolExit += OnPlayerExitGunPool;
    }
    private void UnSubscribeEvents()
    {
        StackSignals.Instance.onPlayerGameObject -= OnSetPlayer;
        GunPoolSignals.Instance.onGunPoolExit -= OnPlayerExitGunPool;
    }
    
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    #endregion
   

    private void SetColors()
    {
        gunPoolMeshController.SetColors(AreaColorEnum, ColorEnum);
    }


    private void SetTruePool()
    {
        for (int i = 0; i < AreaColorEnum.Count; i++)
        {
            if (ColorEnum.Equals(AreaColorEnum[i]))
            {
                poolControllers[i].IsTruePool = true;
            }
        }
    }
    private void OnSetPlayer(GameObject playerGameObject)
    {
        _player = playerGameObject;
    }

    public void StartAsyncManager()
    {
        _isFire = true;
        FireAndReload();
    }

    private void OnPlayerExitGunPool()
    {
        StopAsyncManager();
    }

    public void StopAsyncManager()
    {
        _isFire = false;
    } 
    
    private async void FireAndReload()
    {
        if (!_isFire) return;
        GunPoolSignals.Instance.onWrongGunPool?.Invoke();
        turretController.RotateToPlayer(_player.transform);
        await Task.Delay(500); 
        FireAndReload();
    }
}
