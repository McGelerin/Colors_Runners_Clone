using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;
namespace Data.ValueObject
{
    [Serializable]
    public class IdleCitizenData
    {
        public float ReachingTime = 5f;
        public float WaitingTime = 1.1f;
        public float RotationTime = 0.2f;
    }
}