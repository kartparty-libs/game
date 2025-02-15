using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public ECheckPointType type;
    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(LayerNames.CheckableObject))
            return;

        // 判断方向
        // ..

        CheckPointManager.Instance.Check(other.gameObject, this);
    }
}
