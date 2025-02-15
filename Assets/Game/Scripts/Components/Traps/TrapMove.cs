using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMove : MonoBehaviour
{
    public Transform Target;
    public List<Vector3> Points = new List<Vector3>();
    [Range(0f, 1f)]
    public float StartProgress = 0f;
    public float Duration = 1f;
    public float StartDelayTime = 0f;
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
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
        _enabled = Points.Count > 1;
        _waitTime = StartDelayTime;
        if (Duration <= 0f)
        {
            Duration = 1f;
        }
        _secTime = Duration / (Points.Count - 1);
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
        updateByTime(_progressTime);
    }
    private void updateByTime(float time)
    {
        if (time > Duration)
        {
            time %= Duration;
        }
        var secId = Mathf.FloorToInt(time / _secTime);
        var start = Points[secId];
        var end = Points[secId + 1];
        var p = (time - secId * _secTime) / _secTime;
        var cp = Curve.Evaluate(p);
        var pos = Vector3.Lerp(start, end, cp);
        Target.localPosition = pos;
    }
    [ContextMenu("Ìí¼Ó")]
    private void addFromCurrent()
    {
        this.StartDelayTime = 0.5f;
        this.Duration = 3f;
        Points.Clear();
        Points.Add(this.transform.localPosition + Vector3.down * 140f);
        Points.Add(this.transform.localPosition);
        Points.Add(this.transform.localPosition + Vector3.down * 140f);
    }
}
