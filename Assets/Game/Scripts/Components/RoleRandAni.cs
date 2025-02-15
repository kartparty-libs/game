using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleRandAni : MonoBehaviour
{
    private float _time;
    private Animator _animator;
    private void Start()
    {
        _animator=GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //if(_time>0f)
        //{
        //    _time -= Time.deltaTime;
        //    if( _time < 0 )
        //    {
        //        playRelaxation();
        //    }
        //}
    }
    private void OnEnable()
    {
        //_time = UnityEngine.Random.Range(1f, 3f);


    }
    public void playRelaxation()
    {
        if (_animator != null)
        {
            _animator.Play("Relaxation", 0);
            var time=Utils.Unity.GetAnimatorStateDuration(_animator, "Relaxation");
            if(time<=0f)
            {
                time = 3f;
            }
            _time = UnityEngine.Random.Range(time, time+5);
        }
    }
}
