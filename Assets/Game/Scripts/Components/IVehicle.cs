using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicle
{
    void ApplyBoostByArea(BoostArea value);
    void ApplyJumpByArea(JumpArea value);
    void ApplyHitByArea(HitArea value);
    void ApplyHeavyHit(float time = 2f);
}
