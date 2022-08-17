using System;
using Commands;
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

    public ColorEnum ColorState
    {
        get => colorState;
        set
        {
            colorState = value;
            collectableMeshController.ColorSet();
        }
    }
    
    #endregion
    #region SerializeField Variables
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CollectablePhysicController physicController;
    [SerializeField] private CollectableMeshController collectableMeshController;

    [SerializeField] private Animator animator;
    [SerializeField] private CollectableAnimationController animationController;

    #endregion
    #region Private Variables

    [SerializeField]
    private ColorEnum colorState;
    [Space]
    private ColorData _colorData;
    private ColorEnum _poolColorEnum;


    #endregion

    #endregion
    
    private void Start()
    {
        ColorState = colorState;
        animationController.SetAnimState(CollectableAnimStates.Idle);
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
        DronePoolSignals.Instance.onDronePoolExit += physicController.CanEnterDronePool;
        CoreGameSignals.Instance.onPlay += OnPlay;
    }

    private void UnsubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnCollectableCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        DronePoolSignals.Instance.onDronePoolExit -= physicController.CanEnterDronePool;
        CoreGameSignals.Instance.onPlay -= OnPlay;
    }


    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        collectableMeshController.ColorControl(collectableGameObject);
    }

    public void InteractionWithObstacle(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
    }
    

    #region Onur Workouth

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

    public void OnDroneArrives(Transform _poolTransform)
    {
        if (CompareTag("Collected"))
        {
            if (_poolColorEnum.Equals(ColorState))
            {
            }
            else
            {
                DronePoolSignals.Instance.onWrongDronePool?.Invoke(gameObject);
                animationController.SetAnimState(CollectableAnimStates.Dying);
            }

        }
    }

    public void SetPoolColor(ColorEnum poolColorEnum)
    {
        _poolColorEnum = poolColorEnum;
    }

    #endregion

    private void OnPlay()
    {
        Debug.Log("Onplay");
        animationController.SetAnimState(CollectableAnimStates.Runner);
    }
}