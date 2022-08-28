using System;
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
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                ScoreSignals.Instance.onVisibleScore?.Invoke(false);
            }

            if (other.CompareTag("Finish"))
            {
                LevelSignals.Instance.onLevelSuccessful?.Invoke();
            }

            if (other.CompareTag("DronePoolReset"))
            {
                DronePoolSignals.Instance.onDronePoolExit?.Invoke();
            }

            if (other.CompareTag("GunPoolExit"))
            {
                manager.Data.MovementData.ForwardSpeed = manager.Data.MovementData.RunSpeed;
                GunPoolSignals.Instance.onGunPoolExit?.Invoke();
            }
            if (other.CompareTag("DronePoolExit"))
            {
                ScoreSignals.Instance.onVisibleScore?.Invoke(true);
            }
            if (other.CompareTag("GunPool"))
            {
                manager.Data.MovementData.ForwardSpeed = manager.Data.MovementData.CrouchSpeed;
            }
        }
        
    }
}