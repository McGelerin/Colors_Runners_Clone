using System;
using Enums;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class SideBuildindData
    {
        [Title("Building State")] 
        [EnumToggleButtons] 
        public BuildingState BuildingState = BuildingState.Uncompleted;
        [HorizontalGroup("Game Data",75)]
        [PreviewField(100)]
        [HideLabel]
        public GameObject Building;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        public int SideBuildingScore;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)] 
        public Vector3 InstantitatePos;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        public TextMeshPro BuildingScoreTMP;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)] 
        public Vector3 OffsetTMP;
    }
}