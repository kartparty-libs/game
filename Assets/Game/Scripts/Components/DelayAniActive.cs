using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DelayAniActive : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    public float DelayTime;
    IEnumerator Start()
    {
        if(DelayTime>0f)
        {
            Target.SetActive(false);
            yield return new WaitForSeconds(DelayTime);
        }
        Target.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position+this.transform.up, this.transform.position + this.transform.up + this.transform.forward);
    }
}
