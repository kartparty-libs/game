// using Cinemachine;
// using Framework;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.Video;

// public partial class Player : MonoBehaviour, IVehicle, ICharacter
// {
//     public float m_GravityValue = -9.81f;
//     public float m_MovementSpeed = 6f;
//     public float m_MovementAcceleratedSpeed = 1f;
//     public float m_RotateSpeed = 200f;
//     public float m_CameraRotateSpeed = 30f;
//     public float m_JumpHeight = 1.5f;
//     public float m_JumpSpeed = 5f;
//     public bool m_IsGround = false;
//     public CinemachineVirtualCamera m_CinemachineVirtualCamera;

//     public Transform m_Skin { get; set; }
//     public Vector2 m_Steer { get; set; }
//     public Vector2 m_CameraSteer { get; set; }
//     public GameObject m_ColliderForCheckpoint { get; private set; }
//     private PlayerInput m_Input;
//     private AudioSource m_AudioSource;
//     private IVehicleEffect m_Effectctrl;
//     public CharacterController m_CharacterController { get; private set; }
//     private Vector3 m_MovementVelocity = Vector3.zero;
//     private Vector3 m_MovementDirection = Vector3.zero;
//     private float m_JumpVelocity;
//     private bool m_IsJump = false;
//     private bool m_outCtrl = false;
//     private float m_outCtrlTime;

//     private int m_GroundLayer;
//     private float m_GroundCheckOffset = 0.3f;
//     private Vector3 m_GroundNormal = Vector3.zero;
//     private float m_CharacterIncludedAngleByGround;

//     public Vector3 m_RotateCrossDir;
//     public float m_Speed { get; private set; }
//     private float m_InertiaSpeed;
//     private float SpeedUpLerpTime;
//     private float SpeedDownLerpTime;

//     private List<SpeedUpBuff> m_SpeedUpBuffs = new List<SpeedUpBuff>();
//     private List<VelocityBuff> m_VelocityBuffs = new List<VelocityBuff>();
//     private Vector3 m_RestorePosition = Vector3.zero;
//     public Vector2 m_CameraLarp = Vector3.zero;
//     private CinemachinePOV m_CinemachinePOV;
//     void Start()
//     {
//         m_CharacterController = GetComponent<CharacterController>();
//         m_AudioSource = GetComponent<AudioSource>();

//         m_GroundLayer = LayerMask.GetMask(new string[] { LayerNames.Ground, LayerNames.Wall });
//         m_ColliderForCheckpoint = CheckPointManager.AddTriggerForCheckableObject(transform);
//         m_Effectctrl = GetComponent<IVehicleEffect>();
//         m_CinemachinePOV = this.m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();


//         this.m_CameraLarp.x = m_CinemachinePOV.m_HorizontalAxis.Value;
//         this.m_CameraLarp.y = m_CinemachinePOV.m_VerticalAxis.Value;
//     }
//     public void SetInput(PlayerInput value)
//     {
//         m_Input = value;
//         m_Input.SetEnabled(true);
//     }
//     void Update()
//     {
//         var deltaTime = Time.deltaTime;
//         if (m_Input != null)
//         {
//             m_Input.Update(deltaTime);
//             while (true)
//             {
//                 var action = m_Input.Read();
//                 if (action == null)
//                 {
//                     break;
//                 }
//                 if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//                 {
//                     continue;
//                 }
//                 if (action.Name == PlayerInput.Steer)
//                 {
//                     if (action.Phase == GameDeviceActionPhase.Performed)
//                     {
//                         m_Steer = action.DataValue2;
//                     }
//                     else if (action.Phase == GameDeviceActionPhase.Canceled)
//                     {
//                         m_Steer = Vector2.zero;
//                     }
//                 }
//                 else if (action.Name == PlayerInput.Jump)
//                 {
//                     if (action.Phase == GameDeviceActionPhase.Started)
//                     {
//                         DoJumpUp();
//                     }
//                 }
//             }
//         }
//         if (m_outCtrl)
//         {
//             m_Steer = Vector2.zero;
//             m_outCtrlTime -= deltaTime;
//             if (m_outCtrlTime <= 0f)
//             {
//                 m_outCtrl = false;
//                 resetState();
//             }
//         }
//         if (Physics.SphereCast(this.transform.position + Vector3.up * m_GroundCheckOffset, this.m_CharacterController.radius, Vector3.down, out RaycastHit hit, m_GroundCheckOffset - this.m_CharacterController.radius + 2 * this.m_CharacterController.skinWidth, this.m_GroundLayer))
//         {
//             this.m_IsGround = true;
//             this.m_GroundNormal = hit.normal;
//             Vector3 project = Vector3.Project(this.m_GroundNormal, this.transform.forward);
//             this.m_CharacterIncludedAngleByGround = Vector3.Angle(this.transform.forward, hit.normal);

//             this.m_MovementDirection = Vector3.ProjectOnPlane(this.transform.forward, this.m_GroundNormal).normalized;
//             // Debug.DrawLine(hit.point, hit.point + hit.normal * 3f, Color.blue);
//             // Debug.DrawLine(hit.point, hit.point + project * 3f, Color.green);
//         }
//         else
//         {
//             this.m_IsGround = false;

//             if (this.m_CharacterIncludedAngleByGround != 90)
//             {
//                 this.m_CharacterIncludedAngleByGround = Mathf.Lerp(this.m_CharacterIncludedAngleByGround, 90, deltaTime);
//                 if (Mathf.Abs(this.m_CharacterIncludedAngleByGround - 90) < 0.1f)
//                 {
//                     this.m_CharacterIncludedAngleByGround = 90;
//                 }
//             }

//             if (this.m_MovementDirection != this.transform.forward)
//             {
//                 this.m_MovementDirection = Vector3.Lerp(this.m_MovementDirection, this.transform.forward, deltaTime * 10f).normalized;
//                 float angle = Vector3.Angle(this.m_MovementDirection, this.transform.forward);
//                 if (angle < 0.5f)
//                 {
//                     this.m_MovementDirection = this.transform.forward;
//                 }
//             }

//             this.m_GroundNormal.Set(0, 1, 0);
//         }
//         // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3f, Color.red);
//         // Debug.DrawLine(this.transform.position, this.transform.position + this.m_MovementDirection * 3f, Color.yellow);

//         this.RotateUpdate(deltaTime);
//         this.VelocityUpdate(deltaTime);
//         this.CameraUpdate(deltaTime);
//         this.AnimUpdate(deltaTime);

//         if (m_AudioSource != null)
//         {
//             var tpitch = 0.8f + m_Speed / m_MovementSpeed;
//             if (!m_IsGround)
//             {
//                 tpitch = 2f;
//             }
//             m_AudioSource.pitch = Mathf.Lerp(m_AudioSource.pitch, tpitch, deltaTime * (m_IsGround ? 1f : 5f));
//         }
//     }

//     void LateUpdate()
//     {
//         if (m_Skin != null)
//         {
//             m_Skin.transform.position = this.transform.position;
//             m_Skin.transform.rotation = this.transform.rotation;
//             // Quaternion targetRotation = Quaternion.FromToRotation(m_Skin.transform.up, this.m_GroundNormal) * m_Skin.transform.rotation;
//             // targetRotation.y = this.transform.rotation.y;
//             // targetRotation.z = this.transform.rotation.z;
//             // m_Skin.transform.rotation = Quaternion.Slerp(m_Skin.transform.rotation, targetRotation, 10 * Time.deltaTime);
//         }
//     }

//     void OnControllerColliderHit(ControllerColliderHit i_ControllerColliderHit)
//     {
//         HitArea pHitArea = i_ControllerColliderHit.gameObject.GetComponent<HitArea>();
//         // todo
//         // pHitArea?.OnColliderHit(this, i_ControllerColliderHit);
//     }

//     // -----------------------------------------------------------------------------------------------------------------------------
//     // 功能
//     public float RotationDotValue;
//     private void RotateUpdate(float i_nDeltaTime)
//     {
//         if (m_Steer.magnitude > 0.1f)
//         {
//             Vector3 rotate = new Vector3(this.m_Steer.x, 0, m_Steer.y);
//             rotate = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * rotate;
//             Quaternion targetRotation = Quaternion.LookRotation(rotate);
//             float maxRotationSpeed = this.m_RotateSpeed * i_nDeltaTime;
//             Quaternion currRotation = this.transform.rotation;
//             Quaternion newRotation = Quaternion.RotateTowards(currRotation, targetRotation, maxRotationSpeed);
//             this.transform.rotation = newRotation;

//             var targetDir = targetRotation * Vector3.forward;
//             var nowDir = transform.rotation * Vector3.forward;
//             this.m_RotateCrossDir = Vector3.Cross(targetDir, nowDir);//正负号
//             RotationDotValue = Vector3.Dot(targetDir, nowDir);
//             if (this.m_IsGround && !m_outCtrl)
//             {

//                 if (this.m_RotateCrossDir.y > 0)
//                 {
//                     SetState(SteerL);
//                 }
//                 else if (this.m_RotateCrossDir.y < 0)
//                 {
//                     SetState(SteerR);
//                 }
//             }
//         }
//         else
//         {
//             RotationDotValue = 1f;
//         }
//     }

//     private void VelocityUpdate(float i_nDeltaTime)
//     {
//         if (this.m_RestorePosition != Vector3.zero)
//         {
//             Debug.Log("this.m_RestorePosition = " + this.m_RestorePosition);
//             this.transform.position = this.m_RestorePosition;
//             this.m_RestorePosition = Vector3.zero;
//         }
//         else
//         {
//             this.GravityVelocity(i_nDeltaTime);
//             this.JumpVelocity(i_nDeltaTime);
//             this.HitVelocity(i_nDeltaTime);
//             this.MovementVelocity(i_nDeltaTime);

//             this.m_CharacterController.Move(this.m_MovementVelocity * i_nDeltaTime);
//             // Debug.DrawRay(this.transform.position, this.m_MovementVelocity * i_nDeltaTime, Color.red, 60f);
//         }
//         this.m_MovementVelocity.Set(0, 0, 0);
//     }

//     private void MovementVelocity(float i_nDeltaTime)
//     {
//         float steerMagnitude = this.m_Steer.magnitude;
//         if (steerMagnitude > 0.1f)
//         {
//             steerMagnitude = Mathf.Min(1, steerMagnitude);

//             float includedAngle = this.m_CharacterIncludedAngleByGround - 90;
//             float includedAngleAbs = Mathf.Abs(includedAngle);
//             if (includedAngleAbs > 0.1)
//             {
//                 float angleSpeed = Mathf.Clamp(-includedAngle * 0.2f, -2, 30);
//                 this.m_InertiaSpeed = Mathf.Lerp(this.m_InertiaSpeed, angleSpeed, i_nDeltaTime * (angleSpeed > 0 ? 5 : 0.5f));
//             }
//             else
//             {
//                 this.m_InertiaSpeed = Mathf.Lerp(this.m_InertiaSpeed, 0, i_nDeltaTime * 0.5f);
//             }

//             // Debug.Log("this.m_InertiaSpeed = " + this.m_InertiaSpeed);

//             float targetSpeed = this.m_MovementSpeed * steerMagnitude + this.m_InertiaSpeed;

//             this.SpeedDownLerpTime = 0;
//             this.SpeedUpLerpTime += this.m_MovementAcceleratedSpeed * i_nDeltaTime;

//             this.m_Speed = this.SpeedUpLerpTime >= 1 ? targetSpeed : Mathf.Lerp(this.m_Speed, targetSpeed, this.SpeedUpLerpTime);

//             this.m_MovementVelocity += this.m_MovementDirection * this.m_Speed;
//         }
//         else
//         {
//             this.SpeedUpLerpTime = 0;

//             if (this.m_Speed > 0)
//             {
//                 this.SpeedDownLerpTime += this.m_MovementAcceleratedSpeed * i_nDeltaTime;
//                 this.m_Speed = this.SpeedDownLerpTime >= 1 ? 0 : Mathf.Lerp(this.m_Speed, 0, this.SpeedDownLerpTime);
//                 this.m_MovementVelocity += this.m_MovementDirection * this.m_Speed;
//             }
//         }

//         if (this.m_SpeedUpBuffs.Count > 0)
//         {
//             float speedUp = 0;
//             for (int i = this.m_SpeedUpBuffs.Count - 1; i >= 0; i--)
//             {
//                 // speedUp += this.m_SpeedUpBuffs[i].SpeedUpValue * this.m_SpeedUpBuffs[i].Timer / this.m_SpeedUpBuffs[i].Time;
//                 speedUp += this.m_SpeedUpBuffs[i].SpeedUpValue;
//                 this.m_SpeedUpBuffs[i].Timer -= i_nDeltaTime;
//                 if (this.m_SpeedUpBuffs[i].Timer <= 0)
//                 {
//                     this.m_SpeedUpBuffs.RemoveAt(i);
//                 }
//             }
//             this.m_MovementVelocity += this.m_MovementDirection * speedUp;
//         }
//     }

//     private void JumpVelocity(float i_nDeltaTime)
//     {
//         if (this.m_IsJump)
//         {
//             this.m_IsJump = false;
//             this.m_JumpVelocity = this.m_JumpSpeed;
//             this.m_MovementVelocity.y = this.m_JumpVelocity;
//         }
//     }

//     private void GravityVelocity(float i_nDeltaTime)
//     {
//         if (!this.m_IsGround || this.m_IsJump)
//         {
//             this.m_JumpVelocity += this.m_GravityValue * i_nDeltaTime;
//         }
//         else
//         {
//             this.m_JumpVelocity = 0;
//         }
//         this.m_MovementVelocity.y = this.m_JumpVelocity;
//     }

//     private void HitVelocity(float i_nDeltaTime)
//     {
//         if (this.m_VelocityBuffs.Count > 0)
//         {
//             for (int i = this.m_VelocityBuffs.Count - 1; i >= 0; i--)
//             {
//                 float VelocityValue = this.m_VelocityBuffs[i].VelocityValue * this.m_VelocityBuffs[i].Timer / this.m_VelocityBuffs[i].Time;
//                 this.m_MovementVelocity += this.m_VelocityBuffs[i].VelocityDirection * VelocityValue;
//                 this.m_VelocityBuffs[i].Timer -= i_nDeltaTime;
//                 if (this.m_VelocityBuffs[i].Timer <= 0)
//                 {
//                     this.m_VelocityBuffs.RemoveAt(i);
//                 }
//             }
//         }
//     }

//     private void CameraUpdate(float i_nDeltaTime)
//     {
//         if (this.m_CinemachinePOV == null)
//         {
//             return;
//         }

//         if (this.m_CameraSteer.sqrMagnitude < 0.1f)
//         {
//             return;
//         }

//         // 旋转摄像机
//         m_CinemachinePOV.m_HorizontalAxis.Value += this.m_CameraSteer.x / Screen.width * (m_CinemachinePOV.m_HorizontalAxis.m_MaxValue - m_CinemachinePOV.m_HorizontalAxis.m_MinValue);
//         m_CinemachinePOV.m_VerticalAxis.Value -= this.m_CameraSteer.y / Screen.height * (m_CinemachinePOV.m_VerticalAxis.m_MaxValue - m_CinemachinePOV.m_VerticalAxis.m_MinValue);

//         this.m_CameraSteer = Vector2.zero;
//     }

//     // -----------------------------------------------------------------------------------------------------------------------------
//     // 通知

//     public void ApplyJumpByArea(JumpArea value)
//     {
//         if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//         {
//             return;
//         }
//         if (!this.m_IsGround)
//         {
//             return;
//         }
//         this.DoJumpUp(value.VelocityBuff);
//         this.m_Effectctrl?.OnJump();
//     }

//     public void ApplyBoostByArea(BoostArea value)
//     {
//         if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//         {
//             return;
//         }
//         this.AddSpeedUpBuff(value.SpeedUpBuff);
//         this.m_Effectctrl?.OnAccelerateByGate();
//     }

//     public void ApplyHitByArea(HitArea value)
//     {
//         if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//         {
//             return;
//         }
//         this.AddVelocityBuff(value.VelocityBuff);
//     }
//     public void ApplyHeavyHit(float time = 2f)
//     {
//         if (m_outCtrlTime > 0.5f)
//         {
//             return;
//         }
//         SetState(Hit);
//         m_outCtrlTime = time;
//         m_outCtrl = true;

//     }
//     public void DoJumpUp(VelocityBuff velocityBuff = null)
//     {
//         if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//         {
//             return;
//         }
//         bool bJump = Physics.Raycast(this.transform.position + Vector3.up * m_GroundCheckOffset, Vector3.down, out RaycastHit hit, 2 * m_GroundCheckOffset, this.m_GroundLayer);
//         if ((bJump || this.m_IsGround) && !this.m_IsJump)
//         {
//             this.m_IsJump = true;
//             SetState(Jump);
//             this.m_Effectctrl?.OnJump();

//             if (velocityBuff != null)
//             {
//                 this.AddVelocityBuff(velocityBuff);
//             }
//         }
//     }

//     public void DoBoost(bool passive = true)
//     {
//         if (GameEntry.Context != null && !GameEntry.Context.PlayerInputEnable)
//         {
//             return;
//         }
//         foreach (var buff in this.m_SpeedUpBuffs)
//         {
//             if (buff.Id == this.GetInstanceID()) return;
//         }
//         SpeedUpBuff speedUpBuff = new SpeedUpBuff();
//         speedUpBuff.Id = this.GetInstanceID();
//         speedUpBuff.Time = 1f;
//         speedUpBuff.Timer = speedUpBuff.Time;
//         speedUpBuff.SpeedUpValue = 5;
//         this.AddSpeedUpBuff(speedUpBuff);
//         this.m_Effectctrl?.OnAccelerate();
//         if (passive)
//         {

//         }
//         else
//         {

//         }

//         SetState(Fast);
//     }

//     public void AddSpeedUpBuff(SpeedUpBuff speedUpBuff)
//     {
//         foreach (var buff in this.m_SpeedUpBuffs)
//         {
//             if (buff.Id == speedUpBuff.Id) return;
//         }
//         this.m_SpeedUpBuffs.Add(speedUpBuff);
//     }

//     public void AddVelocityBuff(VelocityBuff velocityBuff)
//     {
//         foreach (var buff in this.m_VelocityBuffs)
//         {
//             if (buff.Id == velocityBuff.Id) return;
//         }
//         this.m_VelocityBuffs.Add(velocityBuff);
//     }

//     /// <summary>
//     /// 设置角色坐标
//     /// </summary>
//     /// <param name="position"></param>
//     public void SetPosition(Vector3 position)
//     {
//         this.m_RestorePosition = position;
//     }

//     /// <summary>
//     /// 设置角色线性速度
//     /// </summary>
//     /// <param name="velocity"></param>
//     public void SetVelocity(Vector3 velocity)
//     {
//     }
// }
// public partial class Player
// {
//     public const int Normal = 0;
//     public const int Jump = 1;
//     public const int Fast = 2;
//     public const int Victory = 3;
//     public const int SteerL = 4;
//     public const int SteerR = 5;
//     public const int Hit = 6;
//     private Animator _animatorRole;
//     private Animator _animatorCar;
//     private int _aniState;
//     public float _jumpTime;
//     private float _fastTime;
//     public void LoadSkin(int roleId, int carId)
//     {
//         GameEntry.Coroutine.Start(loadSkin(roleId, carId));

//     }
//     public void EngineShutdown()
//     {
//         m_AudioSource.enabled = false;
//     }
//     IEnumerator loadSkin(int roleId, int carId)
//     {
//         if (m_Skin != null)
//         {
//             m_Skin.gameObject.SetActive(false);
//         }
//         yield return null;
//         var skin = new GameObject("playerSkin");
//         // todo
//         // skin.AddComponent<PlayerSkin>().m_Player = this;
//         m_Skin = skin.transform;
//         var role = GameEntry.Table.Role.Get(GameEntry.Context.SelectRoleId);
//         var car = GameEntry.Table.Car.Get(GameEntry.Context.SelectCarId);

//         var result = GameEntry.AssetsLoader.LoadAsset(car.Asset);
//         result.Result.IsGameObject = true;
//         yield return result;
//         if (result.Asset is GameObject go_car)
//         {
//             go_car.transform.SetParent(m_Skin, false);
//             // go_car.transform.localPosition = new Vector3(0, m_OffsetY, 0);
//             _animatorCar = go_car.GetComponent<Animator>();
//             WheelScript[] wheelScripts = go_car.GetComponentsInChildren<WheelScript>();
//             if (wheelScripts.Length > 0)
//             {
//                 foreach (var item in wheelScripts)
//                 {
//                     // todo
//                     // item.m_Player = this;
//                 }
//             }
//         }
//         result = GameEntry.AssetsLoader.LoadAsset(role.Asset);
//         result.Result.IsGameObject = true;
//         yield return result;
//         if (result.Asset is GameObject go_role)
//         {
//             go_role.transform.SetParent(_animatorCar.transform.Find("car_all/car_am/car_Bone003/role"), false);
//             // go_role.transform.localPosition = new Vector3(0, m_OffsetY, 0);
//             _animatorRole = go_role.GetComponent<Animator>();

//         }

//     }
//     private void AnimUpdate(float i_nDeltaTime)
//     {
//         if (_animatorCar == null || _animatorRole == null)
//         {
//             return;
//         }

//         _animatorCar.SetFloat("Speed", this.m_Speed);
//         _animatorRole.SetFloat("Speed", this.m_Speed);


//         if (_finished)
//         {
//             return;
//         }
//         if (_aniState == Jump)
//         {
//             _jumpTime += Time.deltaTime;
//             if (_jumpTime > 0.5f)
//             {
//                 SetAniFloat("Ground", m_IsGround ? 1f : 0f);
//             }
//         }
//         else
//         {
//             SetAniFloat("Ground", m_IsGround ? 1f : 0f);
//         }
//         if (_aniState == Fast)
//         {
//             _fastTime -= i_nDeltaTime;
//             if (_fastTime <= 0f)
//             {
//                 resetState();
//             }
//         }
//         else if (_aniState == SteerL)
//         {
//             if (RotationDotValue > 0.999f)
//             {
//                 SetAniFloat("Steer", 0);
//             }
//             else
//             {
//                 SetAniFloat("Steer", -1);
//             }
//         }
//         else if (_aniState == SteerR)
//         {
//             if (RotationDotValue > 0.999f)
//             {
//                 SetAniFloat("Steer", 0);
//             }
//             else
//             {
//                 SetAniFloat("Steer", 1);
//             }
//         }
//     }
//     private void resetState()
//     {
//         _aniState = Normal;
//     }
//     private bool _finished;
//     public void SetVictory()
//     {
//         _finished = true;
//         SetAniTrigger("Victory");
//     }
//     public void SetState(int state, float value = 0f)
//     {
//         if (m_outCtrl)
//         {
//             return;
//         }
//         _aniState = state;
//         if (_aniState == Jump)
//         {
//             _jumpTime = 0f;
//             SetAniFloat("Ground", 0f);
//             SetAniTrigger("Jump");
//         }
//         else if (_aniState == Fast)
//         {
//             _fastTime = value;
//             SetAniTrigger("Fast");
//         }
//         else if (_aniState == Victory)
//         {
//             _finished = true;
//             SetAniTrigger("Victory");
//         }
//         else if (_aniState == Hit)
//         {
//             SetAniTrigger("Hit");
//         }
//     }

//     private void SetAniTrigger(string name)
//     {
//         if (_animatorRole == null || _animatorCar == null) return;
//         _animatorCar.SetTrigger(name);
//         _animatorRole.SetTrigger(name);
//     }
//     private void SetAniFloat(string name, float value)
//     {
//         if (_animatorRole == null || _animatorCar == null) return;
//         _animatorCar.SetFloat(name, value);
//         _animatorRole.SetFloat(name, value);
//     }
// }