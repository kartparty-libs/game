using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    public Waypoint TargetWaypoint;
    private Vehicle vehicle;
    public float lookDot;
    public float steerDot;
    private Waypoint prevWaypoint;
    private Vector3 TargetPos;
    private float lineLength;
    void Start()
    {
        vehicle = GetComponent<Vehicle>();
        lineLength = 0f;
        if (TargetWaypoint != null)
        {
            TargetPos = TargetWaypoint.transform.position + Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0) * Vector3.forward;

        }
        vehicle.OnErrorReset = vehicleReset;
    }
    private void vehicleReset()
    {
        if (prevWaypoint != null)
        {
            vehicle.transform.position = prevWaypoint.transform.position;
            vehicle.transform.rotation = Quaternion.LookRotation((TargetPos - vehicle.transform.position).normalized);
        }
        else if (TargetWaypoint != null)
        {
            vehicle.transform.position = TargetPos - TargetWaypoint.transform.forward * TargetWaypoint.radius;
            vehicle.transform.rotation = TargetWaypoint.transform.rotation;
        }
        else
        {

        }


    }
    private void FixedUpdate()
    {
        if (vehicle == null)
        {
            return;
        }
        float steerValue = 0f;
        float accelValue = 0f;
        if (TargetWaypoint != null)
        {
            var dis = (TargetWaypoint.transform.position - vehicle.transform.position);
            var dir = dis.normalized;
            lookDot = Vector3.Dot(vehicle.transform.forward, dir);
            steerDot = Vector3.Dot(vehicle.transform.right, dir);
            float distanceToTargetSqr = dis.sqrMagnitude;
            if (distanceToTargetSqr < TargetWaypoint.radius * TargetWaypoint.radius * 4f)
            {
                prevWaypoint = TargetWaypoint;
                var r = UnityEngine.Random.Range(0, TargetWaypoint.NextPoint.Count);
                TargetWaypoint = TargetWaypoint.NextPoint[r];
                lineLength = (TargetWaypoint.transform.position - prevWaypoint.transform.position).magnitude + TargetWaypoint.radius + prevWaypoint.radius;
                TargetPos = TargetWaypoint.transform.position + Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0) * Vector3.forward * TargetWaypoint.radius * 0.5f;
            }

            var nextDot = Vector3.Dot(vehicle.transform.forward, TargetWaypoint.transform.forward);
            steerValue = steerDot;
            accelValue = lookDot * 0.7f + nextDot * 0.3f;

            vehicle.VelocityLimit = 0f;

            if (lookDot < 0.9f)
            {
                vehicle.VelocityLimit = 20f * (1f + lookDot) * 0.5f;
            }
            if (accelValue < 0f)
            {
                accelValue = 0.02f;
                steerValue += accelValue;
            }
            if (lookDot < 0f)
            {
                steerValue = steerDot * 2f;
            }
            if (lineLength > 0f)
            {
                if (distanceToTargetSqr > lineLength * lineLength)
                {
                    vehicle.VelocityLimit = 0.1f;
                }
            }

        }
        vehicle.steerInput = steerValue;
        vehicle.accelInput = accelValue;
    }

    void OnDrawGizmos()
    {
        // Visualize waypoint
        if (TargetWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, TargetPos + Vector3.up * 0.5f);
        }
    }
}
