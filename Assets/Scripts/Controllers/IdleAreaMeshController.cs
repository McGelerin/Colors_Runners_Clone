using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class IdleAreaMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serializefield Variables

        [SerializeField] private IdleAreaManager manager;
        [SerializeField] private List<MeshRenderer> mesh;

        #endregion

        #endregion
        
        public void ChangeBuildingGradient(float gra)
        {
            Debug.Log(gra);
            foreach (var VARIABLE in mesh)
            {
                VARIABLE.material = VARIABLE.material;
                VARIABLE.material.DOFloat(gra,"_Saturation", 1);
            }
        }
    }
}