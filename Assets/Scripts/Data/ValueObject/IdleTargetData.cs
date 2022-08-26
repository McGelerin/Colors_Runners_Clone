using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;
namespace Data.ValueObject
{
    [Serializable]
    public class IdleTargetData
    {
        public List<IdleNavigationEnum> axises = new List<IdleNavigationEnum>();
    }
}