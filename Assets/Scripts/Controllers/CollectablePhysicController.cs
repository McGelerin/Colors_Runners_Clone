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
        #region Serializefield Variables
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
                //Destroy(other.transform.parent.gameObject);
                other.gameObject.SetActive(false);
            }

            if (other.CompareTag("BoostArea") && CompareTag("Collected"))
            {
                if (manager.transform.GetSiblingIndex()<=5)
                {
                    StackSignals.Instance.onBoostArea?.Invoke();
                    //other.gameObject.SetActive(false);
                    other.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                
            }
            
            if (_isFirstTime && other.CompareTag("DronePoolColor"))
            {
                _isFirstTime = false;
                DronePoolSignals.Instance.onCollectableCollideWithDronePool?.Invoke(transform.parent.gameObject, other.transform);

                manager.SetPoolColor(other.transform.parent.GetComponent<DronePoolMeshController>().OnGetColor(other.transform)/*DronePoolSignals.Instance.onGetColor(other.transform)*/);//uzerinde bulundugu rengi manager'e bildirir. Manager zaten kendi rengini biliyor.
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

        public  void CanEnterDronePool()
        {
            _isFirstTime = true;
        }
    }
}

