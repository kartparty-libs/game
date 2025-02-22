using System;
using System.Collections.Generic;
using Proto;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 匹配系统
    /// </summary>
    public class MatchSystem : BaseSystem<MatchSystem>
    {
        private const int idle = 0;
        private const int match_players = 1;
        private const int loading = 2;
        private const int raceing = 3;
        private const int exit = 4;
        private const int race_finished = 5;

        private long m_nRoodId = 0;
        private int m_nMapCfgId = 0;
        private int m_nPlayerNum = 0;
        public int RoomMaxPlayerCount { get; private set; }

        private Dictionary<long, CharacterCreateData> m_tMatchPlayerDatas = new Dictionary<long, CharacterCreateData>();
        public float m_nStartTime { get; private set; }
        public bool matchFinished { get; private set; }
        public long Owner { get; private set; }
        public int GetMapCfgId() => m_nMapCfgId;
        public Dictionary<long, CharacterCreateData> GetMatchPlayerDatas() => m_tMatchPlayerDatas;
        public int GetMatchCurrNum() => m_tMatchPlayerDatas.Count;
        public bool IsStartBattle { get; private set; }
        public bool IsBattle { get; private set; }
        private DateTime StartBattleTime;
        private bool waitForStartBattle;
        public override bool IsUpdate() { return true; }
        private int selectMapId;
        private int matchState;
        private bool hasStart;
        public override void OnAwake()
        {
            base.OnAwake();
            matchState = idle;
            EventManager.Instance.Regist(EventDefine.Global.OnMatchServerConnected, OnMatchServerConnected);
        }
        public void SetSelectMap(int mapId)
        {
            selectMapId = mapId;
            if (mapId < 0)
            {
                matchState = exit;
            }
            else
            {
                matchState = idle;
            }
            hasStart = false;
        }
        private void OnMatchServerConnected()
        {
            if (matchState == idle)
            {
                if (selectMapId != -1)
                {
                    RequestMatch(selectMapId);
                }
            }
            else if (matchState == exit)
            {

            }
            else
            {
                GameEntry.Match.Handler.ReConnect();
            }

        }

        public override void OnUpdate(float i_nDelay)
        {
            base.OnUpdate(i_nDelay);

            if (matchFinished && m_nStartTime > 0)
            {
                m_nStartTime -= i_nDelay;
                if (m_nStartTime <= 0)
                {
                    m_nStartTime = 0;
                    matchState = loading;
                    GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameWorld.ToString());
                }
            }
            if (waitForStartBattle)
            {
                if (TimeUtils.GetServerTime() > StartBattleTime)
                {
                    waitForStartBattle = false;
                    IsBattle = true;
                }
            }
        }

        /// <summary>
        /// 请求匹配
        /// </summary>
        private void RequestMatch(int i_nMapCfgId)
        {
            GameEntry.Context.MatchMode = true;
            this.m_tMatchPlayerDatas.Clear();
            this.matchFinished = false;
            GameEntry.Match.Handler.MatchMap(i_nMapCfgId);
        }

        /// <summary>
        /// 取消匹配
        /// </summary>
        public void EndMateCompe(bool isPve = false)
        {
            if (!isPve)
            {
                GameEntry.Match.Handler.LeaveRoom();
            }
            matchFinished = false;
            matchState = exit;
        }
        public void KickPlayer(long id)
        {
            GameEntry.Match.Handler.KickPlayer(id);
        }
        public void EnterRoom(ResMsgBodyPlayerEnterRoom res)
        {
            this.m_nRoodId = res.RoomData.InstId;
            UpdateRoomData(res.RoomData);
            GameEntry.GUI.Close(GameEntry.GUIPath.MatchEntryUI.Path);
            GameEntry.GUI.Open(GameEntry.GUIPath.MatchUI.Path);
            matchState = match_players;
        }
        public void UpdateRoomInfo(ResMsgBodyRoomDataChange res)
        {
            UpdateRoomData(res.RoomData);
        }
        public void RaceingBegin()
        {
            matchState = raceing;
        }
        public void RaceingEnd()
        {
            matchState = race_finished;
        }
        public void Error()
        {
            matchState = exit;
        }
        public void ReConnectServer(ResMsgBodyReconnect res)
        {
            var roomstate = res.RoomData.RoomState;
            if (roomstate == RoomStateEnum.RoomStateInBattle || roomstate == RoomStateEnum.RoomStateEndBattle)
            {
                if (matchState == match_players || matchState == loading)
                {
                    matchState = exit;
                    GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
                    return;
                }
            }
        }
        private void UpdateRoomData(RoomData value)
        {
            this.m_tMatchPlayerDatas.Clear();
            this.m_nRoodId = value.InstId;
            m_nMapCfgId = value.MapCfgId;
            GameEntry.Context.SelectMapId = m_nMapCfgId;
            RoomMaxPlayerCount = value.RoomMaxPlayerCount;
            Owner = value.RoomOwnerPlayer;
            foreach (var item in value.Players)
            {
                var data = new CharacterCreateData();
                data.uid = item.RoleId;
                data.characterCarCfgId = 1;
                data.playerName = item.Name;
                data.playerTrackIdx = item.TrackIdx - 1;
                data.characterRoleCfgId = item.RoleCfgId;
                data.head = item.HeadId;
                data.IsRoomOwner = item.IsRoomOwner;
                data.isReady = item.IsReady;
                data.RankTiersId = item.RankTiersCfgId;
                m_tMatchPlayerDatas.Add(data.uid, data);
                if (item.RoleId == PlayerSystem.Instance.GetUID())
                {
                    data.characterTypeEnum = EnumDefine.CharacterTypeEnum.MainCharacter;
                }
                else if (item.RobotMasterRoleId == PlayerSystem.Instance.GetUID())
                {
                    data.characterTypeEnum = EnumDefine.CharacterTypeEnum.VehicleAI;
                }
                else
                {
                    data.characterTypeEnum = EnumDefine.CharacterTypeEnum.NetWorkCharacter;
                }
                if (item.IsRobot)
                {
                    // String[] strings = item.Name.Split("+");
                    // int nameId1 = Convert.ToInt32(strings[0]);
                    // int nameId2 = Convert.ToInt32(strings[1]);
                    // var d1 = GameEntry.Table.Nickname.GetItem(nameId1);
                    // var d2 = GameEntry.Table.Nickname.GetItem(nameId2);
                    // if (d1 == null)
                    // {
                    //     d1 = GameEntry.Table.Nickname.GetItem(0);
                    // }
                    // if (d2 == null)
                    // {
                    //     d2 = GameEntry.Table.Nickname.GetItem(0);
                    // }

                    // data.playerName = $"{d1.FirstName}{d2.LastName}";
                    data.playerName = GetRobotName(item.Name);
                }
            }
            IsBattle = value.RoomState == RoomStateEnum.RoomStateInBattle;
            IsStartBattle = value.RoomState == RoomStateEnum.RoomStateStartBattle;
            if (value.RoomState == RoomStateEnum.RoomStateEndBattle)
            {
                if (GameEntry.Context.Gameplay is GameplayRace race)
                {
                    race.MatchData.PlayerAllFinishRace();
                }
            }
            GameEntry.GUIEvent.DispatchEvent(GUIEvent.OnRoomPlayerInfoChange);
            // Debug.LogError("UpdateRoomInfo");
            if (value.RoomState == RoomStateEnum.RoomStateEndMatch)
            {
                OnStartMateCompeComplete();
            }
            else if (IsStartBattle && !IsBattle)
            {
                var diff = TimeUtils.GetDiff(TimeUtils.GetServerTime(), StartBattleTime);
                Debug.LogError("wait time before battle:" + diff.TotalSeconds.ToString());
                if (diff.TotalSeconds > 0)
                {
                    StartBattleTime = TimeUtils.GetDateTimeByMilliseconds(value.SwicthRoomStateTime);
                    waitForStartBattle = true;
                }
            }
            hasStart = value.RoomState == RoomStateEnum.RoomStateEndMatch || value.RoomState == RoomStateEnum.RoomStateStartBattle || value.RoomState == RoomStateEnum.RoomStateEndBattle || value.RoomState == RoomStateEnum.RoomStateInBattle;
        }
        public void SetPveMatchMapId(int value, int role, int car)
        {
            m_nMapCfgId = value;
            GameEntry.Context.SelectMapId = m_nMapCfgId;
            m_tMatchPlayerDatas.Clear();
            var cfg = GameEntry.Table.Map.Get(value);
            var count = cfg.LineMaxPlayerNum - 1;
            var pCharacterData = new CharacterCreateData()
            {
                uid = PlayerSystem.Instance.GetUID(),
                playerName = PlayerSystem.Instance.GetName(),
                playerTrackIdx = 0,
                characterRoleCfgId = role,
                characterCarCfgId = car,
                head = PlayerSystem.Instance.GetHead(),
                IsRoomOwner = true,
                isReady = true,
                characterTypeEnum = EnumDefine.CharacterTypeEnum.MainCharacter
            };
            m_tMatchPlayerDatas.Add(pCharacterData.uid, pCharacterData);
            int k = 1;
            while (count-- > 0)
            {
                pCharacterData = new CharacterCreateData()
                {
                    playerName = GameEntry.OfflineManager.GetRandomNickName(),
                    playerTrackIdx = count + 1,
                    characterRoleCfgId = GameEntry.OfflineManager.GetRandomRoleId(),
                    characterCarCfgId = GameEntry.OfflineManager.GetRandomCarId(),
                    head = GameEntry.OfflineManager.GetRandomHeadId(),
                    IsRoomOwner = false,
                    isReady = true,
                    characterTypeEnum = EnumDefine.CharacterTypeEnum.VehicleAI,
                };
                pCharacterData.uid = k++;
                m_tMatchPlayerDatas.Add(pCharacterData.uid, pCharacterData);
            }

        }

        public CharacterCreateData GetPlayerInfo(long id)
        {
            foreach (var item in m_tMatchPlayerDatas)
            {
                if (item.Value.uid == id)
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 匹配完成
        /// </summary>
        public void OnStartMateCompeComplete(bool started = true)
        {
            this.matchFinished = started;
            this.m_nStartTime = 3;
            if (started)
            {
                GameEntry.GUIEvent.DispatchEvent(GUIEvent.OnStartMateCompeComplete);
            }
        }

        public void CompleteLoadMap()
        {
            if (GameEntry.Context.MatchMode)
            {
                GameEntry.Match.Handler.PlayerLoadMapComplete();
            }
        }
        public void PlayerFinishRace(long roleId, float seconds, bool isRobot)
        {
            //只有自己和ai
            if (GameEntry.Context.Gameplay is GameplayRace race)
            {
                var myTime = Mathf.FloorToInt(seconds * 10000);

                if (GameEntry.Context.MatchMode)
                {
                    GameEntry.Match.Handler.PlayerFinishRace(roleId, myTime);
                }
                else
                {
                    if (m_tMatchPlayerDatas.TryGetValue(roleId, out var info))
                    {
                        MatchCharacterData data = new MatchCharacterData();
                        if (isRobot)
                        {
                            data.Time = Mathf.FloorToInt(race.RaceTime * 10000);
                        }
                        else
                        {
                            data.Time = myTime;
                        }
                        data.Goal = true;
                        data.RoleID = info.uid;
                        data.Name = info.playerName;
                        race.MatchData.CharacterFinish(data);
                        race.MatchData.SetWaitTime(5000);
                    }
                }
                if (PlayerSystem.Instance.GetUID() == roleId)
                {
                    var len = race.MatchData.MatchCharacterCount;
                    for (int i = 0; i < len; i++)
                    {
                        if (race.MatchData.GetItem(i).RoleID == roleId)
                        {
                            if (!GameEntry.Context.MatchMode)
                            {
                                GameEntry.Http.Handler.FinishMap(race, i + 1, myTime);
                            }
                            race.SetRank(i + 1);
                            break;
                        }
                    }
                }
                // GameEntry.OfflineManager.PlayerFinish(roleId, myTime);
            }
        }

        public void OnPlayerFinishSync(ResMsgBodyPlayerCompleteGame value)
        {
            if (GameEntry.Context.Gameplay is GameplayRace race)
            {
                if (m_tMatchPlayerDatas.TryGetValue(value.BattlePlayerResultData.RoleId, out var info))
                {
                    MatchCharacterData data = new MatchCharacterData();
                    data.Time = value.BattlePlayerResultData.Time;
                    data.RoleID = info.uid;
                    data.Name = info.playerName;
                    race.MatchData.CharacterFinish(data);
                    race.MatchData.SetWaitTime(value.BattlePlayerResultData.RemainTime);
                }
            }
        }
        public bool HasStart()
        {
            return hasStart;
        }
        public string GetRobotName(string nameid)
        {
            String[] strings = nameid.Split("+");
            int nameId1;
            int nameId2;
            if (strings.Length == 2)
            {
                nameId1 = Convert.ToInt32(strings[0]);
                nameId2 = Convert.ToInt32(strings[1]);
            }
            else
            {
                nameId1 = UnityEngine.Random.Range(0, GameEntry.Table.Nickname.ItemCount);
                nameId2 = UnityEngine.Random.Range(0, GameEntry.Table.Nickname.ItemCount);
            }
            var d1 = GameEntry.Table.Nickname.GetItem(nameId1);
            var d2 = GameEntry.Table.Nickname.GetItem(nameId2);
            if (d1 == null)
            {
                d1 = GameEntry.Table.Nickname.GetItem(0);
            }
            if (d2 == null)
            {
                d2 = GameEntry.Table.Nickname.GetItem(0);
            }

            return $"{d1.FirstName}{d2.LastName}";
        }
        public float GetAiSpeed(int rankId)
        {
            var tpl = GameEntry.Table.RankTiers.Get(rankId);
            if (tpl == null)
            {
                tpl = GameEntry.Table.RankTiers.MinLevelData;
            }
            return UnityEngine.Random.Range(tpl.AISpeedMin, tpl.AISpeedMax);
        }
    }
}