using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour, IVehicle
{
    private const float KMH = 2.23694f;
    public Action OnErrorReset;
    public float accelInput;
    public float steerInput;
    public float brakeInput;
    public bool boostButton;
    public float Speed;
    public float VelocityLimit;
    [Header("最大速度")]
    public float VelocityMax = 60f;
    public AnimationCurve VelocityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public float ForwardSteerAngleMax = 60f;
    public AnimationCurve SteerCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public float BackwardSteerAngleMax = 260f;
    private Rigidbody rb;
    private float _velocity;
    private float accTime;
    private Vector3 _pos;
    private RaycastHit[] _hits;
    public float _idleTime;
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _hits = new RaycastHit[2];
        VelocityLimit = 0f;
        audioSource = GetComponent<AudioSource>();
    }

    public void ApplyJumpByArea(JumpArea value)
    {
        rb.AddForce(Vector3.up * value.ForceValue, ForceMode.Impulse);
    }
    public void ApplyBoostByArea(BoostArea value)
    {

    }

    public void ApplyHitByArea(HitArea value)
    {
    }

    public void ApplyHeavyHit(float time = 2f)
    {
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        bool hitGround = false;
        if (Physics.RaycastNonAlloc(transform.position, -transform.up, _hits, 1f) > 0)
        {
            hitGround = true;
        }
        var acc = accelInput;
        if (!hitGround)
        {
            acc = 0f;
        }
        var maxv = VelocityMax / KMH;
        if (acc > 0f)
        {
            accTime += deltaTime;
            var curveValue = VelocityCurve.Evaluate(accTime);
            _velocity += curveValue * 15f * deltaTime * acc;
            _velocity = Mathf.Min(maxv, _velocity);
        }
        else if (acc < 0f)
        {
            accTime = 0f;
            _velocity -= 10f * deltaTime;
            _velocity = Mathf.Max(-10f, _velocity);
        }
        else
        {
            accTime = 0f;
            if (hitGround)
            {
                _velocity = Mathf.Lerp(_velocity, 0f, deltaTime * 1f);
            }
            else
            {
                _velocity = Mathf.Lerp(_velocity, 0f, deltaTime * 0.1f);
            }
        }
        var updot = Vector3.Dot(transform.up, Vector3.up);
        if (updot < 0.95f)
        {
            var y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, y, 0f), 0.1f);
        }
        if (VelocityLimit > 0f)
        {
            _velocity = Mathf.Lerp(_velocity, VelocityLimit, deltaTime);
        }
        var v = _velocity * transform.forward;
        var rbv = rb.velocity;
        v.y = rbv.y;
        rb.velocity = v;
        var mv = _pos - transform.position;
        _pos = transform.position;
        Speed = mv.magnitude / deltaTime * 2.23694f;
        if (_velocity > 0 && Speed < 5f)
        {
            _idleTime += deltaTime;
            if (_idleTime > 3f)
            {
                rb.Sleep();
                rb.velocity = Vector3.zero;
                OnErrorReset?.Invoke();
                rb.WakeUp();
                _idleTime = 0f;
            }
        }
        else
        {
            _idleTime = 0f;
        }
        var absV = Mathf.Abs(_velocity);
        if (steerInput != 0f && hitGround)
        {
            var v2s = Mathf.Clamp(absV / 3f, 0f, 1f);
            if (_velocity > 0)
            {
                var vp = _velocity / maxv;
                vp = SteerCurve.Evaluate(vp);
                transform.rotation = transform.rotation * Quaternion.Euler(0f, steerInput * deltaTime * ForwardSteerAngleMax * vp * v2s, 0f);
            }
            else if (_velocity < 0f)
            {
                var vp = -_velocity / maxv;
                vp = SteerCurve.Evaluate(vp);
                transform.rotation = transform.rotation * Quaternion.Euler(0f, -steerInput * deltaTime * BackwardSteerAngleMax * vp * v2s, 0f);
            }
        }
        if (audioSource != null)
        {
            var tpitch = 0.8f + _velocity / VelocityMax;
            if (!hitGround)
            {
                tpitch = 2f;
            }
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, tpitch, deltaTime * (hitGround ? 1f : 5f));
        }
    }
}
