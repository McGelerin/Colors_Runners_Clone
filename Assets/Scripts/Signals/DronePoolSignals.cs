using Enums;
using Extentions;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Signals
{
    public class DronePoolSignals : MonoSingleton<DronePoolSignals>
    {
        public Func<int> onGetStackCount;
        public UnityAction<Boolean> onOutlineBorder;
        
        public UnityAction<Transform> onDroneArrives;
        public UnityAction<Transform> onDroneGone;
        public UnityAction<Transform> onPlayerCollideWithDronePool;
        public UnityAction<GameObject> onCollectableCollideWithDronePool;
        public UnityAction<GameObject> onWrongDronePool;
        public UnityAction onDronePoolExit;
    }
}