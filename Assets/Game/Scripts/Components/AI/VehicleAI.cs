using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework
{
    public class VehicleAI : MonoBehaviour, ICharacter, ICheckableObject, ITrapsAcceptor, IVehicle, ICharacterNetWorkSender, ICharacterAnimatorController
    {
        public const string SpeedEffectValue = nameof(SpeedEffectValue);
        private long ServerInstId;
        public string Name;
        public int Head;
        public int Id;

        public CharacterAnimatorController CharacterAnimatorController { get; private set; }
        public CharacterNetWorkSenderController CharacterNetWorkSenderController { get; private set; }

        public Transform m_Skin { get; set; }
        public int lineIndex;
        LineRenderer lineRenderer;

        IVehicleEffect vehicleEffect;

        public float runDelay;

        public float checkOffect = 1f;
        public float raycastDistance => checkOffect + 1.2f; // 射线的长度
        public LayerMask m_GroundLayer; // 地面的层级
        public bool m_IsGround;
        public bool m_IsGround2;

        public float speed = 20f;
        public float rotationSpeed = 8;
        Quaternion currentRot;

        public int currentSegment = 0;
        private float t = 0.0f;
        public Rigidbody rb;
        public CapsuleCollider capsuleCollider;

        Transform self;
        public Transform model;
        private List<ForceBuff> _forces = new List<ForceBuff>();
        GameObject ColliderForCheckpoint;
        public int lastCheckPointIndex = -1;
        public int laps;

        public float rankPoint;
        public int rank;

        int targetLoopCount;

        float boostCooldown;

        bool MatchFinished => laps >= targetLoopCount;
        bool raceFinished = false;
        private CharacterInfo characterInfo = new CharacterInfo();
        private bool m_IsJump = false;
        private float m_JumpTime = 0;
        public bool m_IsDizzy;
        public float m_DizzyTime;

        // -----------------------------------------------------------------------------------------------------------------------------
        private const float checkErrorInterval = 3f;
        private float checkErrorTime;
        private Vector3 lastPosition;
        public float maxSpeed;
        protected virtual void Awake()
        {
            this.rb = this.transform.gameObject.GetComponent<Rigidbody>();
            this.capsuleCollider = this.transform.gameObject.GetComponent<CapsuleCollider>();

            this.CharacterAnimatorController = this.transform.gameObject.GetComponent<CharacterAnimatorController>();
            if (this.CharacterAnimatorController == null) this.CharacterAnimatorController = this.transform.gameObject.AddComponent<CharacterAnimatorController>();
            this.CharacterAnimatorController.m_Character = this;

            this.CharacterNetWorkSenderController = this.transform.gameObject.GetComponent<CharacterNetWorkSenderController>();
            if (this.CharacterNetWorkSenderController == null) this.CharacterNetWorkSenderController = this.transform.gameObject.AddComponent<CharacterNetWorkSenderController>();
            this.CharacterNetWorkSenderController.m_Character = this;
            lastPosition = transform.position;
            var scene = GameEntry.Context.SceneConfig;
            if (scene != null)
            {
                this.speed = Random.Range(scene.AISpeedMin, scene.AISpeedMax);
                if(GameEntry.Context.MatchMode)
                {
                    this.speed=23f;
                }
            }
            else
            {
                this.speed = Random.Range(19f, 23f);
            }

        }

        void Start()
        {
            self = transform;
            m_GroundLayer = LayerMask.GetMask(new string[] { LayerNames.Ground, LayerNames.Wall });
            if (AIPathManager.inst != null)
            {
                lineRenderer = AIPathManager.inst.GetNextLine();
            }

            vehicleEffect = GetComponent<IVehicleEffect>();

            // StartCoroutine(loadSkin());

            Id = IdGenerator.GenerateId();

            ColliderForCheckpoint = CheckPointManager.AddTriggerForCheckableObject(self);

            var data = GameEntry.Table.Map.Get(GameEntry.Context.SelectMapId);
            targetLoopCount = data.Loop;

            StartCoroutine(AutoBoost());
            checkErrorTime = 0f;
            if (GameEntry.Context.Gameplay is GameplayRace pve)
            {
                if (GameEntry.Context.MatchMode)
                {
                    maxSpeed = MatchSystem.Instance.GetAiSpeed(characterData.RankTiersId);
                }
                else
                {
                    maxSpeed = pve.GetAIMaxSpeed();
                }
            }
            else
            {
                var scene = GameEntry.Context.SceneConfig;
                maxSpeed = Random.Range(scene.AISpeedMin, scene.AISpeedMax);
            }
        }
        void Update()
        {
            // CorrectingRotationError();

            // 朝着切线方向平滑旋转物体
            // Quaternion targetRotation = Quaternion.LookRotation(self.forward, Vector3.up);
            // currentRot = Quaternion.Slerp(model.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            // model.rotation = currentRot;
            Vector3 pPos = this.transform.position + Vector3.up * 1;
            m_IsGround = Physics.SphereCast(pPos, this.capsuleCollider.radius, Vector3.down, out RaycastHit hit, 1.3f - this.capsuleCollider.radius, this.m_GroundLayer);
            m_IsGround2 = Physics.SphereCast(pPos, this.capsuleCollider.radius, Vector3.down, out RaycastHit airhit, 1.8f - this.capsuleCollider.radius, this.m_GroundLayer);

            m_JumpTime -= Time.deltaTime;
            if (m_IsGround && m_IsJump && m_JumpTime <= 0)
            {
                this.m_IsJump = false;
            }

            var status = CheckPointManager.Instance.GetStatus(ColliderForCheckpoint);
            if (status != null)
            {
                if (status.checkPointIndexList.Count > 0)
                {
                    lastCheckPointIndex = status.checkPointIndexList.Last();
                }
                laps = status.lapsCompleted;
                rankPoint = status.progress;
                rank = status.rank;
                if (!raceFinished)
                {
                    if (GameEntry.Context.Gameplay.Playing)
                    {
                        var dis = Vector3.Distance(lastPosition, this.transform.position);
                        if (dis > 1f)
                        {
                            checkErrorTime = 0f;
                            lastPosition = this.transform.position;
                        }
                        else
                        {
                            checkErrorTime += Time.deltaTime;
                            if (checkErrorTime > checkErrorInterval)
                            {
                                checkErrorTime = 0f;
                                CheckPointManager.Instance.ResetTarget(this.ColliderForCheckpoint);
                            }
                        }
                    }
                    if (status.lapsCompleted >= targetLoopCount)
                    {
                        raceFinished = true;
                        _forces.Clear();
                        // this.CharacterAnimatorController.SetBool(AnimatorHashIDs.SprintBool, false);
                        this.CharacterAnimatorController.SetTrigger(AnimatorHashIDs.VictoryTrigger);
                        MatchSystem.Instance.PlayerFinishRace(this.characterInfo.RoleId, GameEntry.Context.Gameplay.RaceTime, true);
                        // if (!GameEntry.Context.OfflineMode)
                        // {
                        //     GameEntry.Net.Send(CtoS.K_PlayerCompleteGame, 0, this.ServerInstId, this.Name);
                        // }
                        // else
                        // {
                        //     GameEntry.OfflineManager.PlayerFinish(this.characterInfo.RoleId, 800000);
                        // }

                    }
                }
            }
            boostCooldown -= Time.deltaTime;
        }


        void FixedUpdate()
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.Playing)
                return;

            if (MatchFinished)
            {
                return;
            }

            if (m_IsDizzy)
            {
                m_DizzyTime -= Time.deltaTime;
                if (m_DizzyTime <= 0f)
                {
                    m_IsDizzy = false;
                }
                return;
            }

            if (runDelay > 0)
            {
                runDelay -= Time.fixedDeltaTime;
                return;
            }
            if (!m_IsGround)
            {
                rb.AddForce(9 * Vector3.down, ForceMode.Acceleration);
            }
            if (lineRenderer == null)
            {
                return;
            }
            var segmentBegin = lineRenderer.GetPosition(currentSegment);
            var segmentEnd = lineRenderer.GetPosition(currentSegment + 1);

            // 获取路径上的点和切线方向

            var myPos = transform.position;
            Vector3 position = GetCatmullRomPoint(currentSegment, t);
            Vector3 direction = (segmentEnd - myPos).normalized;
            Vector3 directionCurve = GetCatmullRomTangent(currentSegment, t);

            // 将物体移动到路径上的点
            // rb.MovePosition(position);

            // 朝着切线方向旋转物体
            // rb.MoveRotation(Quaternion.LookRotation(directionCurve, Vector3.up));

            CorrectingRotationError();

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            self.rotation = Quaternion.Slerp(self.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);


            var targetDir = targetRotation * Vector3.forward;
            var nowDir = self.rotation * Vector3.forward;
            Vector3 pRotateCrossDir = Vector3.Cross(targetDir, nowDir); // y > 0 左转  y < 0 右转
            this.CharacterAnimatorController.SetFloat(AnimatorHashIDs.SteerFloat, Mathf.Abs(pRotateCrossDir.y) < 0.1 ? 0 : pRotateCrossDir.y);

            var force = direction.ResetY().normalized * speed * 4f;

            // 修正向前力
            var forwardFactor = Vector3.Dot(self.forward, direction);
            if (forwardFactor < 0)
                forwardFactor = 0;
            force *= forwardFactor;

            // Debug.DrawRay(myPos, force / 1000, Color.red);
            // 施加力以沿着切线方向移动

            // 空中力减半
            if (!m_IsGround)
            {
                // force *= 0.8f;
            }
            var vspeed = rb.velocity;
            vspeed.y = 0f;
            if (vspeed.magnitude < maxSpeed)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }
            // rb.AddForce(self.forward * speed, ForceMode.Force);

            var pastDistance = Vector3.Distance(segmentBegin, myPos);
            var currentSegmentLength = Vector3.Distance(segmentBegin, segmentEnd);

            t = pastDistance / currentSegmentLength;

            // 检查方向
            var beginToEnd = segmentEnd - segmentBegin;
            var beginToOwner = transform.position - segmentBegin;
            var ensureDirection = Vector3.Dot(beginToEnd, beginToOwner) > 0.5f;

            // 当前段是否走完
            var isCurrentSegmentCompleted = pastDistance >= currentSegmentLength - 1;
            if (isCurrentSegmentCompleted && ensureDirection)
            {
                // 为下一段做准备
                // var hasNextSegment = lineRenderer.positionCount > currentSegment + 2;

                // 当前段走完，则索引加1
                currentSegment++;

                // 循环移动
                if (currentSegment >= lineRenderer.positionCount - 1)
                    currentSegment = 0;

                return;
            }

            BuffTakeEffect();

            this.CharacterAnimatorController.SetFloat(AnimatorHashIDs.SpeedFloat, this.rb.velocity.magnitude);
            this.CharacterAnimatorController.SetBool(AnimatorHashIDs.DizzyBool, this.m_IsDizzy);
            this.CharacterAnimatorController.SetBool(AnimatorHashIDs.GroundBool, this.m_IsGround2);
        }

        // -----------------------------------------------------------------------------------------------------------------------------
        private CharacterCreateData characterData;
        public void Init(CharacterCreateData characterData)
        {
            this.characterData = characterData;
            this.ServerInstId = characterData.uid;
            this.Name = characterData.playerName;
            this.Head = characterData.head;
            this.SetPosition(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).position);
            this.SetRotation(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).rotation);
            this.LoadSkin(characterData.characterRoleCfgId, characterData.characterCarCfgId);
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
            return this.rb.velocity;
        }

        public virtual long GetServerInstId()
        {
            return ServerInstId;
        }

        public virtual void OnChangeAniTrigger(int hashId)
        {
            this.CharacterNetWorkSenderController.OnChangeAniTrigger(hashId);
        }

        public virtual void OnChangeAniFloat(int hashId, float value)
        {
            this.CharacterNetWorkSenderController.OnChangeAniFloat(hashId, value);
        }

        public virtual void OnChangeAniBool(int hashId, bool value)
        {
            this.CharacterNetWorkSenderController.OnChangeAniBool(hashId, value);
        }
        public CharacterInfo GetCharacterInfo()
        {
            characterInfo.RoleId = this.ServerInstId;
            characterInfo.Name = this.Name;
            characterInfo.HeadIconId = Head;
            characterInfo.Rank = 100;
            var status = CheckPointManager.Instance.GetStatus(ColliderForCheckpoint);
            if (status != null)
            {
                characterInfo.Rank = status.rank;
                ColliderForCheckpoint.name = this.Name;
            }
            return characterInfo;
        }
        // -----------------------------------------------------------------------------------------------------------------------------

        IEnumerator AutoBoost()
        {
            while (!MatchFinished)
            {
                var randomDelay = Random.Range(1, 5);
                yield return new WaitForSeconds(randomDelay);
                Boost();
            }
        }

        void CorrectingRotationError()
        {
            if (!self)
                return;

            var curEulerAngles = self.eulerAngles;
            curEulerAngles.x = 0;
            curEulerAngles.z = 0;
            self.eulerAngles = curEulerAngles;
        }

        public void LoadSkin(int roleId, int carId)
        {
            GameEntry.Coroutine.Start(loadSkin(roleId, carId));
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
                        // item.m_Player = this;
                    }
                }
            }
            result = GameEntry.AssetsLoader.LoadAsset(role.Asset);
            result.Result.IsGameObject = true;
            yield return result;
            if (result.Asset is GameObject go_role)
            {
                go_role.transform.SetParent(this.CharacterAnimatorController._animatorCar.transform.Find("car_all/car_am/car_Bone003/role"), false);
                this.CharacterAnimatorController._animatorRole = go_role.GetComponent<Animator>();
            }
        }

        void BuffTakeEffect()
        {
            var len = _forces.Count;
            while (len-- > 0)
            {
                var force = _forces[len];
                force.Time -= Time.fixedDeltaTime;
                if (force.Time <= 0f)
                {
                    _forces.RemoveAt(len);
                }
                rb.AddForce(force.Force * force.ForceValue, force.Mode);
            }
            if (len <= 0)
            {
                // this.CharacterAnimatorController.SetBool(AnimatorHashIDs.SprintBool, false);
            }
        }

        // 获取路径上的点
        Vector3 GetCatmullRomPoint(int segment, float t)
        {
            int pointCount = lineRenderer.positionCount;

            int i = Mathf.Clamp(segment, 0, pointCount - 1);
            int nextIndex = (i + 1) % pointCount;

            return CatmullRomSpline(lineRenderer.GetPosition((i - 1 + pointCount) % pointCount), lineRenderer.GetPosition(i), lineRenderer.GetPosition(nextIndex), lineRenderer.GetPosition((nextIndex + 1) % pointCount), t);
        }

        // 获取Catmull-Rom样条曲线上的切线方向
        Vector3 GetCatmullRomTangent(int segment, float t)
        {
            int pointCount = lineRenderer.positionCount;

            int i = Mathf.Clamp(segment, 0, pointCount - 1);
            int nextIndex = (i + 1) % pointCount;

            return CatmullRomSplineTangent(lineRenderer.GetPosition((i - 1 + pointCount) % pointCount), lineRenderer.GetPosition(i), lineRenderer.GetPosition(nextIndex), lineRenderer.GetPosition((nextIndex + 1) % pointCount), t).normalized;
        }

        // Catmull-Rom样条曲线插值
        Vector3 CatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t * t2;

            return 0.5f * ((2.0f * p1) + (-p0 + p2) * t + (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 + (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);
        }

        // Catmull-Rom样条曲线切线方向
        Vector3 CatmullRomSplineTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;

            return 0.5f * ((-p0 + p2) + 2.0f * (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t + 3.0f * (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t2);
        }

        // 计算路径的总长度
        float CalculateTotalDistance()
        {
            float totalDistance = 0.0f;
            int pointCount = lineRenderer.positionCount;

            for (int i = 1; i < pointCount; i++)
            {
                totalDistance += Vector3.Distance(lineRenderer.GetPosition(i - 1), lineRenderer.GetPosition(i));
            }

            return totalDistance;
        }

        void Jump(float forceValue = 25f)
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.Playing)
                return;

            if (!this.m_IsJump && this.m_IsGround && m_JumpTime <= 0)
            {
                this.m_IsJump = true;
                this.m_JumpTime = 0.1f;
                rb.AddForce(forceValue * Vector3.up, ForceMode.VelocityChange);
                this.CharacterAnimatorController.SetTrigger(AnimatorHashIDs.JumpTrigger);
            }
        }

        void Boost(bool ignoreCooldown = false)
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.Playing)
                return;

            var instId = GetInstanceID();

            if (ignoreCooldown)
            {
                // 已存在就移除
                var len = _forces.Count;
                while (len-- > 0)
                {
                    var force = _forces[len];
                    if (force.Id == instId)
                    {
                        _forces.RemoveAt(len);
                        break;
                    }
                }
            }
            else
            {
                if (boostCooldown > 0)
                    return;
            }

            // Debug.Log($"AI 加速");
            var buff = new ForceBuff();
            buff.Force = transform.forward;
            buff.Id = instId;
            buff.Mode = ForceMode.VelocityChange;
            buff.ForceValue = 0.5f;
            buff.Time = 1f;
            _forces.Add(buff);

            vehicleEffect?.OnAccelerate();

            boostCooldown = 5;
            // this.CharacterAnimatorController.SetBool(AnimatorHashIDs.SprintBool, true);
        }

        void IVehicle.ApplyBoostByArea(BoostArea value)
        {
            if (self == null)
            {
                return;
            }
            var dot = Vector3.Dot(value.transform.forward, self.forward);
            if (dot < 0.5f)
            {
                return;
            }
            if (value.ForceTime > 0f)
            {
                foreach (var item in _forces)
                {
                    if (item.Id == value.GetInstanceID())
                    {
                        return;
                    }
                }
                var buff = new ForceBuff();
                buff.Force = value.transform.forward;
                buff.Id = value.GetInstanceID();
                buff.Mode = value.Mode;
                buff.ForceValue = value.ForceValue;
                buff.Time = value.ForceTime;
                _forces.Add(buff);
            }
            else
            {
                rb.AddForce(value.transform.forward * value.ForceValue, value.Mode);
            }

            vehicleEffect?.OnAccelerateByGate();
        }

        void IVehicle.ApplyJumpByArea(JumpArea value)
        {
            Jump();
        }

        void IVehicle.ApplyHitByArea(HitArea value)
        {
            rb.AddForce(value.VelocityBuff.VelocityDirection * value.VelocityBuff.VelocityValue, ForceMode.VelocityChange);
        }

        void IVehicle.ApplyHeavyHit(float time)
        {
            if (this.m_DizzyTime > 0.5f)
            {
                return;
            }
            this.m_DizzyTime = time;
            this.m_IsDizzy = true;
        }


        int ICheckableObject.GetEntityId()
        {
            return Id;
        }

        void ITrapsAcceptor.ExecuteJump(AIOperationJump data)
        {
            // Debug.Log($"AI 跳");
            if (!m_IsGround)
                return;

            Jump(data.Force);
        }

        void ITrapsAcceptor.ExecuteBoost(AIOperationBoost data)
        {
            Boost(true);
        }
        public void AddBuff(BaseBuff value)
        {

        }
    }
}