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
        public List<GameObject> CollectableStack = new List<GameObject>();
        public List<GameObject> UnstackList = new List<GameObject>();
        public ItemAddOnStackCommand ItemAddOnStack;

        #endregion

        #region Seralized Veriables
        [SerializeField] private GameObject levelHolder;
        [SerializeField] private GameObject collectable;
        #endregion

        #region Private Variables

        private StackData _stackData;
        private StackMoveController _stackMoveController;
        private ItemRemoveOnStackCommand _itemRemoveOnStackCommand;
        private RandomRemoveListItemCommand _randomRemoveListItemCommand;
        private StackShackAnimCommand _stackShackAnimCommand;
        private InitialzeStackCommand _initialzeStackCommand;
        private DublicateStateItemsCommand _dublicateStateItemsCommand;
        private StackItemBorder _stackItemBorder;
        private GameObject _playerGameObject;
        private SetColorState _setColorState;
        private Transform _poolTriggerTransform;
        private UnstackItemsToStack _unstackItemsToStack;

        private bool _isPlayerOnDronePool = false;
        private Vector3 _direction;

        #endregion
        #endregion
        
        private void Awake()
        {
            _stackData = GetStackData();
            Init();
        }
        
        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;
        
        private void Init()
        {
            _stackMoveController = new StackMoveController();
            _stackMoveController.InisializedController(_stackData);
            ItemAddOnStack = new ItemAddOnStackCommand(ref CollectableStack, transform, _stackData);
            _itemRemoveOnStackCommand = new ItemRemoveOnStackCommand(ref CollectableStack, ref levelHolder);
            _randomRemoveListItemCommand = new RandomRemoveListItemCommand(ref CollectableStack, ref levelHolder);
            _stackShackAnimCommand = new StackShackAnimCommand(ref CollectableStack, _stackData);
            _initialzeStackCommand = new InitialzeStackCommand(collectable, this);
            _setColorState = new SetColorState(ref CollectableStack);
            _dublicateStateItemsCommand = new DublicateStateItemsCommand(ref CollectableStack, ref ItemAddOnStack);
            _stackItemBorder = new StackItemBorder(ref UnstackList);
            _unstackItemsToStack =new UnstackItemsToStack(ref CollectableStack, ref UnstackList, ref _dublicateStateItemsCommand,gameObject);
        }

        #region Event Subscription
        private void OnEnable()
        {
            SubscribeEvent();
            _initialzeStackCommand.Execute(_stackData.InitialStackItem);
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            StackSignals.Instance.onInteractionCollectable += OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle += _itemRemoveOnStackCommand.Execute;
            StackSignals.Instance.onPlayerGameObject += OnSetPlayer;
            StackSignals.Instance.ColorType += OnGateState;
            GunPoolSignals.Instance.onWrongGunPool += _randomRemoveListItemCommand.Execute;
            GunPoolSignals.Instance.onGunPoolExit += _dublicateStateItemsCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool += OnStackToUnstack;
            DronePoolSignals.Instance.onWrongDronePool += OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone += OnDroneGone;
            DronePoolSignals.Instance.onGetStackCount += OnGetStackCount;
            StackSignals.Instance.onGetCurrentScore += OnGetStackCount;
            DronePoolSignals.Instance.onOutlineBorder += _stackItemBorder.Execute;
            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoveOnStackCommand.Execute;
            StackSignals.Instance.onPlayerGameObject -= OnSetPlayer;
            StackSignals.Instance.ColorType -= OnGateState;
            GunPoolSignals.Instance.onWrongGunPool -= _randomRemoveListItemCommand.Execute;
            GunPoolSignals.Instance.onGunPoolExit -= _dublicateStateItemsCommand.Execute;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnPlayerCollideWithDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool -= OnStackToUnstack;
            DronePoolSignals.Instance.onWrongDronePool -= OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone -= OnDroneGone;
            DronePoolSignals.Instance.onGetStackCount -= OnGetStackCount;
            StackSignals.Instance.onGetCurrentScore -= OnGetStackCount;
            DronePoolSignals.Instance.onOutlineBorder -= _stackItemBorder.Execute;
            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        }
        private void OnDisable()
        {
            UnSubscribeEvent();
        }
        #endregion

        private void Start()
        {
            ScoreSignals.Instance.onSetScore?.Invoke(CollectableStack.Count);
        }
        
        private void Update()
        {
            if (_isPlayerOnDronePool)StackMove(true);
            else StackMove();
        }

        private void OnSetPlayer(GameObject player)
        {
            _playerGameObject = player;
        }
        
        private void StackMove(bool isOnDronePool=false)
        {
             if (gameObject.transform.childCount > 0)
             {
                _stackMoveController.StackItemsMoveOrigin(_playerGameObject.transform.position, CollectableStack,isOnDronePool);
             }
        }
        
        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            ItemAddOnStack.Execute(collectableGameObject);
            collectableGameObject.tag = "Collected";
            StartCoroutine(_stackShackAnimCommand.Execute());
        }

        private void OnPlayerCollideWithDronePool(Transform poolTriggerTransform)
        {
            _poolTriggerTransform = poolTriggerTransform;
            _isPlayerOnDronePool = true;
            CollectableStack[0].transform.DOMoveZ(CollectableStack[0].transform.position.z + 5, 1f);
        }

        private void OnGateState(ColorEnum gateColorState)
        {
            _setColorState.Execute(gateColorState);
        }

        private void OnReset()
        {
            foreach (Transform childs in transform)
            {
                Destroy(childs.gameObject);
            }
            CollectableStack.Clear();
            _initialzeStackCommand.Execute(_stackData.InitialStackItem);
        }

        private void OnStackToUnstack(GameObject collectable)//command olabilir
        {
            UnstackList.Add(collectable);
            collectable.transform.SetParent(levelHolder.transform);
            CollectableStack.Remove(collectable);
            CollectableStack.TrimExcess();
            StackMoveToPool();
        }

        private void StackMoveToPool()
        {
            if (CollectableStack.Count > 0) OnPlayerCollideWithDronePool(_poolTriggerTransform);
            else DronePoolSignals.Instance.onUnstackFull?.Invoke();
        }

        private void OnWrongDronePoolCollectablesDelete(GameObject wrongPoolCollectable)
        {
            wrongPoolCollectable.tag = "Collectable";
            UnstackList.Remove(wrongPoolCollectable);
        }

        private void OnDroneGone()
        {
            _isPlayerOnDronePool = false;
            _unstackItemsToStack.Execute();
        }
        
        private int OnGetStackCount()
        {
            return CollectableStack.Count;
        }

        private void OnLevelSuccessful()
        {
            var lastCollectable = CollectableStack[CollectableStack.Count - 1];
            var itemDuration = 1;
            foreach (var item in CollectableStack)
            {
                item.transform.SetParent(levelHolder.transform);
                item.transform.DOMove(_playerGameObject.transform.position, .1f*itemDuration).OnComplete(()=>
                {
                    if (lastCollectable.Equals(item))
                    {
                        StackSignals.Instance.onLastCollectableAddedToPlayer?.Invoke(true);
                    }
                    item.SetActive(false);
                    StackSignals.Instance.onSetPlayerScale?.Invoke(.1f);

                  
                });
                itemDuration += 1;
                
            }
            CollectableStack.Clear();
            CollectableStack.TrimExcess();
        }
    }
}