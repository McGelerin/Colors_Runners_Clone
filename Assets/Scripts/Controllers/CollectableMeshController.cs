using System;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;
namespace Controllers
{
    public class CollectableMeshController: MonoBehaviour
    {
        #region Self Variables
        #region Serializefield Variables
        [SerializeField] private CollectableManager manager;
        [SerializeField] private SkinnedMeshRenderer mesh;
        #endregion
        #region Private Variables
        private ColorEnum _otherColorState;
        private ColorData _colorData;
        #endregion
        #endregion

        private void Awake()
        {
            _colorData = GetColorData();
        }
        
        private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;
        
        public void ColorControl(GameObject otherGameObject)
        {
            otherGameObject.tag = "Collected";
            _otherColorState = otherGameObject.transform.parent.gameObject.GetComponent<CollectableManager>().ColorState;

            if (manager.ColorState == _otherColorState)
            {
                StackSignals.Instance.onInteractionCollectable?.Invoke(otherGameObject.transform.parent.gameObject);
            }
            else
            {
                otherGameObject.transform.parent.gameObject.SetActive(false);
                StackSignals.Instance.onInteractionObstacle?.Invoke(manager.gameObject);
            }
        }
        
        public void ColorSet()
        {
            if (manager.ColorState == ColorEnum.Rainbow)
            {
                mesh.material = _colorData.RainbowMaterial;
            }
            else
            {
                mesh.material = _colorData.ColorMaterial;
                mesh.material.color = _colorData.color[(int) manager.ColorState];
            }
        }
    }
}