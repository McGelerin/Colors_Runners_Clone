using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Signals;
using Data.UnityObject;
using Data.ValueObject;
using Commands;
using DG.Tweening;
using Enums;


namespace Managers
{
    public class StackManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public StackData StackData;
        public List<GameObject> CollectableStack = new List<GameObject>();
        public List<GameObject> UnStack = new List<GameObject>();
        public StackValueUpdateCommand StackValueUpdateCommand;
        public ItemAddOnStackCommand ItemAddOnStackCommand;

        #endregion

        #region Seralized Veriables
        [SerializeField] private GameObject levelHolder;
        [SerializeField] private GameObject collectable;
        #endregion

        #region Private Variables

        private StackMoveController _stackMoveController;
        private ItemRemoveOnStackCommand _itemRemoveOnStackCommand;
        private RandomRemoveListItemCommand _randomRemoveListItemCommand;
        private StackShackAnimCommand _stackShackAnimCommand;
        private InitialzeStackCommand _initialzeStackCommand;
        private GameObject _playerGameObject;
        private SetColorState _setColorState;
        //private ColorEnum _gateState;

        private bool _isPlayerOnDronePool = false;

        #endregion
        #endregion

        #region Event Subscription
        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            StackSignals.Instance.onInteractionCollectable += OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle += _itemRemoveOnStackCommand.Execute;
            StackSignals.Instance.onPlayerGameObject += OnSetPlayer;
            StackSignals.Instance.onUpdateType += StackValueUpdateCommand.Execute;
            StackSignals.Instance.ColorType += OnGateState;
            GunPoolSignals.Instance.onWrongGunPool += _randomRemoveListItemCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool += OnStackToUnStack;
            DronePoolSignals.Instance.onWrongDronePool += OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone += OnDroneGone;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoveOnStackCommand.Execute;
            StackSignals.Instance.onPlayerGameObject -= OnSetPlayer;
            StackSignals.Instance.onUpdateType -= StackValueUpdateCommand.Execute;
            StackSignals.Instance.ColorType -= OnGateState;
            GunPoolSignals.Instance.onWrongGunPool -= _randomRemoveListItemCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnStackToUnStack;
            DronePoolSignals.Instance.onWrongDronePool -= OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
        }
        private void OnDisable()
        {
            UnSubscribeEvent();
        }
        #endregion

        private void Awake()
        {
            StackData = GetStackData();
            Init();
        }
        
        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;
        
        private void Start()
        {
            _initialzeStackCommand.Execute();
        }
        
        private void Init()
        {
            _stackMoveController = new StackMoveController();
            _stackMoveController.InisializedController(StackData);
            ItemAddOnStackCommand = new ItemAddOnStackCommand(ref CollectableStack, transform, StackData);
            _itemRemoveOnStackCommand = new ItemRemoveOnStackCommand(ref CollectableStack, ref levelHolder, this);
            _randomRemoveListItemCommand = new RandomRemoveListItemCommand(ref CollectableStack, ref levelHolder, this);
            _stackShackAnimCommand = new StackShackAnimCommand(ref CollectableStack, StackData);
            StackValueUpdateCommand = new StackValueUpdateCommand(ref CollectableStack);
            _initialzeStackCommand = new InitialzeStackCommand(collectable, this);
            _setColorState = new SetColorState(ref CollectableStack);
        }

        private void Update()
        {
            if (_isPlayerOnDronePool)
            {
                StackXMoveOnDronePool();
            }
            else
                StackMove();
        }

        private void OnSetPlayer(GameObject player) //Find yapmamak için oyun başladığında 1 kere playerı gonderiyorum
        {
            _playerGameObject = player;
        }
        
        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            ItemAddOnStackCommand.Execute(collectableGameObject);
            collectableGameObject.tag = "Collected";
            StartCoroutine(_stackShackAnimCommand.Execute());
            StackValueUpdateCommand.Execute();
        }

        private void StackMove()
        {
            if (_playerGameObject !=  null)
            {
                //transform.position = new Vector3(0, 0, direction.z + StackData.DistanceFormPlayer);
                Vector3 direction = new Vector3(_playerGameObject.transform.position.x,
                    _playerGameObject.transform.position.y, _playerGameObject.transform.position.z);
                if (gameObject.transform.childCount > 0)
                {
                    _stackMoveController.StackItemsMoveOrigin(direction, CollectableStack);
                }
            }
        }
        
        private void OnPlayerCollideWithDronePool(Transform poolTriggerTransform)
        {
            _isPlayerOnDronePool = true;
        }

        private void OnGateState(ColorEnum gateColorState)
        {
            _setColorState.Execute(gateColorState);
        }
        
        private void OnPlay()
        {
            
        }

        private void OnReset()
        {
            foreach (Transform childs in transform)
            {
                Destroy(childs.gameObject);
            }
            CollectableStack.Clear();
        }
        
        private void StackXMoveOnDronePool()
        {
            if (_playerGameObject != null)
            {
                //transform.position = new Vector3(0, 0, direction.z + StackData.DistanceFormPlayer);
                Vector3 direction = new Vector3(_playerGameObject.transform.position.x,
                    _playerGameObject.transform.position.y, _playerGameObject.transform.position.z);
                if (gameObject.transform.childCount > 0)
                {
                    _stackMoveController.StackItemsMoveOrigin(direction, CollectableStack, true);
                }
            }
        }

        private void OnStackToUnStack(GameObject collectable, Transform colorTransform)
        {
            UnStack.Add(collectable);

            if (CollectableStack.Contains(collectable))
            {
                CollectableStack.Remove(collectable);
            }
            CollectableStack.TrimExcess();
        }

        private void OnWrongDronePoolCollectablesDelete(GameObject wrongPoolCollectable)
        {
            if (UnStack.Contains(wrongPoolCollectable))
            {
                wrongPoolCollectable.tag = "Collectable";
                UnStack.Remove(wrongPoolCollectable);
            }
        }

        private void OnDroneGone(Transform transform)
        {
            _isPlayerOnDronePool = false;
            foreach (var i in UnStack)
            {
                CollectableStack.Add(i);
            }
            UnStack.Clear();
        }
    }
}
