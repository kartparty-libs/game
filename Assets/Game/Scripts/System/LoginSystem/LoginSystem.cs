using System;
using System.Collections;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Framework
{

    public class LoginSystem : BaseSystem<LoginSystem>
    {
        // ----------------------------------------------------------------------------------------
        private const int SingleReConnectTime = 5;
        private const int TotalReConnectTime = 60;
        private const float LoginTimeout = 6f;

        private const float HeartBeatTime = 5;
        private string m_sAccountId = "";
        private bool m_bConnectSuc = false;
        private bool m_bPlayerLogin = false;

        private bool m_bReConnect = false;
        private float m_nSingleReConnectTime = -1;
        private float m_nTotalReConnectTime = -1;
        private float m_nHeartBeatTime = HeartBeatTime;
        private float m_nOnHeartTime = 15;
        protected Thread m_pThreadLogin;
        public override void OnStart()
        {
            EventManager.Instance.Regist(EventDefine.Global.OnConnectSuc, OnConnectSuc);
            EventManager.Instance.Regist(EventDefine.Global.OnConnectFail, OnConnectFail);
            EventManager.Instance.Regist(EventDefine.Global.OnDisconnect, OnDisconnect);
            EventManager.Instance.Regist(EventDefine.Global.OnConnectClose, OnConnectClose);
        }
        public override bool IsUpdate() { return true; }
        public override void OnUpdate(float i_nDelay)
        {
            if (m_bReConnect)
            {
                m_nSingleReConnectTime = m_nSingleReConnectTime - i_nDelay;
                m_nTotalReConnectTime = m_nTotalReConnectTime - i_nDelay;

                if (m_nSingleReConnectTime <= 0)
                {
                    m_nSingleReConnectTime = SingleReConnectTime;
                    Debug.Log("m_bReConnect totalTime " + m_nTotalReConnectTime);
                    PlayerLogin(m_sAccountId);
                }
                if (m_nTotalReConnectTime <= 0)
                {
                    m_bReConnect = false;
                    Debug.Log("m_nTotalReConnectTime lesseq 0f");
                }
            }
            if (m_bConnectSuc && m_bPlayerLogin && !m_bReConnect)
            {
                UpdateHeartBeat(i_nDelay);
            }
        }
        private CoroutineID _loginTimeoutID;
        IEnumerator loginTimeout()
        {
            yield return new WaitForSeconds(LoginTimeout);
            Debug.LogError("登陆超时:" + LoginTimeout);
            _loginTimeoutID = null;
            m_bConnectSuc = false;
            m_bReConnect = true;
            m_nSingleReConnectTime = 2;
            m_nTotalReConnectTime = TotalReConnectTime;

        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void PlayerLogin(string i_sAccountId, bool fromUI = false)
        {
            var customIp = PlayerPrefs.GetString("ip");
            var host = GlobalDefine.Network_ServerHost;
            var port = GlobalDefine.Network_ServerPort;
            if (!string.IsNullOrEmpty(customIp))
            {
                var hp = customIp.Split(":");
                if (hp.Length == 2)
                {
                    if (int.TryParse(hp[1], out port))
                    {
                        host = hp[0].Trim();
                    }
                }
            }
            m_sAccountId = i_sAccountId;
            if (m_bConnectSuc && !fromUI)
            {
                var pLoginData = new JObject();
                pLoginData.Add("selServerIP", host);
                pLoginData.Add("selServerPort", port);
                pLoginData.Add("serverid", GlobalDefine.Network_ServerId);
                pLoginData.Add("mac", "");
                pLoginData.Add("openid", i_sAccountId);
                pLoginData.Add("pf", 1);
                pLoginData.Add("channelCode", "editor");
                pLoginData.Add("ui", fromUI ? 1 : 0);

                GameEntry.Net.Send(CtoS.K_EnterKSReqMsg, pLoginData);

                _loginTimeoutID = GameEntry.Coroutine.Start(loginTimeout());
            }
            else
            {
                GameEntry.Net.Connect(new NetworkAddress(host, port));
            }
        }

        /// <summary>
        /// 玩家注销
        /// </summary>
        public void PlayerLogoff()
        {
            if (!m_bPlayerLogin) return;
            m_bPlayerLogin = false;
            GameEntry.Net.Send(CtoS.K_LeaveKSReqMsg);
        }



        /// <summary>
        /// 服务器连接成功事件
        /// </summary>
        public void OnConnectSuc()
        {
            if (m_bReConnect)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 10);
            }
            Debug.Log("OnConnectSuc");
            m_bConnectSuc = true;
            m_bReConnect = false;
            PlayerLogin(m_sAccountId);
        }

        /// <summary>
        /// 服务器连接失败事件
        /// </summary>
        public void OnConnectFail()
        {
            if (m_bPlayerLogin)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 11);
            }
            else
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 12);
            }
        }



        /// <summary>
        /// 服务器连接关闭
        /// </summary>
        public void OnConnectClose()
        {
            m_bConnectSuc = false;
            if (m_bPlayerLogin)
            {
                m_bReConnect = true;
                m_nSingleReConnectTime = SingleReConnectTime;
                m_nTotalReConnectTime = TotalReConnectTime;
            }
        }

        /// <summary>
        /// 服务器连接断开
        /// </summary>
        public void OnDisconnect()
        {
            Debug.LogError("OnDisconnect " + m_bPlayerLogin);

            if (m_bPlayerLogin)
            {
                m_bConnectSuc = false;
                m_bReConnect = true;
                m_nSingleReConnectTime = 2;
                m_nTotalReConnectTime = TotalReConnectTime;
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 11);
            }

        }

        /// <summary>
        /// 刷新心跳
        /// </summary>
        public void UpdateHeartBeat(float i_nDelay)
        {
            m_nHeartBeatTime = m_nHeartBeatTime - i_nDelay;
            m_nOnHeartTime = m_nOnHeartTime - i_nDelay;
            if (m_nHeartBeatTime <= 0)
            {
                SendHeartBeat();
            }
            if (m_nOnHeartTime <= 0)
            {
                m_nOnHeartTime = HeartBeatTime;
                OnDisconnect();
            }
        }
        /// <summary>
        /// 发送心跳
        /// </summary>
        public void SendHeartBeat()
        {
            m_nHeartBeatTime = HeartBeatTime;
            GameEntry.Net.Send(CtoS.K_HeartBeat);
        }

        /// <summary>
        /// 心跳通知
        /// </summary>
        public void OnHeartBeat()
        {
            m_nHeartBeatTime = HeartBeatTime;
            m_nOnHeartTime = 15;
        }
    }
}
