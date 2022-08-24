using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class DroneArrivesCommand
    {
        #region Self Variables
        #region Private Variables
        private GameObject _drone;
        private List<Collider> _colliders;
        private Transform _transform;
        #endregion
        #endregion

        public DroneArrivesCommand(ref GameObject drone,ref List<Collider>colliders,Transform transform)
        {
            _drone = drone;
            _colliders = colliders;
            _transform = transform;

        }

        public void Execute(Transform _poolTransform)
        {
            if (!_transform.Equals(_poolTransform)) return;
            _drone.SetActive(true);

            foreach (var t in _colliders)
            {
                t.enabled = false;
            }
        }
    }
}