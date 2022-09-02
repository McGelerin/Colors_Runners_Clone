using System;
using Signals;
using UnityEngine;
using Managers;
using Enums;

namespace Controllers
{
    public class PlayerParticuleController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private ParticleSystem particleExplode;
        [SerializeField] private ParticleSystem currentParticle;
        [SerializeField] private ParticleSystem currentParticleExplode;

        #endregion

        #endregion

        private void Awake()
        {
            currentParticle = Instantiate(particle, manager.transform.position, particle.transform.rotation);
            currentParticleExplode = Instantiate(particleExplode, manager.transform.position, particleExplode.transform.rotation);
            currentParticle.Stop();
            currentParticleExplode.Stop();
     //       currentParticle.gameObject.SetActive(false);
        }

        public void StartParticule(Transform instantiateTransform)
        {
           // currentParticle = Instantiate(particle, instantiateTransform.position, particle.transform.rotation);
           currentParticle.gameObject.transform.position = instantiateTransform.position;
           currentParticleExplode.gameObject.transform.position = new Vector3(instantiateTransform.position.x, instantiateTransform.position.y + 5, instantiateTransform.position.z);
           currentParticle.Play();
           currentParticleExplode.Play();
        }

        public void StopParticule()
        {
            currentParticle.Stop();
            currentParticleExplode.Stop();
            if (currentParticle.Equals(null))
            {
                Destroy(currentParticle.gameObject, 1f);

            }
            if (currentParticleExplode.Equals(null))
            {
                Destroy(currentParticleExplode.gameObject, 1f);

            }
        }
    }
}