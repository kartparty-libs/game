using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using UnityEngine;

public class CollectMoneyBeat : MonoBehaviour
{
    // Start is called before the first frame update
    private float _time;
    private float _scale;
    private int _count;
    private bool _playing;
    private bool _reset;
    void Start()
    {
        _time = 0f;
        _playing = false;
    }
    public void Show(float scale)
    {
        if (_playing)
        {
            return;
        }
        _playing = true;
        _scale = scale;
        this.transform.localScale = Vector3.one * _scale;
        _time = 0.08f;
        _count = 5;
        _reset = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_playing)
        {
            return;
        }
        if (_count > 0)
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                if (_time <= 0f)
                {
                    this.transform.localScale = _reset ? Vector3.one : Vector3.one * _scale;
                    _reset = !_reset;
                    _time = 0.08f;
                    _count--;
                    if (_count < 1)
                    {
                        this.transform.localScale = Vector3.one;
                        _playing = false;
                    }
                }
            }
        }

    }
}
