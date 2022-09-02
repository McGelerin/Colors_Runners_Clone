using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelBuildingData
    {
        public MainBuildingData mainBuildingData;
        public List<SideBuildindData> sideBuildindData;
    }
}