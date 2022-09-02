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
            if (!(manager.transform.localScale.z + value <= 3) ||!(manager.transform.localScale.z >=1 )) return;
            var transform1 = manager.transform;
            var position = transform1.position;

            if (value > 0)
            {
                position = new Vector3(position.x, position.y + value / 2, position.z);
            }
            transform1.position = position;
            transform1.localScale += Vector3.one*value;
        }
    }
}