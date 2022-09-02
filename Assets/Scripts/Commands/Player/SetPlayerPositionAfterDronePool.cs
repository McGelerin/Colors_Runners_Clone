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
            var position = _transform.position;
            _transform.DOMove(new Vector3(truePool.position.x, position.y, position.z + 15), 1f);
        }
    }
}