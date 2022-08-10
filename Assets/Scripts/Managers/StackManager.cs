using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Signals;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Commands;


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
            StackSignals.Instance.onInteractionObstacle += OnReBuildList;
            StackSignals.Instance.onStackFollowPlayer += OnStackMove;
            StackSignals.Instance.onUpdateType += StackValueUpdateCommand.StackValuesUpdate;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoveOnStackCommand.RemoveStackListItems;
            StackSignals.Instance.onInteractionObstacle -= OnReBuildList;
            StackSignals.Instance.onStackFollowPlayer -= OnStackMove;
            StackSignals.Instance.onUpdateType -= StackValueUpdateCommand.StackValuesUpdate;
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
            _itemRemoveOnStackCommand = new ItemRemoveOnStackCommand(ref CollectableStack, levelHolder, this);
            _stackShackAnimCommand = new StackShackAnimCommand(ref CollectableStack, StackData);
            StackValueUpdateCommand = new StackValueUpdateCommand(ref CollectableStack);
            _initialzeStackCommand = new InitialzeStackCommand(collectable, this);
        }

        private StackData GetStackData() => Resources.Load<CD_Stack>("Data/CD_StackData").StackData;

        private void OnInteractionWithATM(GameObject collectableGameObject)
        {
            //ScoreSignals.Instance.onSetAtmScore?.Invoke((int)collectableGameObject.GetComponent<CollectableManager>()
            //    .CollectableTypeValue + 1);
            if (LastCheck == false)
            {
                _itemRemoveOnStackCommand.RemoveStackListItems(collectableGameObject);
            }
            else
            {
                collectableGameObject.SetActive(false);
            }
        }

        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            ItemAddOnStackCommand.AddStackList(collectableGameObject);
            StartCoroutine(_stackShackAnimCommand.StackItemsShackAnim());
            StackValueUpdateCommand.StackValuesUpdate();
        }

        private void OnStackMove(Vector3 direction)
        {
            transform.position = new Vector3(0, gameObject.transform.position.y, direction.z - 0.5f);
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

        private void OnReBuildList(GameObject gameObject)
        {
            CollectableStack[0].transform.localPosition = new Vector3(CollectableStack[1].transform.localPosition.x, CollectableStack[1].transform.localPosition.y, 0);
            for (int i = 1; i < CollectableStack.Count; i++)
            {
                CollectableStack[i].transform.localPosition = new Vector3(CollectableStack[i].transform.localPosition.x, CollectableStack[i].transform.localPosition.y, CollectableStack[i - 1].transform.localPosition.z - 1);
            }
        }
    }
}