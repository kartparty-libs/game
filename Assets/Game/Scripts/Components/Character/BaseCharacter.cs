using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Framework
{
    /// <summary>
    /// 角色接口
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(CharacterCreateData characterData);

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy();

        /// <summary>
        /// 获取角色坐标
        /// </summary>
        public Vector3 GetPosition();

        /// <summary>
        /// 设置角色坐标
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position);

        /// <summary>
        /// 获取角色旋转
        /// </summary>
        public Quaternion GetRotation();

        /// <summary>
        /// 设置角色旋转
        /// </summary>
        /// <param name="position"></param>
        public void SetRotation(Quaternion rotation);

        /// <summary>
        /// 设置角色速度
        /// </summary>
        public Vector3 GetVelocity();

        /// <summary>
        /// 加载外显
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="carId"></param>
        public void LoadSkin(int roleId, int carId);

        /// <summary>
        /// 获取服务器唯一id
        /// </summary>
        /// <returns></returns>
        public long GetServerInstId();

        public CharacterInfo GetCharacterInfo();
        public void AddBuff(BaseBuff value);
    }
    public class BuffContext : IBuffContext
    {
        public Vector3 Velocity;

        public void Clear()
        {
            Velocity = Vector3.zero;
        }
    }
    /// <summary>
    /// 角色基类
    /// </summary>
    public partial class BaseCharacter : MonoBehaviour, ICharacter, IVehicle, ICharacterAnimatorController
    {
        private long ServerInstId;
        public string Name;
        public int Head;
        public CharacterController CharacterController { get; private set; }
        public AudioSource m_AudioSource { get; private set; }
        public IVehicleEffect m_Effectctrl { get; private set; }
        public CharacterAnimatorController CharacterAnimatorController { get; private set; }

        public Transform m_Skin { get; set; }
        public GameObject m_ColliderForCheckpoint { get; private set; }
        public bool m_IsGround = false;
        public bool m_IsGround2 = false;
        public int m_GroundLayer { get; private set; }
        public Vector3 m_GroundNormal = Vector3.zero;
        public float m_CharacterIncludedAngleByGround { get; private set; }

        public List<SpeedUpBuff> m_SpeedUpBuffs = new List<SpeedUpBuff>();
        public List<VelocityBuff> m_VelocityBuffs = new List<VelocityBuff>();
        public BuffContext BuffContext { get; private set; } = new BuffContext();
        private BuffNode BuffNode;
        public bool m_IsDizzy;
        public float m_DizzyTime;
        public bool m_IsHit;
        public float m_HitTime;
        public Vector3 m_RotateCrossDir;
        private CharacterInfo characterInfo = new CharacterInfo();

        // -----------------------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            BuffNode = new BuffNode(BuffContext);
            this.CharacterController = GetComponent<CharacterController>();
            this.m_AudioSource = GetComponent<AudioSource>();
            this.m_Effectctrl = GetComponent<IVehicleEffect>();

            this.CharacterAnimatorController = this.transform.gameObject.GetComponent<CharacterAnimatorController>();
            if (this.CharacterAnimatorController == null) this.CharacterAnimatorController = this.transform.gameObject.AddComponent<CharacterAnimatorController>();
            this.CharacterAnimatorController.m_Character = this;
        }

        protected virtual void Start()
        {
            m_GroundLayer = LayerMask.GetMask(new string[] { LayerNames.Ground, LayerNames.Wall });
            m_ColliderForCheckpoint = CheckPointManager.AddTriggerForCheckableObject(transform);
            var components = transform.Find("Components");
            var len = components.childCount;
            while (len-- > 0)
            {
                var component = components.GetChild(len);
                Utils.Unity.SetActive(component.gameObject, GameEntry.Context.Gameplay.ExecuteComponent(component));
            }

        }
        public CharacterInfo GetCharacterInfo()
        {
            characterInfo.RoleId = this.ServerInstId;
            characterInfo.Name = this.Name;
            characterInfo.HeadIconId = Head;
            characterInfo.Rank = 100;
            var status = CheckPointManager.Instance.GetStatus(m_ColliderForCheckpoint);
            if (status != null)
            {
                characterInfo.Rank = status.rank;
                m_ColliderForCheckpoint.name = this.Name;
            }
            return characterInfo;
        }
        protected virtual void Update()
        {
            BuffNode.Update(Time.deltaTime);
            if (m_IsDizzy)
            {
                m_DizzyTime -= Time.deltaTime;
                if (m_DizzyTime <= 0f)
                {
                    m_IsDizzy = false;
                }
            }
            else if (m_IsHit)
            {
                m_HitTime -= Time.deltaTime;
                if (m_HitTime <= 0f)
                {
                    m_IsHit = false;
                }
            }
            if (Physics.SphereCast(this.transform.position + Vector3.up * this.CharacterController.radius, this.CharacterController.radius, Vector3.down, out RaycastHit hit, 2 * this.CharacterController.skinWidth, this.m_GroundLayer))
            {
                this.m_IsGround = true;
                this.m_GroundNormal = hit.normal;
                this.m_CharacterIncludedAngleByGround = Vector3.Angle(this.transform.forward, hit.normal);

                // Vector3 project = Vector3.Project(this.m_GroundNormal, this.transform.forward);
                // Debug.DrawLine(hit.point, hit.point + hit.normal * 3f, Color.blue);
                // Debug.DrawLine(hit.point, hit.point + project * 3f, Color.green);
            }
            else
            {
                this.m_IsGround = false;
                if (this.m_CharacterIncludedAngleByGround != 90)
                {
                    this.m_CharacterIncludedAngleByGround = Mathf.Lerp(this.m_CharacterIncludedAngleByGround, 90, Time.deltaTime);
                    if (Mathf.Abs(this.m_CharacterIncludedAngleByGround - 90) < 0.1f) this.m_CharacterIncludedAngleByGround = 90;
                }
                this.m_GroundNormal.Set(0, 1, 0);
            }

            this.m_IsGround2 = Physics.SphereCast(this.transform.position + Vector3.up * this.CharacterController.radius, this.CharacterController.radius, Vector3.down, out RaycastHit airhit, 0.5f, this.m_GroundLayer);
            this.CharacterAnimatorController.SetBool(AnimatorHashIDs.DizzyBool, this.m_IsDizzy);
            this.CharacterAnimatorController.SetBool(AnimatorHashIDs.GroundBool, this.m_IsGround2);
            // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3f, Color.red);
            // Debug.DrawLine(this.transform.position, this.transform.position + this.m_MovementDirection * 3f, Color.yellow);
        }

        protected virtual void LateUpdate()
        {
        }

        // -----------------------------------------------------------------------------------------------------------------------------

        public virtual void Init(CharacterCreateData characterData)
        {
            this.ServerInstId = characterData.uid;
            this.SetPosition(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).position);
            this.SetRotation(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).rotation);
            this.LoadSkin(characterData.characterRoleCfgId, characterData.characterCarCfgId);
            this.Name = characterData.playerName;
            this.Head = characterData.head;
        }

        public virtual void Destroy()
        {
            Destroy(this);
        }

        public virtual Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public virtual void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        public virtual Quaternion GetRotation()
        {
            return this.transform.rotation;
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            this.transform.rotation = rotation;
        }

        public virtual Vector3 GetVelocity()
        {
            return this.CharacterController.velocity;
        }

        public virtual long GetServerInstId()
        {
            return ServerInstId;
        }

        public virtual void OnChangeAniTrigger(int hashId) { }
        public virtual void OnChangeAniFloat(int hashId, float value) { }
        public virtual void OnChangeAniBool(int hashId, bool value) { }

        // -----------------------------------------------------------------------------------------------------------------------------
        // Buff

        public virtual void AddSpeedUpBuff(SpeedUpBuff speedUpBuff)
        {
            foreach (var buff in this.m_SpeedUpBuffs)
            {
                if (buff.Id == speedUpBuff.Id) return;
            }
            this.m_SpeedUpBuffs.Add(speedUpBuff);
        }

        public virtual bool AddVelocityBuff(VelocityBuff velocityBuff)
        {
            return BuffNode.Add(velocityBuff);
        }
        public void ApplyHit(VelocityBuff VelocityBuff)
        {
            if (this.AddVelocityBuff(VelocityBuff))
            {
                if (!this.m_IsHit)
                {
                    this.m_HitTime = 1f;
                    this.m_IsHit = true;
                    this.CharacterAnimatorController.SetTrigger(AnimatorHashIDs.HitTrigger);
                }
            }
        }
        // -----------------------------------------------------------------------------------------------------------------------------
        // 通知

        public virtual void ApplyJumpByArea(JumpArea value)
        {
        }

        public virtual void ApplyBoostByArea(BoostArea value)
        {
        }

        public virtual void ApplyHitByArea(HitArea value)
        {
        }

        public virtual void ApplyHeavyHit(float time = 2f)
        {
        }
        public void AddBuff(BaseBuff value)
        {
            if (value.BuffType == EnumDefine.BuffType.VelocityBuff)
            {
                if (value is VelocityBuff buff)
                {
                    AddVelocityBuff(buff);
                }
            }
        }
    }
    public partial class BaseCharacter
    {
        private Transform BoneNode;
        private Transform NormalNode;
        private Transform RoleTransform;
        public void LoadSkin(int roleId, int carId)
        {
            StartCoroutine(loadSkin(roleId, carId));
        }
        IEnumerator loadSkin(int roleId, int carId)
        {
            if (m_Skin != null)
            {
                m_Skin.gameObject.SetActive(false);
            }
            yield return null;
            var skin = new GameObject("playerSkin");
            m_Skin = skin.transform;
            m_Skin.parent = this.transform;
            m_Skin.localPosition = Vector3.zero;
            m_Skin.localRotation = Quaternion.identity;
            m_Skin.localScale = Vector3.one;

            var role = GameEntry.Table.Role.Get(roleId);
            var car = GameEntry.Table.Car.Get(carId);

            var result = GameEntry.AssetsLoader.LoadAsset(car.Asset);
            result.Result.IsGameObject = true;
            yield return result;
            if (result.Asset is GameObject go_car)
            {
                go_car.transform.SetParent(m_Skin, false);
                this.CharacterAnimatorController._animatorCar = go_car.GetComponent<Animator>();
                WheelScript[] wheelScripts = go_car.GetComponentsInChildren<WheelScript>();
                if (wheelScripts.Length > 0)
                {
                    foreach (var item in wheelScripts)
                    {
                        item.m_Player = this;
                    }
                }
                BoneNode = this.CharacterAnimatorController._animatorCar.transform.Find("car_all/car_am/car_Bone003/role");
                NormalNode = this.CharacterAnimatorController._animatorCar.transform;
            }
            result = GameEntry.AssetsLoader.LoadAsset(role.Asset);
            result.Result.IsGameObject = true;
            yield return result;
            if (result.Asset is GameObject go_role)
            {
                RoleTransform = go_role.transform;
                RoleTransform.SetParent(BoneNode, false);
                this.CharacterAnimatorController._animatorRole = go_role.GetComponent<Animator>();
            }
        }
        public void EngineShutdown()
        {
            m_AudioSource.enabled = false;
        }
    }
}