using DG.Tweening;
using UnityEngine;

namespace Commands
{
    public class SetPlayerPositionAfterDronePool
    {
        #region Private Variables

        private Transform _transform;

        #endregion
        public SetPlayerPositionAfterDronePool(Transform transform)
        {
            _transform = transform;
        }

        public void Execute(Transform dronePoolTransform)
        {
            _transform.position = new Vector3(dronePoolTransform.position.x, _transform.position.y, _transform.position.z + 15f);
            //_transform.DOMove(new Vector3(_transform.position.x, _transform.position.y, _transform.position.z + 20), 1f);
        }
    }
}