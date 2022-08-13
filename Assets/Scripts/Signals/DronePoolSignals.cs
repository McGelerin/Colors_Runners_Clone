using Enums;
using Extentions;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Signals
{
    public class DronePoolSignals : MonoSingleton<DronePoolSignals>
    {
        public Func<Transform> onGetTruePoolTransform;
        public UnityAction onDroneArrives;
        public UnityAction onDroneGone;
        public UnityAction<Transform> onPlayerCollideWithDronePool;
        public UnityAction<GameObject, Transform> onCollectableCollideWithDronePool;
        public UnityAction<GameObject> onWrongDronePool;
    }
}