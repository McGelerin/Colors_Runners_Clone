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
    public ColorEnum ColorEnum;
    public List<ColorEnum> AreaColorEnum = new List<ColorEnum>();
    #endregion

    #region serializeVars
    [SerializeField] private List<GunPoolPhysicsController> poolControllers;
    [SerializeField] TurretController turretController;
    [SerializeField] GunPoolMeshController gunPoolMeshController;
    #endregion

    #region privateVars
    private bool _isFire = false;
    private GameObject _player;
    #endregion

    #endregion

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
    private void Awake()
    {
        SetTruePool();
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

    public void StartCoroutineManager()
    {
        _isFire = true;
        StartCoroutine(FireAndReload());
    }

    public void OnPlayerExitGunPool()
    {
        StopCoroutineManager();
    }

    public void StopCoroutineManager()
    {
        StopAllCoroutines();
        _isFire = false;
    } 

    IEnumerator FireAndReload()
    {
        if (_isFire)
        {
            GunPoolSignals.Instance.onWrongGunPool?.Invoke();
            turretController.RotateToPlayer(_player.transform);
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(FireAndReload());
        }
    }
}
