using Signals;
using UnityEngine;
using Managers;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private PlayerManager manager;
        
        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                other.transform.parent.GetComponent<DronePoolManager>().DroneArrive(other.transform);
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                DronePoolSignals.Instance.onDronePoolEnter?.Invoke();
            }

            if (other.CompareTag("Finish"))
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke();
            }

            if (other.CompareTag("DronePoolReset"))
            {
                DronePoolSignals.Instance.onDronePoolExit?.Invoke();
            }

            if (other.CompareTag("GunPoolExit"))
            {
                GunPoolSignals.Instance.onGunPoolExit?.Invoke();
            }
        }
    }
}