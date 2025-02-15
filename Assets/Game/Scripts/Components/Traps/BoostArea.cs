using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    public float ForceValue = 10f;
    public ForceMode Mode;
    public float ForceTime;

    public float SpeedUpValue = 5f;
    public float SpeedUpTime = 1.5f;
    public SpeedUpBuff SpeedUpBuff;
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
    private void OnTriggerEnter(Collider other)
    {
        if (ForceTime <= 0f)
        {
            return;
        }
        var vehicle = other.GetComponent<IVehicle>();
        if (vehicle != null)
        {
            vehicle.ApplyBoostByArea(this);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (ForceTime > 0f)
        {
            return;
        }
        var vehicle = other.GetComponent<IVehicle>();
        if (vehicle != null)
        {
            SpeedUpBuff = new SpeedUpBuff();
            SpeedUpBuff.SpeedUpValue = this.SpeedUpValue;
            SpeedUpBuff.Time = this.SpeedUpTime;
            SpeedUpBuff.Timer = this.SpeedUpTime;
            vehicle.ApplyBoostByArea(this);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position + this.transform.up, this.transform.position + this.transform.up + this.transform.forward);
    }
}
