using System;
using System.Numerics;

namespace Data.ValueObject
{
    [Serializable]
    public class DronePoolData
    {
        public float DroneArriveDelay = 2f;
        public float DroneGoneDelay = 2f;
    }
}