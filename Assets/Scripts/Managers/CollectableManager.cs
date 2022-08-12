using Controllers;
using Signals;
using Enums;
using Data.UnityObject;
using Data.ValueObject;
using Sirenix.Serialization;
using UnityEngine;
using DG.Tweening;

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

    #endregion
    #region Private Variables
    
    private ColorData _colorData;
    [SerializeField]
    private ColorEnum _colorEnum;
    public string  _poolColor;


    #endregion

    #endregion


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
    }

    private void UnsubscribeEvents()
    {
        DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
        DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnCollectableCollideWithDronePool;
        DronePoolSignals.Instance.onDroneArrives -= OnDroneArrives;
        StackSignals.Instance.ColorType -= SetColorData;

    }


    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
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
            transform.DOMove(new Vector3(colorTransform.position.x, transform.position.y, transform.position.z + Random.Range(5f, 15f)), 1f);
        }
    }

    public void OnDroneArrives()
    {
        if (_poolColor.Equals(ColorEnum.ToString()))
        {

        }
        else
        {
            DronePoolSignals.Instance.onWrongDronePool?.Invoke(gameObject);
            //Dead Anim
        }
    }

    public void SetPoolColor(string color)
    {
        _poolColor = color;
    }
}