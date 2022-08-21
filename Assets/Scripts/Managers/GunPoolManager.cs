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
    [SerializeField] private List<GunPoolPhysicsController> poolControllers;

    [SerializeField] TurretController turretController;

    #endregion
    #region privateVars
    private ColorData _colorData;
    private Material _currentMaterial;
    private bool _isFire = false;

    private bool _isReady = true;

    private GameObject _player;

    #endregion

    #endregion

    private void SubscribeEvents()
    {
        //GunPoolSignals.Instance.onWrongGunPool += TaretController.RotateToPlayer;
        //GunPoolSignals.Instance.onWrongGunPoolExit += TaretController.OnTargetDisappear;

        StackSignals.Instance.onPlayerGameObject += OnSetPlayer;
        GunPoolSignals.Instance.onGunPoolExit += OnPlayerExitGunPool;

    }
    private void UnSubscribeEvents()
    {
        //GunPoolSignals.Instance.onWrongGunPool -= TaretController.RotateToPlayer;
        //GunPoolSignals.Instance.onWrongGunPoolExit -= TaretController.OnTargetDisappear;

        StackSignals.Instance.onPlayerGameObject -= OnSetPlayer;
        GunPoolSignals.Instance.onGunPoolExit -= OnPlayerExitGunPool;


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
                poolControllers[i].IsTrue = true;
            }
        }

    }

    IEnumerator FireAndReload()
    {
        if (_isFire)
        {
            GunPoolSignals.Instance.onWrongGunPool?.Invoke();
            turretController.RotateToPlayer(_player.transform);
            yield return new WaitForSeconds(0.2f);
            //_isFire = false;
            StartCoroutine(FireAndReload());

        }
    }

    private void OnSetPlayer(GameObject playerGameObject)
    {
        _player = playerGameObject;
    }

    public void StopCoroutineManager()
    {
        StopAllCoroutines();
        _isFire = false;
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

}
