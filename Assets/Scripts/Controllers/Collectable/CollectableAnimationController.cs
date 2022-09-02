using Enums;
using UnityEngine;

namespace Controllers
{
    public class CollectableAnimationController : MonoBehaviour
    {
        #region Self Variables
        #region Serialized Variables
        [SerializeField] private Animator animator;
        #endregion
        #endregion
        
        public void SetAnimState(CollectableAnimStates animState)
        {
            animator.SetTrigger(animState.ToString());
        }
    }
}