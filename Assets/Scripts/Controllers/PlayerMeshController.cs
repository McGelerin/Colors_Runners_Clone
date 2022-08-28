using UnityEngine;

namespace Controllers
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        #endregion

        #endregion

        public void ShowSkinnedMesh()
        {
            skinnedMeshRenderer.enabled = true;
        }
    }
}