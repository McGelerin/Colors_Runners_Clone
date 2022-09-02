using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using DG.Tweening;
namespace Data.ValueObject
{
    [Serializable]
    public class IdleCarData
    {
        public float ReachingTime = 2f;
        public float RotationTime = 1f;
        public Ease RotationEase = Ease.Unset;
        public Ease ReachingEase = Ease.Linear;
    }
}