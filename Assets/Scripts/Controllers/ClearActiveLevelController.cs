using UnityEngine;

namespace Controllers
{
    public class ClearActiveLevelController : MonoBehaviour
    {
        public void ClearActiveLevel(Transform levelHolder)
        {
            Destroy(levelHolder.GetChild(0).gameObject);
        }
    }
}