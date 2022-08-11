using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Signals;

namespace Controllers
{
    public class TaretController : MonoBehaviour
    {
        #region vars
        #region publicVars


        #endregion
        #region serializeVars
        [SerializeField] private float rotationSpeed = 1f;

        [SerializeField] Transform taret1;
        [SerializeField] Transform taret2;
        #endregion
        #region privateVars
        #endregion

        #endregion

        public void Start()
        {
            //Rotate(GameObject.FindGameObjectWithTag("Player").transform);
        }
        public void RotateToPlayer(Transform player)
        {
            taret1.DOLookAt(player.position, rotationSpeed);
            taret2.DOLookAt(player.position, rotationSpeed);
        }
    

        public void OnTargetDisappear()
        {
            StopAllCoroutines();
        }
    }
}