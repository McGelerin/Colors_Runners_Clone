using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitController : MonoBehaviour
{
    public GameObject stickman;
    private ParticleSystem _particleSystem;
    public Transform Position;

    void Start()
    {
        _particleSystem = stickman.GetComponent<ParticleSystem>();
        stickman.transform.position = Position.position;
        // Every 2 secs we will emit.
        InvokeRepeating("DoEmit", 2.0f, 2.0f);
    }

    public void DoEmit()
    {
        // Any parameters we assign in emitParams will override the current system's when we call Emit.
        // Here we will override the start color and size.
        var emitParams = new ParticleSystem.EmitParams();
        
        emitParams.startSize = 0.2f;
        _particleSystem.Emit(emitParams, 10);
        _particleSystem.Play(); // Continue normal emissions
    }

    public void Stop()
    {
        _particleSystem.Stop();
    }
}
