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
        
        public UnityAction<Transform> onDroneArrives = delegate { };
        public UnityAction onUnstackFull = delegate { };
        public UnityAction<bool> onOutlineBorder = delegate { };
        public UnityAction<Transform> onPlayerGotoTruePool = delegate{  };
        public UnityAction onDroneGone = delegate { };



        public UnityAction<Transform> onPlayerCollideWithDronePool = delegate { };
        public UnityAction<GameObject> onCollectableCollideWithDronePool = delegate { };
        public UnityAction<GameObject> onWrongDronePool= delegate { };
        public UnityAction onDronePoolExit = delegate { };
        public UnityAction onDronePoolEnter = delegate { };
    }
}