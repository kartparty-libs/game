using System.Collections;
using Framework;
using UnityEngine;
//碰碰车
public class GameplayDodgems : GameplayBase
{
    public MatchData MatchData { get; private set; } = new MatchData();
    private bool raceFinished;
    private CoroutineID _load;
    private CoroutineID _showResult;
    public override bool ExecuteComponent(Transform value)
    {
        if (value.name == "Dodgems")
        {
            return true;
        }
        return base.ExecuteComponent(value);
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        EventManager.Instance.Regist<EnumDefine.CompetitionMapState>(EventDefine.Global.OnCompetitionGameState, OnCompetitionGameState);
        GameEntry.Context.ServerCompetitionMapState = EnumDefine.CompetitionMapState.ReadyGame;
        PlayerActive = false;
        _load = GameEntry.Coroutine.Start(load());
    }
    protected override void OnExit()
    {
        base.OnExit();
        GameEntry.Coroutine.Stop(_load);
        GameEntry.Coroutine.Stop(_showResult);
        EventManager.Instance.Remove<EnumDefine.CompetitionMapState>(EventDefine.Global.OnCompetitionGameState, OnCompetitionGameState);


        GameEntry.GUI.Close(GameEntry.GUIPath.MainUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.RankUI.Path);


        CharacterManager.Instance.ClearCharacter();
        // MatchData.Clear();

        GameEntry.Net.Send(CtoS.K_PlayerLeaveMapReq);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.LoadingUI.Path, LoadingUI.SetLoadComplete);
    }
    public override void Quit()
    {
        base.Quit();
        _userQuit = true;
        if (_showResult == null)
        {

        }
        GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
    }
    private void OnCompetitionGameState(EnumDefine.CompetitionMapState i_eGameState)
    {
        if (i_eGameState == EnumDefine.CompetitionMapState.EndGame)
        {
            this.EndGame();
        }
        Debug.Log("OnCompetitionGameState -> " + i_eGameState);
    }
    private void EndGame()
    {
        if (!raceFinished)
        {
            gameFinish();
        }
    }

    IEnumerator load()
    {
        while (GameEntry.Context.SceneConfig == null)
        {
            yield return null;
        }

        foreach (var item in MatchSystem.Instance.GetMatchPlayerDatas())
        {
            CharacterManager.Instance.CreateCharacter(item.Value);
        }

        while (CharacterManager.Instance.GetCharacters().Count < MatchSystem.Instance.GetMatchCurrNum())
        {
            yield return null;
        }
        this.AssetsReady = true;
        GameEntry.Context.IsHasNetWorkCharacter = CharacterManager.Instance.IsHasNetWorkCharacter();

        GameEntry.Net.Send(CtoS.K_PlayerLoadMapCompleteReq);
        Debug.LogError("K_PlayerLoadMapCompleteReq");
        float waitTime = 20f;
        while (GameEntry.Context.ServerCompetitionMapState != EnumDefine.CompetitionMapState.StartGame)
        {
            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
                yield break;
            }
            yield return null;
        }
        Debug.LogError("StartGame");
        GameEntry.GUI.SetParam(GameEntry.GUIPath.LoadingUI.Path, LoadingUI.SetLoadComplete);
        raceFinished = false;
        MatchData.Start(GameEntry.Context.SelectMapId);




        GameEntry.GUI.Open(GameEntry.GUIPath.MainUI.Path);
        while (GameEntry.Context.LoadingShow)
        {
            yield return null;
        }
        while (!GameEntry.GUI.IsOpen(GameEntry.GUIPath.MainUI.Path))
        {
            yield return null;
        }
        GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Start);
        // GameEntry.GUI.Open(GameEntry.GUIPath.RankUI.Path);
        yield return new WaitForSeconds(2.5f);
        GameEntry.Context.FollowPlayer.CharacterInputController.SetInput(GameEntry.Context.PlayerInput);
        yield return new WaitForSeconds(1.0f);
        this.PlayerActive = true;
        GameEntry.Context.SceneConfig.GameStart();
        this.GameStart();
    }

    IEnumerator showResult()
    {
        GameEntry.Context.FollowPlayer.EngineShutdown();
        yield return new WaitForSeconds(1f);
        GameEntry.Context.FollowPlayer.OnCompleteGame(true);
        var len = MatchData.MatchCharacterCount;
        var selfId = PlayerSystem.Instance.GetUID();
        var hasWin = false;
        while (len-- > 0)
        {
            var d = MatchData.GetCharacterData(len);
            if (d != null && d.RoleID == selfId)
            {
                hasWin = true;
            }
        }
        if (hasWin)
        {
            GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Win);
        }
        while (GameEntry.Context.ServerCompetitionMapState != EnumDefine.CompetitionMapState.EndGame)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        GameEntry.GUI.Open(GameEntry.GUIPath.RankUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.MainUI.Path);
        _showResult = null;
        if (_userQuit)
        {
            GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
        }
    }
    /// <summary>
    /// 玩家完成比赛
    /// </summary>
    protected void PlayerCompleteGame(float seconds = 600f)
    {
        PlayerActive = false;
        if (!raceFinished)
        {
            raceFinished = true;
            GameEntry.Context.SceneConfig.Focus();
            GameEntry.Context.FollowPlayer.OnCompleteGame(true);
            _showResult = GameEntry.Coroutine.Start(showResult());
            var myTime = Mathf.FloorToInt(seconds * 10000);
            GameEntry.Net.Send(CtoS.K_PlayerCompleteGame, myTime);
        }
    }
    private void gameFinish()
    {
        if (PlayerActive)
        {
            this.PlayerActive = false;
            GameEntry.Context.SceneConfig.Focus();
            GameEntry.Context.FollowPlayer.OnCompleteGame(true);
            _showResult = GameEntry.Coroutine.Start(showResult());
            this.GameEnd();
        }
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (this.Playing)
        {
            if (GameEntry.Context.FollowPlayer != null)
            {
                if (CheckPointManager.Instance != null)
                {
                    var status = CheckPointManager.Instance.GetStatus(GameEntry.Context.FollowPlayer.m_ColliderForCheckpoint);
                    if (status != null)
                    {
                        if (!raceFinished)
                        {
                            if (status.lapsCompleted >= LoopCount)
                            {
                                var seconds = this.PlayTime;
                                if (seconds > 60f * 40f)
                                {
                                    seconds = 60 * 40f;
                                }
                                PlayerCompleteGame(seconds);
                            }
                        }
                    }
                }
            }
        }
    }
    public override void OnEditorUpdate()
    {
        base.OnEditorUpdate();
        if (Input.GetKey(KeyCode.C))
        {
            PlayerCompleteGame();
        }
        if (Input.GetKey(KeyCode.E))
        {
            GameEntry.Context.FollowPlayer.Boost();
        }
        if (Input.GetKey(KeyCode.K))
        {
            GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameLogin.ToString());
        }
    }

}