using DG.Tweening;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class CollectablePhysicController : MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private CollectableManager manager;
        #endregion
        #region Private Variables
        private bool _isFirstTime = true;
        #endregion
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable") && CompareTag("Collected"))
            {
                manager.InteractionWithCollectable(other.transform.gameObject);
            }
            
            if (other.CompareTag("Obstacle")&& CompareTag("Collected"))
            {
                manager.InteractionWithObstacle(transform.parent.gameObject);
                other.gameObject.SetActive(false);
            }

            if (other.CompareTag("BoostArea") && CompareTag("Collected"))
            {
                if (manager.transform.GetSiblingIndex()<=5)
                {
                    StackSignals.Instance.onBoostArea?.Invoke();
                    other.enabled = false;
                }
            }
            
            if (_isFirstTime && other.CompareTag("DronePoolColor"))
            {
                InteractionWithDronePool(other);
            }

            if (other.CompareTag("GunPool"))
            {
                manager.CollectableOnGunPool();
            }
            
            if (other.CompareTag("GunPoolExit"))
            {
                manager.CollectableOnExitGunPool();
            }
        }

        private void InteractionWithDronePool(Collider other)
        {
            _isFirstTime = false;
            StartCoroutine(manager.CrouchAnim());
            var _managerT = manager.transform;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool?.Invoke(transform.parent.gameObject);
            _managerT.DOMove(new Vector3(other.transform.position.x, _managerT.position.y,
                _managerT.position.z + Random.Range(5f, 15f)), 4f);//data olacak
            manager.SetPoolColor(other.transform.parent.GetComponent<DronePoolMeshController>().OnGetColor(other.transform));
        }

        public  void CanEnterDronePool()
        {
            _isFirstTime = true;
        }
    }
}

