using System.Collections.Generic;
using Enums;

namespace Keys
{
    public struct SaveIdleDataParams
    {
        public int IdleLevel;
        public int CollectablesCount;//Idle Başındaki toplanabilirler
        public List<int> MainCurrentScore;
        public List<int> SideCurrentScore;
        public List<BuildingState> MainBuildingState;
        public List<BuildingState> SideBuildingState;
    }
}