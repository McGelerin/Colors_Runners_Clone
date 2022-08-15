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
        [SerializeField] private PlayerManager Manager;


        #endregion
        #region private vars
        private Transform _poolTransform;
        #endregion
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DronePool"))
            {
                DronePoolSignals.Instance.onPlayerCollideWithDronePool?.Invoke(other.transform);
                Manager.GetDronePoolTransform(other.transform.parent.GetComponent<DronePoolManager>().OnGetTruePoolTransform());
                StartCoroutine(DroneArrives());
            }

            if (other.CompareTag("Finish"))
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke();
                Debug.Log("WORKED FIZIK");
            }

            if (other.CompareTag("DronePoolReset"))
            {
                _poolTransform = other.transform.parent;
                DronePoolSignals.Instance.onDronePoolExit?.Invoke();
            }
        }



        private IEnumerator DroneArrives()
        {
            yield return new WaitForSeconds(2f);
            DronePoolSignals.Instance.onDroneArrives?.Invoke(_poolTransform);
            yield return new WaitForSeconds(2f);
            DronePoolSignals.Instance.onDroneGone?.Invoke();
        }

    }
}