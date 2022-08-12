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
                other.tag = "Collected";
                manager.InteractionWithCollectable(other.transform.parent.gameObject);
            }


            if (other.CompareTag("Obstacle")&& CompareTag("Collected"))
            {
                manager.InteractionWithObstacle(transform.parent.gameObject);
                Destroy(other.transform.parent.gameObject);
            }

            if (other.CompareTag("BoostArea") && CompareTag("Collected"))
            {
                other.gameObject.GetComponent<MeshCollider>().enabled = false;
                StackSignals.Instance.onBoostArea?.Invoke();
            }

            if ((other.CompareTag("DronePoolColor")) && CompareTag("Collected"))
            {
            }

            if (_isFirstTime && (other.CompareTag("Kirmizi") || other.CompareTag("Yesil") || other.CompareTag("Mavi") || other.CompareTag("Turkovaz") || other.CompareTag("Sari")) && CompareTag("Collected"))
            {
                _isFirstTime = false;
                DronePoolSignals.Instance.onCollectableCollideWithDronePool?.Invoke(transform.parent.gameObject, other.transform);

                manager.SetPoolColor(other.tag);
            }


        }
    }
}

