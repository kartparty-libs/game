using Cinemachine;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig : MonoBehaviour
{
    public float AISpeedMin = 19f;
    public float AISpeedMax = 23f;
    public Camera MainCamera;
    public CinemachineVirtualCamera FollowVirtualCamera;
    public CinemachineVirtualCamera FinishCamera;
    public CheckPointManager CheckPointManager;
    public AIPathManager AIPathManager;
    public ResetBoard ResetBoard;
    private Transform[] OriginPoints;
    public float HFov = 60;
    public float VFov = 77;
    private void Awake()
    {
        var vm = FinishCamera.gameObject.AddComponent<VMRotation>();
        vm.InputEnable = false;
        vm.PoseX = 1f;
        vm.PoseY = 2f;
        vm.PoseZ = 2f;
        vm.Distance = 5f;
        vm.VerticalHeight = 1f;
    }
    private void Start()
    {
        if (GameEntry.Context != null)
        {
            GameEntry.Context.Init(this);
        }

        this.OriginPoints = new Transform[8];
        Transform pOriginPoints = transform.Find("originPoints");
        if (pOriginPoints != null)
        {
            for (int i = 0; i < pOriginPoints.childCount; i++)
            {
                this.OriginPoints[i] = pOriginPoints.GetChild(i);
            }
        }
        CheckPointManager?.gameObject.SetActive(true);
        ResetBoard?.gameObject.SetActive(true);
        AIPathManager?.gameObject.SetActive(true);

        FollowVirtualCamera.gameObject.SetActive(true);
    }

    public void Focus()
    {
        if (FinishCamera == null)
        {
            return;
        }
        FinishCamera.gameObject.SetActive(true);
        FollowVirtualCamera.gameObject.SetActive(false);
        FinishCamera.Follow = GameEntry.Context.FollowPlayer.transform;
        FinishCamera.LookAt = GameEntry.Context.FollowPlayer.transform;
        GameEntry.Coroutine.Start(ShowFocus());
    }
    private IEnumerator ShowFocus()
    {
        yield return new WaitForSeconds(3f);
        if (FollowVirtualCamera == null)
        {
            yield break;
        }
        FollowVirtualCamera.ForceCameraPosition(FinishCamera.transform.position, FinishCamera.transform.rotation);
        FinishCamera.gameObject.SetActive(false);
        FollowVirtualCamera.gameObject.SetActive(true);
    }
    public void GameStart()
    {
    }

    /// <summary>
    /// 获取赛道起始点
    /// </summary>
    /// <param name="i_nTrackIdx"></param>
    /// <returns></returns>
    public Transform GetOriginPoint(int i_nTrackIdx)
    {
        if (this.OriginPoints.Length <= i_nTrackIdx)
        {
            return this.OriginPoints[0];
        }

        return this.OriginPoints[i_nTrackIdx];
    }
    private int ScreenOrientation = -1;
    private void Update()
    {
        if (GameEntry.Context.EnableRotation)
        {
            var orientation = Screen.height > Screen.width ? 90 : 0;
            if (ScreenOrientation != orientation)
            {
                ScreenOrientation = orientation;
                FollowVirtualCamera.m_Lens.FieldOfView = ScreenOrientation > 0 ? VFov : HFov;
                FinishCamera.m_Lens.FieldOfView = ScreenOrientation > 0 ? VFov : HFov;
            }
        }
    }
}
