using DG.Tweening;
using Signals;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new Rigidbody rigidbody;
        
        #endregion
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                StartCoroutine(DroneArrives());
            }
        }


        public void Jump(float jumpForce)
        {
            rigidbody.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }

        private IEnumerator DroneArrives()
        {
            yield return new WaitForSeconds(2f);
            DronePoolSignals.Instance.onDroneArrives?.Invoke();
            yield return new WaitForSeconds(2f);
            DronePoolSignals.Instance.onDroneGone?.Invoke();
        }
    }
}