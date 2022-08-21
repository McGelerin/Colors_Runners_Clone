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
using System.Collections;

public class CollectableManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables
    [Header("Data")] public CollectableData Data;

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
    [SerializeField] private CollectablePhysicController physicController;
    [SerializeField] private CollectableMeshController collectableMeshController;

    [SerializeField] private CollectableAnimationController animationController;
    [SerializeField]
    private ColorEnum colorState;
    [SerializeField] private CollectableAnimStates initialAnimState;
    #endregion
    #region Private Variables
    [Space]
    private ColorData _colorData;
    private ColorEnum _poolColorEnum;
    


    #endregion

    #endregion

    private void Awake()
    {
        Data = GetCollectableData();
    }

    private CollectableData GetCollectableData() => Resources.Load<CD_Collectable>("Data/CD_Collectable").Data;

    private void Start()
    {
        ColorState = colorState;
        SetCollectableAnimation(initialAnimState);
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
        DronePoolSignals.Instance.onDroneGone += OnDroneGone;
    }

    private void UnsubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnCollectableCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        DronePoolSignals.Instance.onDronePoolExit -= physicController.CanEnterDronePool;
        CoreGameSignals.Instance.onPlay -= OnPlay;
        DronePoolSignals.Instance.onDroneGone -= OnDroneGone;

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

    private void OnPlay()
    {
        initialAnimState = CollectableAnimStates.Runner;

        SetCollectableAnimation(CompareTag("Collectable")
            ? CollectableAnimStates.Idle
            : CollectableAnimStates.Runner);
    }
    
    #region Onur Workouth

    public void OnPlayerCollideWithDronePool(Transform poolTrigerTransform)
    {
        if (CompareTag("Collected"))
        {
            transform.DOMoveZ(poolTrigerTransform.position.z + 2, Data.CollectablesMoveToDronePoolTriggerTime);
            Data.CollectablesMoveToDronePoolTriggerTime += 0.2f;
            StartCoroutine(CrouchAnim());
        }
    }

    public IEnumerator CrouchAnim()
    {
        yield return new WaitForSeconds(Data.CollectablesMoveToDronePoolTriggerTime);
        SetCollectableAnimation(CollectableAnimStates.Crouching);
    }

    public void OnCollectableCollideWithDronePool(GameObject collectable, Transform colorTransform)
    {
        if (CompareTag("Collected") && collectable.Equals(gameObject))
        {
            transform.DOMove(new Vector3(colorTransform.position.x, transform.position.y,
                transform.position.z + Random.Range(5f, 15f)), Data.CollectablesMoveToDronePoolTime);
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
                SetCollectableAnimation(CollectableAnimStates.Dying);
                StartCoroutine(SetActiveFalse());
            }

        }
    }

    public void CollectableOnGunPool()
    {
        SetCollectableAnimation(CollectableAnimStates.CrouchedWalking);

    }

    public void CollectableOnExitGunPool()
    {
        if (CompareTag("Collected"))
        {
            SetCollectableAnimation(CollectableAnimStates.Runner);
        }
    }

    public void SetPoolColor(ColorEnum poolColorEnum)
    {
        _poolColorEnum = poolColorEnum;
    }



    public void OutLineBorder(bool isOutlineOn)
    {
        collectableMeshController.SetOutlineBorder(isOutlineOn);
    }

    private void OnDroneGone(Transform transform)
    {
        if (CompareTag("Collected"))
        {
            SetCollectableAnimation(CollectableAnimStates.Runner);
            Data.CollectablesMoveToDronePoolTriggerTime = 1;

        }
    }

    private IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void SetCollectableAnimation(CollectableAnimStates newAnimState)
    {
        animationController.SetAnimState(newAnimState);
    }


    #endregion
}