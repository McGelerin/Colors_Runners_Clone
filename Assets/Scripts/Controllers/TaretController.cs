using UnityEngine;
using DG.Tweening;

namespace Controllers
{
    public class TaretController : MonoBehaviour
    {
        #region vars
        #region publicVars


        #endregion
        #region serializeVars
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float fireDelay = 0.5f;
        #endregion
        #region privateVars

        #endregion

        #endregion

        public void Start()
        {
            //Rotate(GameObject.FindGameObjectWithTag("Player").transform);
        }
        public void Rotate(Transform player)
        {
            transform.DOLookAt(player.position, rotationSpeed);
        }
        public void Fire()
        {

        }
    }
}