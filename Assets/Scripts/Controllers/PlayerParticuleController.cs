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

        public void StartParticule(Transform instantiateTransform)
        {
            currentParticle = Instantiate(particle, instantiateTransform.position, particle.transform.rotation);
            currentParticle.Play();
        }

        public void StopParticule()
        {
            currentParticle.Stop();
            Destroy(currentParticle.gameObject, 1f);
        }




    }
}