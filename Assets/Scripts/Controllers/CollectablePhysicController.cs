using UnityEngine;

namespace Controllers
{
    public class CollectablePhysicController : MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private CollectableManager manager;
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
            }

        }
    }
}

