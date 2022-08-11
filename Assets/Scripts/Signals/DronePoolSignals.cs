using Enums;
using Extentions;
using System;
using UnityEngine.Events;

namespace Signals
{
    public class DronePoolSignals : MonoSingleton<DronePoolSignals>
    {
        public Func<ColorEnum> onGetColor;
        public UnityAction onDroneArrives;
        public UnityAction onDroneGone;
        public UnityAction onPlayerCollideWithDronePool;
    }
}