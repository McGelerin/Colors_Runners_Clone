using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.ValueObject;

public class IdleCarTargetController : MonoBehaviour
{
    [SerializeField] IdleTargetData Data;

    public IdleTargetData GetData()
    {
        return Data;
    }
}
