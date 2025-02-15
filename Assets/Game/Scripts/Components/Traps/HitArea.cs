using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    public float Time = 0.7f;
    public float VelocityValue = 10f;
    public bool HeavyHit;

    [HideInInspector]
    public VelocityBuff VelocityBuff;

    public void OnColliderHit(BaseCharacter i_pPlayer, ControllerColliderHit i_ControllerColliderHit)
    {
        // Debug.Log("i_ControllerColliderHit.normal" + i_ControllerColliderHit.normal);
        // VelocityBuff velocityBuff = new VelocityBuff();
        // velocityBuff.Id = this.GetInstanceID();
        // velocityBuff.Time = this.Time;
        // velocityBuff.Timer = this.Time;
        // velocityBuff.VelocityValue = this.VelocityValue;
        // velocityBuff.VelocityDirection = i_ControllerColliderHit.normal.normalized;
        // i_pPlayer.ApplyHitByArea(velocityBuff);
    }

    public void OnCollisionEnter(Collision i_pCollision)
    {
        if(i_pCollision.contactCount <= 0)
        {
            return;
        }

        VelocityBuff = new VelocityBuff();
        VelocityBuff.Id = this.GetInstanceID();
        VelocityBuff.Duration = this.Time;
        VelocityBuff.VelocityValue = this.VelocityValue;
        VelocityBuff.VelocityDirection = -i_pCollision.GetContact(0).normal.normalized;
        IVehicle pPlayer = i_pCollision.transform.GetComponent<IVehicle>();
        if(pPlayer != null){
            pPlayer.ApplyHitByArea(this);
            if(HeavyHit)
            {
                pPlayer.ApplyHeavyHit();
            }
        }
    }
}
