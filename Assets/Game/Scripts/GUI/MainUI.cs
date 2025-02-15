
using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MainUI : UIWindowBase
{
    public const string Start = nameof(Start);
    public const string Win = nameof(Win);
    public const string Go = nameof(Go);
    public const string Boost = nameof(Boost);
    private float _time;
    private List<MainRankItem> rankItems;
    private List<CharacterInfo> characterInfos = new List<CharacterInfo>();
    private float _boostCd;
    private float Boost_CD = 4f;
    private TMPro.TextMeshProUGUI _rankName;

    private MatchData MatchData;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.joystick_VariableJoystick.OnEnd = () =>
        {
            GameEntry.Context.FollowPlayer.CharacterInputController.m_Steer = Vector3.zero;
        };
        this.joystick_VariableJoystick.OnHold = (v) =>
        {
            if (GameEntry.Context.Gameplay.PlayerActive)
            {
                GameEntry.Context.FollowPlayer.CharacterInputController.m_Steer.Set(v.x * 2f, 0, v.y * 2f);
            }
            else
            {
                GameEntry.Context.FollowPlayer.CharacterInputController.m_Steer = Vector3.zero;
            }

        };
        this.drag_UIDragBehaviour.OnDragAction = (v) =>
        {
            GameEntry.Context.FollowPlayer.CharacterCameraController.m_CameraSteer = v;
            GameEntry.Context.CameraInputValue = v;
        };
        rankItems = new List<MainRankItem>();
        var goclone = this.list_RectTransform.GetChild(0);
        var len = this.list_RectTransform.childCount;
        for (int i = 0; i < 3; i++)
        {
            var go = GameObject.Instantiate(goclone);
            go.transform.SetParent(this.list_RectTransform, false);
            var w = go.GetComponent<UGUIWidget>().Widget;
            if (w is MainRankItem rankItem)
            {
                rankItems.Add(rankItem);
                rankItem.SetRank(i + 1);
                //rankItem.Widget.gameObject.SetActive(false);
            }
        }
        goclone.gameObject.SetActive(false);
        _rankName = this.loopTxt_TextMeshProUGUI.transform.parent.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        _time = 0;
        _boostCd = 0f;
        this.loopTxt_TextMeshProUGUI.text = "1";
        this.win_GameObject.SetActive(false);
        this.exit_GameObject.SetActive(false);
        this.endTime_TextMeshProUGUI.gameObject.SetActive(false);
        var len = this.n1_RectTransform.childCount;
        while (len-- > 0)
        {
            this.n1_RectTransform.GetChild(len).gameObject.SetActive(false);
            this.n2_RectTransform.GetChild(len).gameObject.SetActive(false);
        }
        this.time_TextMeshProUGUI.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
        MatchData = null;
        if (GameEntry.Context.Gameplay is GameplayRace race)
        {
            MatchData = race.MatchData;
        }
        this.wait_GameObject.SetActive(false);
    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == MainUI.Start)
        {
            this.wait_GameObject.SetActive(true);
            GameEntry.Timer.Start(4f, () =>
            {
                Utils.Unity.SetActive(wait_GameObject, false);
            }, 1);
        }
        else if (name == MainUI.Win)
        {
            this.win_GameObject.SetActive(true);
        }
        else if (name == MainUI.Go)
        {
            _boostCd = 0.001f;
        }
        else if (name == MainUI.Boost)
        {
            doBoost();
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
        this.wait_GameObject.SetActive(false);
        this.exit_GameObject.SetActive(false);
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (GameEntry.Context.EnableRotation)
        {
            joystick_VariableJoystick.Rotation = Screen.height > Screen.width;
        }
        if (GameEntry.Context.FollowPlayer != null)
        {
            var currentSpeed = GameEntry.Context.FollowPlayer.CharacterMovementController.m_Speed;
            this.speed_TextMeshProUGUI.text = CarCultivateSystem.Instance.GetShowSpeed(currentSpeed).ToString();
            this.power_Image.fillAmount = GameEntry.Context.FollowPlayer.CharacterMovementController.m_Speed / GameEntry.Context.FollowPlayer.CharacterMovementController.m_MovementSpeed;
        }
        _time += deltaTime;

        if (_time > 1f)
        {
            _time -= 1f;
            if (MatchData != null && MatchData.ShowFinishTime)
            {
                Utils.Unity.SetActive(this.endTime_TextMeshProUGUI, true);
                var time = Mathf.FloorToInt(MatchData.GetFinishWaitTime());
                this.endTime_TextMeshProUGUI.text = string.Empty;
                int n1 = time / 10;
                int n2 = time % 10;
                var nlen = this.n1_RectTransform.childCount;
                while (nlen-- > 0)
                {
                    Utils.Unity.SetActive(this.n1_RectTransform.GetChild(nlen), nlen == n1);
                    Utils.Unity.SetActive(this.n2_RectTransform.GetChild(nlen), nlen == n2);
                }
                //Utils.Unity.SetActive(this.n1_RectTransform.GetChild(n1), true);
                //Utils.Unity.SetActive(this.n2_RectTransform.GetChild(n2), true);
                if (time < 1)
                {
                    if (GameEntry.Context.OfflineMode)
                    {
                        GameEntry.Context.ServerCompetitionMapState = EnumDefine.CompetitionMapState.EndGame;
                        EventManager.Instance.Dispatch(EventDefine.Global.OnCompetitionGameState, GameEntry.Context.ServerCompetitionMapState);
                    }
                }
            }
            else
            {
                Utils.Unity.SetActive(this.endTime_TextMeshProUGUI, false);
            }


        }
        var myTime = 0f;

        if (GameEntry.Context.Gameplay != null)
        {
            myTime = GameEntry.Context.Gameplay.PlayTime;
        }
        if (myTime <= 0)
        {
            myTime = 0f;
        }
        var ts = new TimeSpan(0, 0, (int)myTime);
        this.time_TextMeshProUGUI.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, Mathf.FloorToInt((myTime % 1f) * 100f));
        characterInfos.Clear();
        if (MatchData != null)
        {
            var list = CharacterManager.Instance.GetCharacters();
            foreach (var item in list)
            {
                var info = item.Value.GetCharacterInfo();
                var finishInfo = MatchData.GetByRoleId(info.RoleId);
                if (finishInfo != null)
                {
                    info.LockRank = finishInfo.Time - 100000000;
                }
                else
                {
                    info.LockRank = info.Rank;
                }
                characterInfos.Add(info);
            }
            characterInfos.Sort(SortRank);
            var len = characterInfos.Count;
            for (int i = 0; i < len; i++)
            {
                var info = characterInfos[i];
                info.LockRank = i + 1;
                if (characterInfos[i].RoleId == PlayerSystem.Instance.GetUID())
                {
                    this.loopTxt_TextMeshProUGUI.text = characterInfos[i].LockRank.ToString();
                    var r = characterInfos[i].LockRank;
                    string n = "TH";
                    if (r == 1)
                    {
                        n = "ST";
                    }
                    else if (r == 2)
                    {
                        n = "ND";
                    }
                    else if (r == 3)
                    {
                        n = "RD";
                    }
                    this._rankName.text = n;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                var rankInfo = this.rankItems[i];
                if (i >= characterInfos.Count)
                {
                    Utils.Unity.SetActive(rankInfo.Widget.gameObject, false);
                    continue;
                }
                Utils.Unity.SetActive(rankInfo.Widget.gameObject, true);
                rankInfo.SetData(characterInfos[i]);

            }
        }

        if (_boostCd > 0)
        {
            _boostCd -= Time.deltaTime;
        }
        var p = Mathf.Clamp01(_boostCd / Boost_CD);
        this.boostcd_Image.fillAmount = p;
        if (Input.GetKey(KeyCode.E))
        {
            doBoost();
        }

    }
    private int SortRank(CharacterInfo a, CharacterInfo b)
    {
        return a.LockRank - b.LockRank;
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.close_Button)
        {
            Application.Quit();
        }
        else if (target == this.jump_Button)
        {
            GameEntry.Context.FollowPlayer.Jump();
        }
        else if (target == this.boost_Button)
        {
            doBoost();
        }
        else if (target == this.exit_Button)
        {
            this.exit_GameObject.SetActive(true);
            Time.timeScale = 0f;
            GameEntry.Audio.SetFloat(AudioDefine.MasterVolume, -40f);
        }
        else if (target == this.yes_Button)
        {
            Time.timeScale = 1f;
            GameEntry.Audio.SetFloat(AudioDefine.MasterVolume, 0f);
            GameEntry.Context.Gameplay.Quit();

        }
        else if (target == this.cancel_Button)
        {
            this.exit_GameObject.SetActive(false);
            Time.timeScale = 1f;
            GameEntry.Audio.SetFloat(AudioDefine.MasterVolume, 0f);
        }
    }
    private void doBoost()
    {
        if (_boostCd > 0)
        {
            return;
        }
        _boostCd = CarCultivateSystem.Instance.GetNosCdTime();
        Boost_CD = _boostCd;
        GameEntry.Context.FollowPlayer.Boost();
    }
}