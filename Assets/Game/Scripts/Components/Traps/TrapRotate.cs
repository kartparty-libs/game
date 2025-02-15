using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotate : MonoBehaviour
{
    public Transform Target;
    public List<Vector3> Angles = new List<Vector3>();
    [Range(0f, 1f)]
    public float StartProgress = 0f;
    public float Duration = 1f;
    public float StartDelayTime = 0f;
    public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
    private float _waitTime;
    private float _progressTime;
    private bool _enabled;
    private float _secTime;
    void Start()
    {
        if (Target == null)
        {
            Target = this.transform;
        }
        _enabled = Angles.Count > 1;
        _waitTime = StartDelayTime;
        if (Duration <= 0f)
        {
            Duration = 1f;
        }
        _secTime = Duration / (Angles.Count - 1);
        _progressTime = Mathf.Clamp01(StartProgress) * Duration;

        updateByTime(_progressTime);
        var collider = this.GetComponent<Collider>();
        if (collider == null)
        {
            var colliders = this.GetComponentsInChildren<Collider>();
            if (colliders.Length < 1)
            {
                this.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enabled)
        {
            return;
        }
        if (_waitTime > 0f)
        {
            _waitTime -= Time.deltaTime;
            return;
        }
        _progressTime += Time.deltaTime;
        if (_progressTime > Duration)
        {
            _progressTime %= Duration;
        }
        updateByTime(_progressTime);
    }
    private void updateByTime(float time)
    {
        if (time > Duration)
        {
            time %= Duration;
        }
        var secId = Mathf.FloorToInt(time / _secTime);
        var start = Angles[secId];
        var end = Angles[secId + 1];
        var p = (time - secId * _secTime) / _secTime;
        var cp = Curve.Evaluate(p);
        var c = Vector3.Lerp(start, end, cp);
        Target.localRotation = Quaternion.Euler(c);
    }
}
