using System;
using UnityEngine;

public class AIOperationJump : MonoBehaviour
{
    public float Force = 10f;
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
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(LayerNames.Car))
            return;

        var acceptor = other.GetComponentInParent<ITrapsAcceptor>();
        acceptor?.ExecuteJump(this);
    }
}
