using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Signals;

namespace Controllers
{
    public class TurretController: MonoBehaviour
    {
        #region vars
        #region publicVars


        #endregion
        #region serializeVars
        [SerializeField] private float rotationSpeed = 1f;

        [SerializeField] Transform taret1;
        [SerializeField] Transform taret2;

        [SerializeField] ParticleSystem taret1uc, taret2uc;


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
            taret1uc.Play();
            taret2uc.Play();
        }
    

        public void OnTargetDisappear()
        {
            StopAllCoroutines();
        }
    }
}