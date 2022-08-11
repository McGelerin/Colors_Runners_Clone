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
        //public StackItemsJumpCommand StackItemsJumpCommand;
        public StackValueUpdateCommand StackValueUpdateCommand;
        public ItemAddOnStackCommand ItemAddOnStackCommand;

        public bool LastCheck;

        #endregion

        #region Private Variables

        private StackMoveController _stackMoveController;
        private ItemRemoveOnStackCommand _itemRemoveOnStackCommand;
        private StackShackAnimCommand _stackShackAnimCommand;
        //private StackInteractionWithConveyorCommand _stackInteractionWithConveyorCommand;
        private InitialzeStackCommand _initialzeStackCommand;
        private OnReBuildListCommand _onReBuildListCommand;

        #endregion

        #region Seralized Veriables

        [SerializeField] private GameObject levelHolder;
        [SerializeField] private GameObject collectable;
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
            StackSignals.Instance.onStackFollowPlayer += OnStackMove;
            StackSignals.Instance.onUpdateType += StackValueUpdateCommand.StackValuesUpdate;
            //
            GunPoolSignals.Instance.onWrongGunPool += _itemRemoveOnStackCommand.RemoveRandomListItem;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoveOnStackCommand.RemoveStackListItems;
            // StackSignals.Instance.onInteractionObstacle -= OnReBuildList;
            StackSignals.Instance.onStackFollowPlayer -= OnStackMove;
            StackSignals.Instance.onUpdateType -= StackValueUpdateCommand.StackValuesUpdate;
            //
            GunPoolSignals.Instance.onWrongGunPool -= _itemRemoveOnStackCommand.RemoveRandomListItem;

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

        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;
        
        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            ItemAddOnStackCommand.AddStackList(collectableGameObject);
            StartCoroutine(_stackShackAnimCommand.StackItemsShackAnim());
            StackValueUpdateCommand.StackValuesUpdate();
        }

        private void OnStackMove(Vector3 direction)
        {
            transform.position = new Vector3(0, 0, direction.z + StackData.DistanceFormPlayer);
            if (gameObject.transform.childCount > 0)
            {
                _stackMoveController.StackItemsMoveOrigin(direction.x, direction.y, CollectableStack);
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

    
    }
}