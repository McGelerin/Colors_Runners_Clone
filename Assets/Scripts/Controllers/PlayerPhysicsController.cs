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

        
        #endregion
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                StartCoroutine(DroneArrives());
            }

            if (other.CompareTag("Finish"))
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke();
                Debug.Log("WORKED FIZIK");
            }
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