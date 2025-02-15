using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 网络角色类
    /// </summary>
    public partial class NetWorkCharacter : BaseCharacter, ICharacterNetWorkReceiver
    {
        public CharacterNetWorkReceiverController CharacterNetWorkReceiverController { get; private set; }
        public CharacterNetWorkReceiverController GetCharacterNetWorkReceiverController() => CharacterNetWorkReceiverController;

        public CharacterMovementController CharacterMovementController { get; private set; }


        // -----------------------------------------------------------------------------------------------------------------------------
        // private Vector3 m_MovementVelocity = Vector3.zero;
        public bool fakeMove;
        protected override void Awake()
        {
            base.Awake();

            this.CharacterNetWorkReceiverController = this.transform.gameObject.GetComponent<CharacterNetWorkReceiverController>();
            if (this.CharacterNetWorkReceiverController == null) this.CharacterNetWorkReceiverController = this.transform.gameObject.AddComponent<CharacterNetWorkReceiverController>();
            this.CharacterNetWorkReceiverController.m_Character = this;

            this.CharacterMovementController = this.transform.gameObject.GetComponent<CharacterMovementController>();
            if (this.CharacterMovementController == null) this.CharacterMovementController = this.transform.gameObject.AddComponent<CharacterMovementController>();
            this.CharacterMovementController.m_Character = this;
            CharacterMovementController.useGravity = false;
        }

        protected override void Update()
        {
            base.Update();
            fakeMove = Vector3.SqrMagnitude(this.BuffContext.Velocity) > 0f;
            /*
            if (fakeMove)
            {
                this.CharacterController.Move(this.m_MovementVelocity * Time.deltaTime);
            }
            m_MovementVelocity = Vector3.zero;
            if (this.m_VelocityBuffs.Count > 0)
            {
                for (int i = this.m_VelocityBuffs.Count - 1; i >= 0; i--)
                {
                    float VelocityValue = this.m_VelocityBuffs[i].VelocityValue * this.m_VelocityBuffs[i].Timer / this.m_VelocityBuffs[i].Time;
                    this.m_MovementVelocity += this.m_VelocityBuffs[i].VelocityDirection * VelocityValue;
                    this.m_VelocityBuffs[i].Timer -= Time.deltaTime;
                    fakeMove = true;
                    if (this.m_VelocityBuffs[i].Timer <= 0)
                    {
                        this.m_VelocityBuffs.RemoveAt(i);
                    }
                }
                if (m_VelocityBuffs.Count < 1)
                {
                    fakeMove = false;
                }
            }
            */
        }
    }
}