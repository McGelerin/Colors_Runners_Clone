﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.ValueObject
{
    [Serializable]
    public class StackData
    {
        public float CollectableOffsetInStack = 1;
        [Range(0.1f, 0.8f)] 
        public float LerpSpeed = 0.25f;
        [Range(0, 0.2f)] 
        public float ShackAnimDuraction = 0.12f;
        [Range(1f,3f)] 
        public float ShackScaleValue = 1f;

        public int InitialStackItem = 5;
    }
}