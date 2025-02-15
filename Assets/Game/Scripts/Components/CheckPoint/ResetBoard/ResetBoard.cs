using System;
using System.Linq;
using Framework;
using UnityEngine;

public class ResetBoard : MonoBehaviour
{
    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(LayerNames.CheckableObject))
            return;
        CheckPointManager.Instance.ResetTarget(other.gameObject);
    }
}
