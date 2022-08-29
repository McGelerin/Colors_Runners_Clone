using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitController : MonoBehaviour
{
    public ParticleSystem system;
    public Transform IstenenYer;
    public Material material;
    void Start()
    {
        // A simple particle material with no texture.
        // Create a Particle System.
        var go = new GameObject("Particle System");
        go.transform.Rotate(-90, 0, 0); // Rotate so the system emits upwards.
        go.transform.position = IstenenYer.position;
        system = go.AddComponent<ParticleSystem>();
        go.GetComponent<ParticleSystemRenderer>().material = material;
        //DoEmit();

        // Every 2 seconds we will emit.
        InvokeRepeating("DoEmit", 60f, 60f);
    }

    void DoEmit()
    {
        // Any parameters we assign in emitParams will override the current system's when we call Emit.
        // Here we will override the position. All other parameters will use the behavior defined in the Inspector.
        var emitParams = new ParticleSystem.EmitParams();
        //emitParams.position = new Vector3(-5.0f, 0.0f, 0.0f);
        emitParams.applyShapeToPosition = true;
        system.Emit(emitParams, 10);
    }
}
