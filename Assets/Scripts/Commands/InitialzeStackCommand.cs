using UnityEngine;
using Managers;


namespace Command
{
    public class InitialzeStackCommand
    {
        #region Self Variables

        #region Private Variables

        private StackManager _manager;
        private GameObject _collectable;
        #endregion
        #endregion

        public InitialzeStackCommand(GameObject collectable,StackManager Manager)
        {
            _collectable = collectable;
            _manager = Manager;
        }
        public void InitialzeStack()
        {
            for (int i = 1; i < _manager.StackData.InitialStackItem; i++)
            {
                GameObject obj = Object.Instantiate(_collectable);
                _manager.ItemAddOnStackCommand.AddStackList(obj);
            }
            _manager.StackValueUpdateCommand.StackValuesUpdate();
        }
    }
}