using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipBubble : MonoBehaviour
{
    public GameObject Target;
    public float Interval = 3f;
    public float ShowTime = 2f;
    private float _time;
    private float _endTime;
    private bool _enable;
    private void Awake()
    {
        Target?.SetActive(false);
        _enable = Target != null;
        _time = 0;
        _endTime = Interval;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_enable)
        {
            return;
        }
        _time += Time.deltaTime;
        if (_time > _endTime)
        {
            _time = 0;
            if (!Target.activeSelf)
            {
                _endTime = ShowTime;
                Target.SetActive(true);
            }
            else
            {
                _endTime = Interval;
                Target.SetActive(false);
            }
        }
    }
}
