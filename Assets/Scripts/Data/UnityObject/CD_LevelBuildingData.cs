using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_IdleLevel", menuName = "Game/CD_IdleLevelBuilding", order = 0)]
    public class CD_LevelBuildingData : ScriptableObject
    {
        public List<LevelBuildingData> LevelBuildingDatas;
    }
}