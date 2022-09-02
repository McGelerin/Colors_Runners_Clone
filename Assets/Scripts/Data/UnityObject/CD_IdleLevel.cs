using System.Collections.Generic;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_IdleLevel", menuName = "Game/CD_IdleLevel", order = 0)]
    public class CD_IdleLevel : ScriptableObject
    {
        public List<CD_LevelBuildingData> Levels = new List<CD_LevelBuildingData>();
    }
}