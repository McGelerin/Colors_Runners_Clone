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

        [SerializeField] private Transform taret1, taret2;

        [SerializeField] private ParticleSystem taret1Particle, taret2Particle;


        #endregion
        #region privateVars
        #endregion

        #endregion

      
        public void RotateToPlayer(Transform player)
        {
            taret1.DOLookAt(player.position, rotationSpeed);
            taret2.DOLookAt(player.position, rotationSpeed);
            taret1Particle.Play();
            taret2Particle.Play();
        }
    

        public void OnTargetDisappear()
        {

        }
    }
}