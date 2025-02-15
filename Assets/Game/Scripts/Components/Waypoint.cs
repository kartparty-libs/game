using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Waypoint : MonoBehaviour
{
    public float radius = 10;
    public float Speed = 1f;
    public List<Waypoint> NextPoint;
    void OnDrawGizmos()
    {
        // Visualize waypoint
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        if(NextPoint != null )
        {
            foreach (var waypoint in NextPoint)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position+Vector3.up*0.5f, waypoint.transform.position + Vector3.up * 0.5f);
            }
        }
    }
}
