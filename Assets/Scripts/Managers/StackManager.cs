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
        public ItemAddOnStackCommand ItemAddOnStack;

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
        private DublicateStateItemsCommand _dublicateStateItemsCommand;
        private GameObject _playerGameObject;
        private SetColorState _setColorState;
        private Transform _poolTriggerTransform;

        private bool _isPlayerOnDronePool = false;
        private Vector3 _direction;

        #endregion
        #endregion
        
        private void Awake()
        {
            StackData = GetStackData();
            Init();
        }

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
            GunPoolSignals.Instance.onGunPoolExit += _dublicateStateItemsCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool += OnStackToUnstack;
            DronePoolSignals.Instance.onWrongDronePool += OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone += OnDroneGone;
            DronePoolSignals.Instance.onGetStackCount += OnGetStackCount;
            DronePoolSignals.Instance.onOutlineBorder += OnStackItemBorder;
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
            GunPoolSignals.Instance.onGunPoolExit -= _dublicateStateItemsCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnStackToUnstack;
            DronePoolSignals.Instance.onWrongDronePool -= OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
            DronePoolSignals.Instance.onGetStackCount -= OnGetStackCount;
            DronePoolSignals.Instance.onOutlineBorder -= OnStackItemBorder;
        }
        private void OnDisable()
        {
            UnSubscribeEvent();
        }
        #endregion
        
        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;
        
        private void Start()
        {
            _initialzeStackCommand.Execute(StackData.InitialStackItem);
        }
        
        private void Init()
        {
            _stackMoveController = new StackMoveController();
            _stackMoveController.InisializedController(StackData);
            ItemAddOnStack = new ItemAddOnStackCommand(ref CollectableStack, transform, StackData);
            _itemRemoveOnStackCommand = new ItemRemoveOnStackCommand(ref CollectableStack, ref levelHolder, this);
            _randomRemoveListItemCommand = new RandomRemoveListItemCommand(ref CollectableStack, ref levelHolder, this);
            _stackShackAnimCommand = new StackShackAnimCommand(ref CollectableStack, StackData);
            StackValueUpdateCommand = new StackValueUpdateCommand(ref CollectableStack);
            _initialzeStackCommand = new InitialzeStackCommand(collectable, this);
            _setColorState = new SetColorState(ref CollectableStack);
            _dublicateStateItemsCommand = new DublicateStateItemsCommand(ref CollectableStack, ref ItemAddOnStack);

        }

        private void Update()
        {
            if (_isPlayerOnDronePool)
            {
                StackMove(true);
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
            ItemAddOnStack.Execute(collectableGameObject);
            collectableGameObject.tag = "Collected";
            StartCoroutine(_stackShackAnimCommand.Execute());
            StackValueUpdateCommand.Execute();
        }

        private void StackMove(bool isOnDronePool=false)
        {
            _direction = new Vector3(_playerGameObject.transform.position.x,
                    _playerGameObject.transform.position.y, _playerGameObject.transform.position.z);
            
            if (gameObject.transform.childCount > 0)
            {
                _stackMoveController.StackItemsMoveOrigin(_direction, CollectableStack,isOnDronePool);
            }
        }

        private void OnPlayerCollideWithDronePool(Transform poolTriggerTransform)
        {
            _poolTriggerTransform = poolTriggerTransform;
            _isPlayerOnDronePool = true;
            CollectableStack[0].transform.DOMoveZ(poolTriggerTransform.position.z + 2, 1f);
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
            _initialzeStackCommand.Execute(StackData.InitialStackItem);

        }

        private void OnStackToUnstack(GameObject collectable)
        {
            UnStack.Add(collectable);

            if (CollectableStack.Contains(collectable))
            {
                CollectableStack.Remove(collectable);
            }
            CollectableStack.TrimExcess();
            
            if (CollectableStack.Count!=0)
            {
                OnPlayerCollideWithDronePool(_poolTriggerTransform);
            }
        }

        private void OnStackItemBorder(Boolean isOutlineOpen)
        {
            for (int i = 0; i < UnStack.Count; i++)
            {
                UnStack[i].GetComponent<CollectableManager>().OutLineBorder(isOutlineOpen);
            }
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
            _dublicateStateItemsCommand.Execute();
        }
        
        public int OnGetStackCount()
        {
            return CollectableStack.Count;
        }
    }
}