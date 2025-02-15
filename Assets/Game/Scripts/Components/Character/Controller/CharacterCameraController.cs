using Cinemachine;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色相机控制器
    /// </summary>
    public class CharacterCameraController : MonoBehaviour
    {
        public MainCharacter m_Character;

        public Vector2 m_CameraSteer { get; set; }

        private CinemachinePOV m_CinemachinePOV;
        private CinemachineVirtualCamera FollowCamera;
        private float m_ScreenValueX = 1.2f;
        private float m_ScreenValueY = 1.2f;



        private Transform FollowTarget;
        private Vector3 CameraPos;
        private bool _inited;
        public float distance;
        public Vector3 Forward;
        public float HorizontalAngle;
        private float VerticalAngle;

        void Start()
        {
            FollowCamera = GameEntry.Context.SceneConfig.FollowVirtualCamera;
            FollowCamera.Follow = this.m_Character.transform;
            // GameEntry.Context.SceneConfig.FollowVirtualCamera.LookAt = this.m_Character.transform;
            // this.m_CinemachinePOV = GameEntry.Context.SceneConfig.FollowVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        void Update()
        {
            return;
            if (this.m_CinemachinePOV == null)
            {
                return;
            }

            if (this.m_CameraSteer.sqrMagnitude < 0.1f)
            {
                return;
            }

            m_CinemachinePOV.m_HorizontalAxis.Value += this.m_CameraSteer.x / Screen.width * m_ScreenValueX * (m_CinemachinePOV.m_HorizontalAxis.m_MaxValue - m_CinemachinePOV.m_HorizontalAxis.m_MinValue);
            m_CinemachinePOV.m_VerticalAxis.Value -= this.m_CameraSteer.y / Screen.height * m_ScreenValueY * (m_CinemachinePOV.m_VerticalAxis.m_MaxValue - m_CinemachinePOV.m_VerticalAxis.m_MinValue);

            this.m_CameraSteer = Vector2.zero;
        }
        private void LateUpdate()
        {
            return;
            if (!_inited)
            {
                if (m_Character != null)
                {
                    FollowTarget = m_Character.transform;
                    _inited = true;
                    CameraPos = Vector3.up * 3f + FollowTarget.transform.forward * -3f;
                    FollowCamera.Follow = null;
                    HorizontalAngle = Quaternion.LookRotation(-CameraPos).y;
                    distance = CameraPos.magnitude;
                    Forward = -CameraPos.normalized;
                    VerticalAngle = 3f;
                }
            }
            if (_inited)
            {
                HorizontalAngle += (this.m_CameraSteer.x / Screen.width) * 360f * m_ScreenValueX;
                VerticalAngle -= (this.m_CameraSteer.y / Screen.height) * 3f * m_ScreenValueY;
                VerticalAngle = Mathf.Clamp(VerticalAngle, 0.3f, 3f);
                this.m_CameraSteer = Vector2.zero;
                var yr = Quaternion.Euler(0, HorizontalAngle, 0);
                Forward = yr * Vector3.forward;
                var cameraPos = FollowTarget.transform.position + Forward * distance;
                Debug.DrawLine(FollowTarget.transform.position, cameraPos);
                FollowCamera.transform.position = Vector3.Lerp(FollowCamera.transform.position, cameraPos + Vector3.up * VerticalAngle, Time.deltaTime * 8f);
                FollowCamera.transform.rotation = Quaternion.LookRotation(FollowTarget.position - FollowCamera.transform.position);
            }
        }
    }
}