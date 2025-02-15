using Framework;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameMain : FsmStateAction
{
    private ResResult _sceneResult;
    protected override void OnEnter()
    {
        base.OnEnter();
        AudioDefine.Load();
        GameEntry.Context.Offline.LoadMapStart();
        GameEntry.GUI.Open(GameEntry.GUIPath.LoadingUI.Path, LoadingUI.SetTotalTime, 2f);
        GameEntry.Context.GameConfig.MainCamera.gameObject.SetActive(true);
        GameEntry.Coroutine.Start(load());
        GameEntry.GUI.Open(GameEntry.GUIPath.NoticeUI.Path);
        GameEntry.GUI.Open(GameEntry.GUIPath.EffectUI.Path);

        GameEntry.Match.Close();
    }
    protected override void OnExit()
    {
        base.OnExit();
        GameEntry.AssetsLoader.Unload(_sceneResult);
        GameEntry.AssetsLoader.Unload();
        GameEntry.GUI.Close(GameEntry.GUIPath.SelectRoleUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.MatchUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);
        if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset asset)
        {
            asset.shadowDistance = 25f;
        }
    }
    IEnumerator load()
    {
        while (!GameEntry.GUI.IsOpen(GameEntry.GUIPath.LoadingUI.Path))
        {
            yield return null;
        }
        _sceneResult = GameEntry.AssetsLoader.LoadAsset("Assets/Res/Scenes/SelectRoleMap.unity");
        yield return _sceneResult;
        while (!_sceneResult.Result.SceneAsset.isDone)
        {
            yield return null;
        }
        _sceneResult.SetSceneActive();
        if (GameEntry.Context.NeedRegister)
        {
            while (GameEntry.Context.NeedRegister)
            {
                yield return null;
            }
        }
        GameEntry.GUI.Close(GameEntry.GUIPath.LoginUI.Path);
        GameEntry.HideDefault();
        GameEntry.GUI.Close(GameEntry.GUIPath.BindMailUI.Path);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI.Path, SelectRoleUI.PlayShowAni);
        GameEntry.GUI.Open(GameEntry.GUIPath.SelectRoleUI.Path);

        while (!GameEntry.GUI.IsOpen(GameEntry.GUIPath.SelectRoleUI.Path))
        {
            yield return null;
        }
        GameEntry.GUI.Close(GameEntry.GUIPath.LoadingUI.Path);
        yield return null;
        GameEntry.Context.Offline.LoadMapEnd();
        CarCultivateSystem.Instance.EnterMain();
        yield return null;
        // GameEntry.GUI.PreLoad(GameEntry.GUIPath.StakeUI.Path);
        GameEntry.GUI.PreLoad(GameEntry.GUIPath.OpenBoxUI.Path);
        GameEntry.GUI.PreLoad(GameEntry.GUIPath.NewTaskUI.Path);

        if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset asset)
        {
            asset.shadowDistance = 5f;
        }
        if (GameEntry.Context.BackToSelectMap)
        {
            GameEntry.Context.BackToSelectMap = false;
            if (GameEntry.Context.MatchMode)
            {
                ExecuteAction.ByActionType(ExecuteActionType.Map_Pvp);
            }
            else
            {
                ExecuteAction.ByActionType(ExecuteActionType.Map_Pve);
            }
        }
    }
}
