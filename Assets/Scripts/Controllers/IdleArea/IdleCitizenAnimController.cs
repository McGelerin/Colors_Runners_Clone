using Enums;
using UnityEngine;

namespace Controllers
{
    public class IdleCitizenAnimController : MonoBehaviour
    {
        #region Self Variables
        #region Serialized Variables
        [SerializeField] private Animator animator;
        #endregion
        #endregion

        public void SetAnimState(IdleCitizenAnimStates animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        public void ResetAnim()
        {
            animator.ResetTrigger(IdleCitizenAnimStates.Idle.ToString());
            animator.ResetTrigger(IdleCitizenAnimStates.Walk.ToString());
        }
    }
}