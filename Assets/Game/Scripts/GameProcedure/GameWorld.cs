using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using static EnumDefine;

public class GameWorld : FsmStateAction
{
    private CoroutineID _load;
    private PlayerInput _input;
    private GameplayBase _gameplay;
    private ResResult _sceneAsset;

    protected override void OnAwake()
    {
        base.OnAwake();
        _input = new PlayerInput();
        GameEntry.Context.PlayerInput = _input;
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        GameEntry.GUI.SetParam(GameEntry.GUIPath.MatchUI.Path, "start");
        GameEntry.GUI.Close(GameEntry.GUIPath.OfflineRewardUI.Path);
        var tpl = GameEntry.Table.Map.Get(GameEntry.Context.SelectMapId);
        var mapType = (EnumDefine.MapTypeEnum)tpl.SceneType;
        if (mapType == EnumDefine.MapTypeEnum.Competition || mapType==MapTypeEnum.PVP)
        {
            _gameplay = new GameplayRace();
        }
        else
        {
            return;
        }
        GameEntry.Context.Gameplay = _gameplay;
        _gameplay.SetMapId(GameEntry.Context.SelectMapId);

        GameEntry.Entity.Clear();
        GameEntry.Context.LoadingShow = true;
        GameEntry.GUI.Open(GameEntry.GUIPath.LoadingUI.Path, LoadingUI.SetTotalTime, 3f);
        _load = GameEntry.Coroutine.Start(load());
        GameEntry.Http.Handler.EnterMap(_gameplay);

    }

    protected override void OnExit()
    {
        base.OnExit();
        _gameplay.Exit();
        GameEntry.Coroutine.Stop(_load);
        if (_sceneAsset != null)
        {
            GameEntry.AssetsLoader.Unload(_sceneAsset);
            GameEntry.AssetsLoader.Unload();
            _sceneAsset = null;
        }
        GameEntry.Http.Handler.GetChangeDataSystemData();
    }
    IEnumerator load()
    {
        GameEntry.Context.Offline.LoadMapStart();
        yield return null;
        Debug.Log("load map " + _gameplay.TableData.Id);
        var result = GameEntry.AssetsLoader.LoadAsset(_gameplay.TableData.Scene);
        yield return result;
        _sceneAsset = result;
        while (!result.Result.SceneAsset.isDone)
        {
            yield return null;
        }
        result.SetSceneActive();
        _gameplay.Enter();
        while (!_gameplay.AssetsReady)
        {
            yield return null;
        }
        GameEntry.Context.Offline.LoadMapEnd();
    }
    protected override void OnUpdate(float deltaTime, float progress)
    {
        base.OnUpdate(deltaTime, progress);
        _gameplay?.Update();
    }

    protected override void OnEditorUpdate(float deltaTime)
    {
        base.OnEditorUpdate(deltaTime);
        _gameplay?.OnEditorUpdate();
    }
}