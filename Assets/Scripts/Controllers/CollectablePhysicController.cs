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
                Destroy(other.transform.parent.gameObject);
            }

            if (other.CompareTag("BoostArea") && CompareTag("Collected"))
            {
                StackSignals.Instance.onBoostArea?.Invoke();
                //other.gameObject.SetActive(false);
                other.gameObject.GetComponent<MeshCollider>().enabled = false;
            }

          

            if (_isFirstTime && other.CompareTag("DronePoolColor"))
            {
                _isFirstTime = false;
                DronePoolSignals.Instance.onCollectableCollideWithDronePool?.Invoke(transform.parent.gameObject, other.transform);

                manager.SetPoolColor(other.transform.parent.parent.GetComponent<DronePoolManager>().OnGetColor(other.transform)/*DronePoolSignals.Instance.onGetColor(other.transform)*/);//uzerinde bulundugu rengi manager'e bildirir. Manager zaten kendi rengini biliyor.
            }


        }

        public  void CanEnterDronePool()
        {
            _isFirstTime = true;
        }
    }
}

