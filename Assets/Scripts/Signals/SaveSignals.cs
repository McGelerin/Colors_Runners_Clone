using System;
using System.Collections.Generic;
using Extentions;
using Enums;
using Keys;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class SaveSignals : MonoSingleton<SaveSignals>
    {
        public UnityAction onRunnerSaveData = delegate { };
        public Func<int> onGetRunnerLevelID = delegate { return 0; };


        public UnityAction onIdleSaveData = delegate {  };
        public Func<SaveIdleDataParams> onSaveIdleParams= delegate { return default;};
        public UnityAction<SaveIdleDataParams> onLoadIdleGame = delegate { };
        public UnityAction onLoadIdle = delegate {  };
        //public Func<SaveIdleDataParams> onIdleLoad = delegate { return default;};

    }
}