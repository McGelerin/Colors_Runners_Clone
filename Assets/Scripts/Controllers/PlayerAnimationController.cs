using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private Animator animator;

        #endregion

        #endregion


        public void SetAnimState(CollectableAnimStates animState)
        {
            animator.SetTrigger(animState.ToString());
        }
    }
}