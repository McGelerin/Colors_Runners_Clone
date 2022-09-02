using Data.UnityObject;
using Data.ValueObject;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCitizenMeshController : MonoBehaviour
{
    #region Self Variables
    #region Serializefield Variables
    [SerializeField] private SkinnedMeshRenderer mesh;
    #endregion
    #region Private Variables
    private ColorData _colorData;
    #endregion
    #endregion

    private void Awake()
    {
        _colorData = GetColorData();
    }
    private ColorData GetColorData() => Resources.Load<CD_Color>("Data/CD_Color").colorData;

    public void SetRandomColor()
    {
        int tempColor = Random.Range(1, _colorData.color.Count);
        mesh.material.color = _colorData.color[tempColor];
    }
}
