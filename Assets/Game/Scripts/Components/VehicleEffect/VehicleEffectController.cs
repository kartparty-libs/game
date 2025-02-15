using System;
using UnityEngine;

public class VehicleEffectController : MonoBehaviour, IVehicleEffect
{
    Transform CachedTransform;

    public VehicleEffectConfig vehicleEffectConfig;

    AudioSource audioSource;

    void Start()
    {
        CachedTransform = transform;
        audioSource = vehicleEffectConfig.GetComponent<AudioSource>();

        vehicleEffectConfig.jumpEffect?.CachedTransform.SetParent(null);
        vehicleEffectConfig.collisionEffect?.CachedTransform.SetParent(null);
    }

    void IVehicleEffect.OnAccelerate()
    {
        if (vehicleEffectConfig.accelerationEffect == null)
            return;

        vehicleEffectConfig.accelerationEffect.Play();
        SoundPlayer.Play(audioSource, vehicleEffectConfig.accelerationSound);
    }

    void IVehicleEffect.OnAccelerateByGate()
    {
        if (vehicleEffectConfig.accelerationByGateEffect == null)
            return;

        vehicleEffectConfig.accelerationByGateEffect.Play();
    }

    void IVehicleEffect.OnJump()
    {
        if (vehicleEffectConfig.jumpEffect == null)
            return;

        vehicleEffectConfig.jumpEffect.CachedTransform.position = CachedTransform.position;
        vehicleEffectConfig.jumpEffect.Play();
        SoundPlayer.Play(audioSource, vehicleEffectConfig.jumpSound);
    }

    public void OnJumpEnd()
    {
        if (vehicleEffectConfig.jumpEndEffect == null)
            return;

        vehicleEffectConfig.jumpEndEffect.CachedTransform.position = CachedTransform.position;
        vehicleEffectConfig.jumpEndEffect.Play();
    }

    void IVehicleEffect.ShowTail()
    {
        if (vehicleEffectConfig.tailEffect == null)
            return;

        vehicleEffectConfig.tailEffect.Play();
    }

    void IVehicleEffect.HideTail()
    {
        if (vehicleEffectConfig.tailEffect == null)
            return;

        vehicleEffectConfig.tailEffect.Stop();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(LayerNames.Traps) && collision.gameObject.layer != LayerMask.NameToLayer(LayerNames.Car))
            return;

        if (collision.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            return;

        // 处理机关陷阱
        // ..

        if (vehicleEffectConfig.collisionEffect == null)
            return;

        vehicleEffectConfig.collisionEffect.CachedTransform.position = collision.GetContact(0).point;
        vehicleEffectConfig.collisionEffect.Play();
        SoundPlayer.Play(audioSource, vehicleEffectConfig.collisionSound);
    }

    void IVehicleEffect.Peng()
    {
        SoundPlayer.Play(audioSource, vehicleEffectConfig.collisionSound);
    }

}
