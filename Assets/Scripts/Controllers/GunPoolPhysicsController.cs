using UnityEngine;

namespace Controllers
{
    public class GunPoolPhysicsController : MonoBehaviour
    {

        #region Self Variables

        #region Public Variables

        public bool IsTruePool = false;

        #endregion

        #region Serialized Variables

        [SerializeField] GunPoolManager manager;

        #endregion

        #region Private Variables

        //private bool _isTriggered = false;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (IsTruePool.Equals(true))
                {
                    manager.StopAsyncManager();
                }
                else
                {
                    manager.StartAsyncManager();
                    //_isTriggered = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (IsTruePool.Equals(false))
                {
                    manager.StopAllCoroutineTrigger();
                }
            }
        }
    }
}
