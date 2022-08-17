using System;
using Controllers;
using Signals;
using Enums;
using Data.UnityObject;
using Data.ValueObject;
using Sirenix.Serialization;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CollectableManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    public ColorEnum ColorEnum
    {
        get => _colorEnum;
        set
        {
            _colorEnum = value;
            GetColorData();
        }
    }


    #endregion
    #region SerializeField Variables
    [SerializeField] private Material CurrentMaterial;
    [SerializeField] private Transform MeshObject;
    [SerializeField] private Renderer meshRenderer;

    [SerializeField] private Animator animator;
    [SerializeField] private CollectableAnimationController animationController;
    [SerializeField] private CollectablePhysicController physicController;

    #endregion
    #region Private Variables
    
    private ColorData _colorData;
    [SerializeField]
    private ColorEnum _colorEnum;
    public ColorEnum  _poolColorEnum;
    private ColorEnum _otherColorEnum;


    #endregion

    #endregion

    private void Awake()
    {
        animationController.SetAnimState(CollectableAnimStates.Idle);
    }

    private void Start()
    {
        ColorEnum = _colorEnum;
    }
    private void GetColorData()
    {
        _colorData = Resources.Load<CD_Color>("Data/CD_Color").colorData;

        if (ColorEnum == ColorEnum.Rainbow)
        {
            meshRenderer.material = _colorData.RainbowMaterial;
        }
        else
        {
            meshRenderer.material = _colorData.ColorMaterial;
            meshRenderer.material.color = _colorData.color[(int) ColorEnum];
        }
    }

    private void SetColorData(ColorEnum color)
    {
        if (CompareTag("Collected"))
        {
            ColorEnum = color;
        }
    }
    
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onCollectableCollideWithDronePool += OnCollectableCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives += OnDroneArrives;
        StackSignals.Instance.ColorType += SetColorData;
        CoreGameSignals.Instance.onPlay += OnPlay;
    }

    private void UnsubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnCollectableCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        StackSignals.Instance.ColorType -= SetColorData;
        CoreGameSignals.Instance.onPlay -= OnPlay;

    }


    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        collectableGameObject.tag = "Collected";
        _otherColorEnum = collectableGameObject.transform.parent.gameObject.GetComponent<CollectableManager>()
            .ColorEnum;
        if (ColorEnum == _otherColorEnum)
        {
            StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject.transform.parent.gameObject);
        }
        else
        {
            collectableGameObject.transform.parent.gameObject.SetActive(false);
            StackSignals.Instance.onInteractionObstacle?.Invoke(gameObject);
        }
    }

    public void InteractionWithObstacle(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
    }

    public void OnPlayerCollideWithDronePool(Transform poolTrigerTransform)
    {
        if (CompareTag("Collected"))
        {
            transform.DOMoveZ(poolTrigerTransform.position.z + 2 , 1f);
        }
    }

    public void OnCollectableCollideWithDronePool(GameObject collectable, Transform colorTransform)
    {
        if (CompareTag("Collected") && collectable.Equals(gameObject))
        {
            transform.DOMove(new Vector3(colorTransform.position.x, transform.position.y,
                transform.position.z + Random.Range(5f, 15f)), 1f);
        }
    }

    public void OnDroneArrives()
    {
        if (_poolColorEnum.Equals(ColorEnum))
        {

        }
        else
        {
            DronePoolSignals.Instance.onWrongDronePool?.Invoke(gameObject);
            animationController.SetAnimState(CollectableAnimStates.Dying);
        }
    }
    public void SetPoolColor(ColorEnum poolColorEnum)
    {
        _poolColorEnum = poolColorEnum;
        Debug.Log("gelen enum:" + poolColorEnum);
    }

    private void OnPlay()
    {
        animationController.SetAnimState(CollectableAnimStates.Idle);
    }
}