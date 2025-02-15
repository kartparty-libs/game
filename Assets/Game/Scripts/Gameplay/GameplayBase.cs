
using UnityEngine;

public class GameplayBase
{
    public bool PveMode = true;
    public Map_Data TableData { get; protected set; }
    //游戏时间
    public float RaceTime { get; protected set; }
    //玩家时间
    public float PlayTime { get; protected set; }
    //玩家是否可以操控
    public bool PlayerActive { get; protected set; }
    public bool Playing { get; protected set; }
    public int LoopCount { get; protected set; }
    public bool AssetsReady { get; protected set; }
    public int Rank { get; protected set; }
    private float _gameStartTime;
    private byte _state;
    protected bool _userQuit;
    public void SetMapId(int mapId)
    {
        PlayerActive = false;
        TableData = GameEntry.Table.Map.Get(mapId);
        LoopCount = TableData.Loop;
        if (LoopCount < 1)
        {
            LoopCount = 1;
        }
        Rank = -1;
    }
    public virtual bool ExecuteComponent(Transform value)
    {
        return false;
    }
    public void Enter()
    {
        if (_state > 0)
        {
            return;
        }
        _state = 1;
        Rank = -1;
        OnEnter();
    }
    public void Exit()
    {
        if (_state != 1)
        {
            return;
        }
        _state = 2;
        OnExit();
    }
    //主动退出
    public virtual void Quit()
    {
        _userQuit = true;
        if (GameEntry.Context.MatchMode)
        {
            GameEntry.Match.Handler.LeaveRoom();
        }
    }
    public void Update()
    {
        if (_state != 1)
        {
            return;
        }
        RaceTime = Time.time - _gameStartTime;
        if (PlayerActive)
        {
            PlayTime = RaceTime;
        }
        OnUpdate();
    }
    public void GameStart()
    {
        if (Playing)
        {
            return;
        }
        _gameStartTime = Time.time;
        Playing = true;
    }
    public void GameEnd()
    {
        if (!Playing)
        {
            return;
        }
        Playing = false;
    }
    protected virtual void OnEnter()
    {

    }
    protected virtual void OnExit()
    {

    }
    protected virtual void OnUpdate()
    {

    }
    protected virtual void OnGameStart()
    {

    }
    protected virtual void OnGameEnd()
    {

    }
    public virtual void OnEditorUpdate()
    {

    }

}