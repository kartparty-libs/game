using System.Collections;
using Framework;
using UnityEngine;
//竞速
public class GameplayRace : GameplayBase
{
    public MatchData MatchData { get; private set; }
    private bool myRaceFinished;
    private CoroutineID _load;
    private CoroutineID _showResult;
    public GameplayRace()
    {
        MatchData = new MatchData();
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
        GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);

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
        if (!myRaceFinished)
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

        GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.LoadingUI.Path, LoadingUI.SetLoadComplete);
        myRaceFinished = false;
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
        //等3秒
        yield return new WaitForSeconds(3f);



        if (GameEntry.Context.MatchMode)
        {
            MatchSystem.Instance.CompleteLoadMap();
            float waitTime = 30f;
            while (!MatchSystem.Instance.IsBattle && !MatchSystem.Instance.IsStartBattle)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
                    MatchSystem.Instance.Error();
                    yield break;
                }
                yield return null;
            }
            if (MatchSystem.Instance.IsStartBattle)
            {
                GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Start);
            }
            waitTime = 30f;
            while (!MatchSystem.Instance.IsBattle)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    MatchSystem.Instance.Error();
                    GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
                    yield break;
                }
                yield return null;
            }
            MatchSystem.Instance.RaceingBegin();
            GameEntry.Context.FollowPlayer.CharacterInputController.SetInput(GameEntry.Context.PlayerInput);
            this.PlayerActive = true;
            GameEntry.Context.SceneConfig.GameStart();
            this.GameStart();
            GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Go);
        }
        else
        {
            Debug.Log("StartGame");
            //单机
            // GameEntry.GUI.Open(GameEntry.GUIPath.RankUI.Path);
            GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Start);
            yield return new WaitForSeconds(2.5f);
            GameEntry.Context.FollowPlayer.CharacterInputController.SetInput(GameEntry.Context.PlayerInput);
            yield return new WaitForSeconds(1.0f);
            this.PlayerActive = true;
            GameEntry.Context.SceneConfig.GameStart();
            this.GameStart();
            GameEntry.GUI.SetParam(GameEntry.GUIPath.MainUI.Path, MainUI.Go);
        }
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
            {
                if (d != null && d.RoleID == selfId)
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
        if (!myRaceFinished)
        {
            Debug.LogError("PlayerCompleteGame");
            myRaceFinished = true;
            GameEntry.Context.SceneConfig.Focus();
            GameEntry.Context.FollowPlayer.OnCompleteGame(true);
            _showResult = GameEntry.Coroutine.Start(showResult());
            var roleId = PlayerSystem.Instance.GetUID();
            MatchSystem.Instance.PlayerFinishRace(roleId, seconds, false);

            /*
            foreach (var item in MatchSystem.Instance.GetMatchPlayerDatas())
            {
                if (item.Value.roleId == roleId)
                {
                    MatchData.SetWaitTime(10000f);
                    MatchCharacterData data = new MatchCharacterData();
                    data.Time = 60000;
                    data.RoleID = item.Value.roleId;
                    data.Name = item.Value.playerName;
                    data.RemainTime = 5000;
                    MatchData.CharacterFinish(data);
                }
            }

            var len = MatchData.MatchCharacterCount;
            for (int i = 0; i < len; i++)
            {
                if (MatchData.GetItem(i).RoleID == roleId)
                {
                    this.Rank = i + 1;
                    var tpl = TableData;
                    if (i == 0)
                    {
                        //第一名
                        PlayerPrefs.SetFloat(VehicleAI.SpeedEffectValue + tpl.Id, 0f);
                    }
                    else
                    {

                        var starLv = TaskSystem.Instance.GetMapStarCount(tpl.Id);
                        var startpl = GameEntry.Table.MapStar.Get(tpl.Id, starLv);
                        var effectvalue = PlayerPrefs.GetFloat(VehicleAI.SpeedEffectValue + tpl.Id, 0f);
                        effectvalue += startpl.SubValue;
                        PlayerPrefs.SetFloat(VehicleAI.SpeedEffectValue + tpl.Id, effectvalue);
                    }
                    break;
                }
            }
            */
        }
    }
    private void gameFinish()
    {
        if (PlayerActive)
        {
            this.PlayerActive = false;
            Debug.LogError("gameFinish");
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
                        if (!myRaceFinished)
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
            // PlayerCompleteGame();
        }
        if (Input.GetKey(KeyCode.K))
        {
        }
    }
    public float GetAIMaxSpeed()
    {
        var starLv = TaskSystem.Instance.GetMapStarCount(this.TableData.Id);
        var heroSpeed = CarCultivateSystem.Instance.GetCarSpeed();
        var maxSpeed = GameEntry.Table.MapStar.GetValue(heroSpeed, TableData.Id, starLv);
        var effectvalue = PlayerPrefs.GetFloat(VehicleAI.SpeedEffectValue + TableData.Id, 0f);
        if (effectvalue < 0)
        {
            effectvalue = 0;
        }
        if (effectvalue > maxSpeed - 1f)
        {
            effectvalue = maxSpeed - 1f;
        }
        maxSpeed -= effectvalue;
        return maxSpeed;
    }
    public void SetRank(int value)
    {
        var tpl = TableData;
        if (value == 1)
        {
            //第一名
            PlayerPrefs.SetFloat(VehicleAI.SpeedEffectValue + tpl.Id, 0f);
        }
        else
        {
            var starLv = TaskSystem.Instance.GetMapStarCount(tpl.Id);
            var startpl = GameEntry.Table.MapStar.Get(tpl.Id, starLv);
            var effectvalue = PlayerPrefs.GetFloat(VehicleAI.SpeedEffectValue + tpl.Id, 0f);
            effectvalue += startpl.SubValue;
            PlayerPrefs.SetFloat(VehicleAI.SpeedEffectValue + tpl.Id, effectvalue);
        }
    }
}