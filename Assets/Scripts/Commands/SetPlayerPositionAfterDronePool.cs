using DG.Tweening;
using UnityEngine;

namespace Commands
{
    public class SetPlayerPositionAfterDronePool
    {
        #region Self Variables
        #region Private Variables
        private Transform _transform;
        #endregion
        #endregion
        public SetPlayerPositionAfterDronePool(Transform transform)
        {
            _transform = transform;
        }

        public void Execute(Transform truePool)
        {
            _transform.DOMove(new Vector3(truePool.position.x, _transform.position.y, _transform.position.z + 15), 1f);
        }
    }
}