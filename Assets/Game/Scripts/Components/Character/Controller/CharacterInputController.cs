using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色输入控制器
    /// </summary>
    public class CharacterInputController : MonoBehaviour
    {
        public MainCharacter m_Character;
        public Vector3 m_Steer = Vector3.zero;
        private PlayerInput m_Input;

        void Update()
        {
            var deltaTime = Time.deltaTime;
            if (this.m_Input != null)
            {
                this.m_Input.Update(deltaTime);
                while (true)
                {
                    var action = this.m_Input.Read();
                    if (action == null)
                    {
                        break;
                    }
                    if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
                    {
                        continue;
                    }
                    if (action.Name == PlayerInput.Steer)
                    {
                        if (action.Phase == GameDeviceActionPhase.Performed)
                        {
                            this.m_Steer.Set(action.DataValue2.x, 0, action.DataValue2.y);
                        }
                        else if (action.Phase == GameDeviceActionPhase.Canceled)
                        {
                            this.m_Steer = Vector3.zero;
                        }
                    }
                    else if (action.Name == PlayerInput.Jump)
                    {
                        if (action.Phase == GameDeviceActionPhase.Started)
                        {
                            this.m_Character.Jump();
                        }
                    }
                }
            }
            this.m_Character.SetVelocity(this.m_Steer);
        }

        public void SetInput(PlayerInput value)
        {
            this.m_Input = value;
            this.m_Input.SetEnabled(true);
        }
    }
}