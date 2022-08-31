using System;
using Signals;
using UnityEngine;
using Managers;
using Enums;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

namespace Controllers
{
    public class PlayerParticuleController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private ParticleSystem currentParticle;

        #endregion

        #endregion

        private void Awake()
        {
            currentParticle = Instantiate(particle, manager.transform.position, particle.transform.rotation);
            currentParticle.Stop();
     //       currentParticle.gameObject.SetActive(false);
        }

        public void StartParticule(Transform instantiateTransform)
        {
           // currentParticle = Instantiate(particle, instantiateTransform.position, particle.transform.rotation);
           currentParticle.gameObject.transform.position = instantiateTransform.position;
           currentParticle.Play();
        }

        public void StopParticule()
        {
            currentParticle.Stop();
            if (currentParticle.Equals(null))
            {
                Destroy(currentParticle.gameObject, 1f);

            }
        }
    }
}