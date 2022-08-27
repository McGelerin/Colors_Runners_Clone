using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Signals;
using System;

public class GameStateChangerTest : MonoBehaviour
{
    void Start()
    {
    }
    #region Event Subscriptions

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
   
        CoreGameSignals.Instance.onPlay += OnChangeGameState;

    }

    

    private void UnsubscribeEvents()
    {
        CoreGameSignals.Instance.onPlay -= OnChangeGameState;

    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion

    private void OnChangeGameState()
    {
        CoreGameSignals.Instance.onChangeGameState?.Invoke();
    }
}
