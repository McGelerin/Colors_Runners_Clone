using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Enums;
using Signals;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using System.Collections;

//[ExecuteInEditMode]
public class GunPoolManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    public ColorEnum ColorEnum;
    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();

    #endregion

    #region Serialized Variables

    [SerializeField] private List<GunPoolPhysicsController> poolPhysicsControllers;
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

    private void Start()
    {
        SetColors();
    }

    private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;

    private void SendColorDataToControllers()
    {
        gunPoolMeshController.SetColorData(_colorData);
    }

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
                poolPhysicsControllers[i].IsTruePool = true;
            }
        }
    }
    private void OnSetPlayer(GameObject playerGameObject)
    {
        _player = playerGameObject;
    }

    public void StartAsyncManager()//isim değişecek
    {
        _isFire = true;
        StartCoroutine(FireAndReload());
    }

    private void OnPlayerExitGunPool()
    {
        StopAsyncManager();
        StartCoroutine(turretController.SearchAnim());
    }

    public void StopAsyncManager()//isim değişecek
    {
        _isFire = false;
    }
    public void StopAllCoroutineTrigger()//isim değiş
    {
        StopAllCoroutines();
    }

    //private async void FireAndReload()
    //{
    //    if (!_isFire) return;
    //    GunPoolSignals.Instance.onWrongGunPool?.Invoke();
    //    turretController.RotateToPlayer(_player.transform);
    //    await Task.Delay(500); 
    //    FireAndReload();
    //}

    private IEnumerator FireAndReload()
    {
        if (!_isFire || DronePoolSignals.Instance.onGetStackCount() == 0) yield break;
        GunPoolSignals.Instance.onWrongGunPool?.Invoke();
        turretController.RotateToPlayer(_player.transform);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FireAndReload());
    }
}