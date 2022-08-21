using DG.Tweening;
using Signals;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Managers;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        [SerializeField] private PlayerManager manager;


        #endregion
        #region private vars
        #endregion
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
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