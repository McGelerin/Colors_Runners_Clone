using Enums;
using Extentions;
using System;
using UnityEngine.Events;

namespace Signals
{
    public class GunPoolSignals : MonoSingleton<GunPoolSignals>
    {
        public Func<ColorEnum> onGetColor;
    }
}