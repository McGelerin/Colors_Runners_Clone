using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelBuilding
    {
        [HorizontalGroup("LevelDetails")]
        [LabelWidth(100)]
        public int LevelNumber;
        [HorizontalGroup("LevelDetails")]
        [LabelWidth(100)]
        public GameObject LevelPlane;
        public List<LevelBuildingData> LevelBuildingDatas;
    }
}