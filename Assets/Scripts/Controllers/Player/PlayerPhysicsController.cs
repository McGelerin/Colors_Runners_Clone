using System;
using System.Collections;
using Signals;
using UnityEngine;
using Managers;
using Enums;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private ParticleSystem currentParticle;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                ScoreSignals.Instance.onVisibleScore?.Invoke(false);
                return;
            }

            if (other.CompareTag("Finish"))
            {
                LevelSignals.Instance.onLevelSuccessful?.Invoke();
                other.gameObject.SetActive(false);
                return;
            }

            if (other.CompareTag("DronePoolReset"))
            {
                DronePoolSignals.Instance.onDronePoolExit?.Invoke();
                return;
            }

            if (other.CompareTag("GunPoolExit"))
            {
                manager.Data.MovementData.ForwardSpeed = manager.Data.MovementData.RunSpeed;
                GunPoolSignals.Instance.onGunPoolExit?.Invoke();
                return;
            }

            if (other.CompareTag("DronePoolExit"))
            {
                ScoreSignals.Instance.onVisibleScore?.Invoke(true);
                return;
            }

            if (other.CompareTag("GunPool"))
            {
                manager.Data.MovementData.ForwardSpeed = manager.Data.MovementData.CrouchSpeed;
                return;
            }

            if (other.CompareTag("Citizen"))
            {
                //ScoreSignals.Instance.onUpdateScore?.Invoke(1);
                //int currentScore = ScoreSignals.Instance.onGetIdleScore();
                ScoreSignals.Instance.onSetScore?.Invoke(1);
                return;
            }

            if (other.CompareTag("CollectableIdle"))
            {
                ScoreSignals.Instance.onSetScore?.Invoke(1);
                other.transform.parent.gameObject.SetActive(false);
                IdleSignals.Instance.onIdleCollectableValue(-1);
            }
        }
    }
}