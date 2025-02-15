using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Framework;
using UnityEngine;

public class SelectRoleSceneConfig : MonoBehaviour
{
    public List<GameObject> Cars;
    public List<GameObject> Roles;
    public GameObject UpgradeEffect;
    // public List<GameObject> PartPoints;
    // public List<GameObject> Car03Parts;
    public GameObject CarRender;

    private void Start()
    {
        if (GameEntry.Context != null)
        {
            GameEntry.Context.SelectRoleSceneConfig = this;
        }
        var len = Cars.Count;
        while (len-- > 0)
        {
            // Cars[len].SetActive(len==0);
        }
        len = Roles.Count;
        while (len-- > 0)
        {
            var role = Roles[len];
            // role.SetActive(len == 0);
            role.AddComponent<RoleRandAni>();
        }
        Utils.Unity.SetActive(UpgradeEffect, false);
        HidePart();
    }
    public void ShowUpgradeEffect()
    {
        Utils.Unity.SetActive(UpgradeEffect, false);
        Utils.Unity.SetActive(UpgradeEffect, true);
        GameEntry.Timer.Start(3f, () =>
        {
            Utils.Unity.SetActive(UpgradeEffect, false);
        }, 1);
    }
    public void HidePart()
    {
        // return;
        
    }
    public void ShowPart(int id)
    {
        
    }


    public CinemachineVirtualCamera VirtualCamera;
    public float HFov = 45;
    public float VFov = 60;
    private int ScreenOrientation = -1;

    private void Update()
    {
        if (GameEntry.Context.EnableRotation)
        {
            var orientation = Screen.height > Screen.width ? 90 : 0;
            if (ScreenOrientation != orientation)
            {
                ScreenOrientation = orientation;
                VirtualCamera.m_Lens.FieldOfView = ScreenOrientation > 0 ? VFov : HFov;
                VirtualCamera.transform.rotation = Quaternion.Euler(3f, 0, orientation);
            }
        }
        else
        {
            VirtualCamera.transform.rotation = Quaternion.Euler(3f, 0, 0);
            ScreenOrientation = 0;
            VirtualCamera.m_Lens.FieldOfView = Screen.height > Screen.width ? VFov : HFov;
        }
    }
}
