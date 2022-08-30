using Enums;
using Keys;
using Managers;
using UnityEngine;

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

        public void SetSpeedVariable(IdleInputParams inputParams)
        {
            float speedX = Mathf.Abs(inputParams.ValueX);
            float speedZ = Mathf.Abs(inputParams.ValueZ);
            animator.SetFloat("Speed", (speedX + speedZ) / 2);
        }

        public void SetPlayerScale(float value)
        {
            manager.transform.position = new Vector3(manager.transform.position.x, manager.transform.position.y + value / 2, manager.transform.position.z);
            manager.transform.localScale += Vector3.one*value;
        }
    }
}