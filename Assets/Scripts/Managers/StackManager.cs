using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Signals;
using Data.UnityObject;
using Data.ValueObject;
using Commands;
using DG.Tweening;


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

        public bool LastCheck;

        #endregion

        #region Seralized Veriables
        [SerializeField] private GameObject levelHolder;
        [SerializeField] private GameObject collectable;
        #endregion

        #region Private Variables

        private StackMoveController _stackMoveController;
        private ItemRemoveOnStackCommand _itemRemoveOnStackCommand;
        private StackShackAnimCommand _stackShackAnimCommand;
        private InitialzeStackCommand _initialzeStackCommand;
        private OnReBuildListCommand _onReBuildListCommand;
        private GameObject _playerGameObject;

        private bool _isOnDronePool = false;

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
            StackSignals.Instance.onInteractionObstacle += _itemRemoveOnStackCommand.RemoveStackListItems;
            // StackSignals.Instance.onInteractionObstacle += OnReBuildList;
            StackSignals.Instance.onPlayerGameObject += OnSetPlayer;
            StackSignals.Instance.onUpdateType += StackValueUpdateCommand.StackValuesUpdate;
            //
            GunPoolSignals.Instance.onWrongGunPool += _itemRemoveOnStackCommand.RemoveRandomListItem;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool += OnDronePool;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool += OnStackToUnStack;
            DronePoolSignals.Instance.onWrongDronePool += OnWrongDronePoolCollectablesDelete;
            DronePoolSignals.Instance.onDroneGone += OnDroneGone;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoveOnStackCommand.RemoveStackListItems;
            // StackSignals.Instance.onInteractionObstacle -= OnReBuildList;
            StackSignals.Instance.onPlayerGameObject -= OnSetPlayer;
            StackSignals.Instance.onUpdateType -= StackValueUpdateCommand.StackValuesUpdate;
            //
            GunPoolSignals.Instance.onWrongGunPool -= _itemRemoveOnStackCommand.RemoveRandomListItem;
            DronePoolSignals.Instance.onPlayerCollideWithDronePool -= OnDronePool;
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
            _stackMoveController = new StackMoveController();
            _stackMoveController.InisializedController(StackData);
            ItemAddOnStackCommand = new ItemAddOnStackCommand(ref CollectableStack, transform, StackData);
            _onReBuildListCommand = new OnReBuildListCommand(ref CollectableStack, StackData.CollectableOffsetInStack);
            _itemRemoveOnStackCommand = new ItemRemoveOnStackCommand(ref CollectableStack,ref levelHolder, this,ref _onReBuildListCommand);
            _stackShackAnimCommand = new StackShackAnimCommand(ref CollectableStack, StackData);
            StackValueUpdateCommand = new StackValueUpdateCommand(ref CollectableStack);
            _initialzeStackCommand = new InitialzeStackCommand(collectable, this);
        }

        private void Update()
        {
            if (_isOnDronePool)
            {
                StackXMoveOnDronePool();
            }
            else
                StackMove();
        }

        private void OnSetPlayer(GameObject player)
        {
            _playerGameObject = player;
        }
        
        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;
        
        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            ItemAddOnStackCommand.AddStackList(collectableGameObject);
            collectableGameObject.tag = "Collected";
            StartCoroutine(_stackShackAnimCommand.StackItemsShackAnim());
            StackValueUpdateCommand.StackValuesUpdate();
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
                    _stackMoveController.StackItemsMoveOrigin(direction.x, direction.y, direction.z, CollectableStack);
                }
            }
        }
        private void OnDronePool(Transform poolTriggerTransform)
        {
            _isOnDronePool = true;
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
                    _stackMoveController.StackItemsMoveOrigin(direction.x, direction.y, direction.z, CollectableStack, true);
                }
            }
        }

        private void OnPlay()
        {
            LastCheck = false;
            _initialzeStackCommand.InitialzeStack();
        }

        private void OnReset()
        {
            foreach (Transform childs in transform)
            {
                Destroy(childs.gameObject);
            }
            CollectableStack.Clear();
        }

        private void OnStackToUnStack(GameObject collectable, Transform colorTransform)
        {
            UnStack.Add(collectable);

            if (CollectableStack.Contains(collectable))
            {
                CollectableStack.Remove(collectable);

            }


            CollectableStack.TrimExcess();
            //_itemRemoveOnStackCommand.RemoveStackListItems(collectable);
        }

        private void OnWrongDronePoolCollectablesDelete(GameObject wrongPoolCollectable)
        {
            if (UnStack.Contains(wrongPoolCollectable))
            {
                UnStack.Remove(wrongPoolCollectable);
            }
        }

        private void OnDroneGone()
        {
            _isOnDronePool = false;
            foreach (var i in UnStack)
            {
                CollectableStack.Add(i);
            }
        }
    }
}