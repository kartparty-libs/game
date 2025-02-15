using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea : MonoBehaviour
{
    public float ForceValue = 1f;
    public float Time = 0.7f;
    public float VelocityValue = 5f;
    public Vector3 VelocityDirection = Vector3.up;

    [HideInInspector]
    public VelocityBuff VelocityBuff;
    private void Start()
    {
        var colliders = GetComponents<Collider>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }
        }
        colliders = GetComponentsInChildren<Collider>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }
        }
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        this.VelocityBuff = new VelocityBuff();
        this.VelocityBuff.Id = this.GetInstanceID();
        this.VelocityBuff.Duration = this.Time;
        this.VelocityBuff.VelocityValue = this.VelocityValue;
        this.VelocityBuff.VelocityDirection = this.VelocityDirection;
        IVehicle vehicle = other.transform.GetComponent<IVehicle>();
        if(vehicle != null){
            vehicle.ApplyJumpByArea(this);
        }
    }
}
