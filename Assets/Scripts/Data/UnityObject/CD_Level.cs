using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "Game/CD_Level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public List<int> Levels = new List<int>();
    }
}