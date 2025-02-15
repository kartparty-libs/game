using System;
using UnityEngine;

public class AIOperationBoost : MonoBehaviour
{
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(LayerNames.Car))
            return;

        var acceptor = other.GetComponentInParent<ITrapsAcceptor>();
        acceptor?.ExecuteBoost(this);
    }
}
