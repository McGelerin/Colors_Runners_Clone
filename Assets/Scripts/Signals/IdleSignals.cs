using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals:MonoSingleton<IdleSignals>
    {
        public UnityAction<bool> onIteractionBuild = delegate {  };
        public UnityAction<int> onBuildStateComplite = delegate {  };
    }
}