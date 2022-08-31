using System;
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
            }

            if (other.CompareTag("Finish"))
            {
                LevelSignals.Instance.onLevelSuccessful?.Invoke();
                other.gameObject.SetActive(false);
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
            if (other.CompareTag("Buy"))
            {
                manager.SetAnim(CollectableAnimStates.Buy);
                manager.ParticuleState(true, other.transform);

            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Buy"))
            {
                StayCondition();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Buy"))
            {
                manager.SetAnim(CollectableAnimStates.Run);
                manager.ParticuleState(false);
            }
        }
        
        private async void  StayCondition()
        {
            ScoreSignals.Instance.onSetScore?.Invoke(-1);
            await Task.Delay(250);
        }
    }
}