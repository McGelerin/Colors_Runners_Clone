using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.ValueObject;

public class IdleCarTargetController : MonoBehaviour
{
    [SerializeField] IdleTargetData data;

    public IdleTargetData GetData()
    {
        return data;
    }
}
