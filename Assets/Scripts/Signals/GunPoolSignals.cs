using Enums;
using Extentions;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace Signals
{
    public class GunPoolSignals : MonoSingleton<GunPoolSignals>
    { 
        public UnityAction<Transform> onWrongGunPool = delegate { };
        public UnityAction onWrongGunPoolExit = delegate { };
        public UnityAction onGunPoolExit = delegate { };


    }
}