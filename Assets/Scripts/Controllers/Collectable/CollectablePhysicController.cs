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
                return;
            }
            
            if (other.CompareTag("Obstacle")&& CompareTag("Collected"))
            {
                manager.InteractionWithObstacle(transform.parent.gameObject);
                other.gameObject.SetActive(false);
                return;
            }

            if (other.CompareTag("BoostArea") && CompareTag("Collected"))
            {
                if (manager.transform.GetSiblingIndex()<=5)//funk yap
                {
                    StackSignals.Instance.onBoostArea?.Invoke();
                    other.enabled = false;
                }
                return;
            }
            
            if (_isFirstTime && other.CompareTag("DronePoolColor"))
            {
                InteractionWithDronePool(other);
                return;
            }

            if (other.CompareTag("GunPool"))
            {
                manager.CollectableOnGunPool();
                return;
            }
            
            if (other.CompareTag("GunPoolExit"))
            {
                manager.CollectableOnExitGunPool();
            }
        }

        private void InteractionWithDronePool(Collider other)//managera cek;
        {
            _isFirstTime = false;
            StartCoroutine(manager.CrouchAnim());
            var managerT = manager.transform;
            DronePoolSignals.Instance.onCollectableCollideWithDronePool?.Invoke(transform.parent.gameObject);
            managerT.DOMove(new Vector3(other.transform.position.x, managerT.position.y,
                managerT.position.z + Random.Range(5f, 13f)), 4f);//data olacak
            manager.SetPoolColor(other.transform.parent.GetComponent<DronePoolMeshController>().GetColor(other.transform));
        }

        public  void CanEnterDronePool()
        {
            _isFirstTime = true;
        }
    }
}

