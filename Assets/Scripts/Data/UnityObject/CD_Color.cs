using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;
using Enums;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Color", menuName = "Game/CD_Color", order = 0)]
    public class CD_Color : ScriptableObject
    {
        public ColorData colorData;
    }
}