using System;
using UnityEngine;

public class EffectConfig : MonoBehaviour
{
    public Transform CachedTransform { get; private set; }

    public float origin_delay;

    public float origin_aliveTime;

    public float delay;

    public float aliveTime;

    ParticleSystem[] psArray;

    public bool playOnStart;

    void Awake()
    {
        CachedTransform = transform;
    }

    void Start()
    {
        psArray = GetComponentsInChildren<ParticleSystem>();

        delay = origin_delay;
        aliveTime = origin_aliveTime;

        if (!playOnStart)
            Stop();
    }

    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        if (aliveTime > 0)
        {
            aliveTime -= Time.deltaTime;
            if (aliveTime <= 0)
            {
                Stop();
            }
        }
    }

    public void Play()
    {
        delay = origin_delay;
        aliveTime = origin_aliveTime;

        foreach (var ps in psArray)
        {
            ps.Play();
        }
    }

    public void Stop()
    {
        foreach (var ps in psArray)
        {
            ps.Stop();
            ps.Clear();
        }
    }
}
