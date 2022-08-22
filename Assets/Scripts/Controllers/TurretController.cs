using UnityEngine;
using DG.Tweening;

namespace Controllers
{
    public class TurretController: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        [SerializeField] private float rotationSpeed = 1f;

        [SerializeField] private Transform taret1, taret2;

        [SerializeField] private ParticleSystem taret1Particle, taret2Particle;
        
        #endregion

        #endregion

        public void RotateToPlayer(Transform player)
        {
            var position = player.position;
            taret1.DOLookAt(position, rotationSpeed);
            taret2.DOLookAt(position, rotationSpeed);
            taret1Particle.Play();
            taret2Particle.Play();
        }

        public void OnTargetDisappear()
        {

        }
    }
}