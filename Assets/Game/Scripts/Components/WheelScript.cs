using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Video;

public enum WheelType
{
    FrontWeheel_L,
    FrontWeheel_R,
    BackWeheel_L,
    BackWeheel_R,
}

public class WheelScript : MonoBehaviour
{
    public WheelType m_WheelType = WheelType.FrontWeheel_L;
    public float m_WheelRadius = 0.3f;
    public float m_WheelMaxAngle = 45f;

    public BaseCharacter m_Player { get; set; }

    void Update()
    {
        if (this.m_Player != null)
        {
            float velocity = this.m_Player.CharacterController.velocity.magnitude;
            if (velocity > 0)
            {
                this.transform.GetChild(0).Rotate(velocity * 5, 0, 0, Space.Self);
            }
            if (this.m_WheelType == WheelType.FrontWeheel_R || this.m_WheelType == WheelType.FrontWeheel_L)
            {
                if (this.m_Player is MainCharacter mainCharacter)
                {
                    if (mainCharacter.CharacterMovementController.m_VelocityMagnitude > 0)
                    {
                        if (mainCharacter.CharacterMovementController.m_RotateCrossDir.y > 0)
                        {
                            this.transform.localEulerAngles = new Vector3(0, 0, -30 * (this.m_WheelType == WheelType.FrontWeheel_L ? 1 : -1));
                        }
                        else if (mainCharacter.CharacterMovementController.m_RotateCrossDir.y < 0)
                        {
                            this.transform.localEulerAngles = new Vector3(0, 0, 30 * (this.m_WheelType == WheelType.FrontWeheel_L ? 1 : -1));
                        }
                    }
                    else
                    {
                        this.transform.localEulerAngles = Vector3.zero;
                    }
                }
            }
        }
    }
}
