using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 主玩家角色接口
    /// </summary>
    public interface IMainCharacter
    {
        /// <summary>
        /// 设置角色线性速度
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector3 velocity);

        /// <summary>
        /// 角色跳跃
        /// </summary>
        public void Jump();

        /// <summary>
        /// 角色冲刺
        /// </summary>
        public void Boost();
    }

    /// <summary>
    /// 主玩家角色类
    /// </summary>
    public partial class MainCharacter : BaseCharacter, IMainCharacter, ICharacterNetWorkSender, ICharacterMovement
    {
        public CharacterCameraController CharacterCameraController { get; private set; }
        public CharacterInputController CharacterInputController { get; private set; }
        public CharacterMovementController CharacterMovementController { get; private set; }
        public CharacterMovementController GetCharacterMovementController() => CharacterMovementController;
        public CharacterNetWorkSenderController CharacterNetWorkSenderController { get; private set; }

        // -----------------------------------------------------------------------------------------------------------------------------

        public override void Init(CharacterCreateData characterData)
        {
            base.Init(characterData);
            GameEntry.Context.FollowPlayer = this;
        }

        // -----------------------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            this.CharacterCameraController = this.transform.gameObject.GetComponent<CharacterCameraController>();
            if (this.CharacterCameraController == null) this.CharacterCameraController = this.transform.gameObject.AddComponent<CharacterCameraController>();
            this.CharacterCameraController.m_Character = this;

            this.CharacterInputController = this.transform.gameObject.GetComponent<CharacterInputController>();
            if (this.CharacterInputController == null) this.CharacterInputController = this.transform.gameObject.AddComponent<CharacterInputController>();
            this.CharacterInputController.m_Character = this;

            this.CharacterMovementController = this.transform.gameObject.GetComponent<CharacterMovementController>();
            if (this.CharacterMovementController == null) this.CharacterMovementController = this.transform.gameObject.AddComponent<CharacterMovementController>();
            this.CharacterMovementController.m_Character = this;

            this.CharacterNetWorkSenderController = this.transform.gameObject.GetComponent<CharacterNetWorkSenderController>();
            if (this.CharacterNetWorkSenderController == null) this.CharacterNetWorkSenderController = this.transform.gameObject.AddComponent<CharacterNetWorkSenderController>();
            this.CharacterNetWorkSenderController.m_Character = this;

            CharacterMovementController.m_MovementSpeed = CarCultivateSystem.Instance.GetCarSpeed();
        }

        protected override void Update()
        {
            base.Update();

            // TODO
            if (m_AudioSource != null)
            {
                var tpitch = 0.8f + this.CharacterMovementController.m_Speed / this.CharacterMovementController.m_MovementSpeed;
                if (!m_IsGround)
                {
                    tpitch = 2f;
                }
                m_AudioSource.pitch = Mathf.Lerp(m_AudioSource.pitch, tpitch, Time.deltaTime * (m_IsGround ? 1f : 5f));
            }
            //this.CharacterAnimatorController.SetBool(AnimatorHashIDs.HitBool, this.m_IsHit);
        }

        // -----------------------------------------------------------------------------------------------------------------------------
        // 接口

        public override Vector3 GetVelocity()
        {
            return this.CharacterMovementController.m_LastMovementVelocity;
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            this.CharacterMovementController.SetVelocity(velocity);
        }

        public virtual void Jump()
        {
            this.CharacterMovementController.Jump();
        }

        public virtual void Boost()
        {
            this.CharacterMovementController.Sprint();
        }

        public override void OnChangeAniTrigger(int hashId)
        {
            this.CharacterNetWorkSenderController.OnChangeAniTrigger(hashId);
        }

        public override void OnChangeAniFloat(int hashId, float value)
        {
            this.CharacterNetWorkSenderController.OnChangeAniFloat(hashId, value);
        }

        public override void OnChangeAniBool(int hashId, bool value)
        {
            this.CharacterNetWorkSenderController.OnChangeAniBool(hashId, value);
        }

        // -----------------------------------------------------------------------------------------------------------------------------
        // 通知

        public override void ApplyJumpByArea(JumpArea value)
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
            {
                return;
            }
            if (!this.m_IsGround)
            {
                return;
            }
            this.AddVelocityBuff(value.VelocityBuff);
            this.CharacterMovementController.Jump();
            this.m_Effectctrl?.OnJump();
        }

        public override void ApplyBoostByArea(BoostArea value)
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
            {
                return;
            }
            this.AddSpeedUpBuff(value.SpeedUpBuff);
            this.m_Effectctrl?.OnAccelerateByGate();
        }

        public override void ApplyHitByArea(HitArea value)
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
            {
                return;
            }
            ApplyHit(value.VelocityBuff);
        }
        public override void ApplyHeavyHit(float time = 2f)
        {
            if (this.m_DizzyTime > 0.5f)
            {
                return;
            }
            this.m_DizzyTime = CarCultivateSystem.Instance.GetDizzyTime();
            this.m_IsDizzy = true;
        }

        public void OnCompleteGame(bool i_bVictory)
        {
            if (i_bVictory)
            {
                this.CharacterAnimatorController.SetTrigger(AnimatorHashIDs.VictoryTrigger);
            }
            this.CharacterInputController.m_Steer = Vector3.zero;
            this.CharacterMovementController.SetVelocity(Vector3.zero);
        }
    }
}