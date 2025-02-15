using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Framework
{
    public abstract class NetworkManager : BaseManager<NetworkManager>
    {
        protected INetworkHandler[] m_pNetworkHandles;
        private bool _disconnect;

        public override void OnAwake()
        {
            m_pNetworkHandles = new INetworkHandler[]{
                CSharpNetworkHandler.Instance
            };
            OnInitialize();
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <value></value>
        public abstract bool IsConnect { get; }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="i_pNetworkAddress"></param>
        public abstract void Connect(NetworkAddress i_pNetworkAddress);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="i_pSendData"></param>
        /// <param name="i_pSendDataLength"></param>
        public abstract void Send(byte[] i_pSendData, int i_pSendDataLength);

        /// <summary>
        /// 关闭
        /// </summary>
        public abstract void Close();

        protected virtual void OnInitialize()
        {
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnInitialize();
            }
        }

        protected virtual void OnConnecting()
        {
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnConnecting();
            }
        }

        protected virtual void OnConnectSuc()
        {
            _disconnect = false;
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnConnectSuc();
            }
        }

        protected virtual void OnConnectFail()
        {
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnConnectFail();
            }
        }

        protected virtual void OnConnectClose()
        {
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnConnectClose();
            }
        }

        protected virtual void OnDisconnect()
        {
            if (_disconnect)
            {
                return;
            }
            Debug.LogError("OnDisconnect");
            _disconnect = true;
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnDisconnect();
            }
        }

        protected virtual void OnReceive(string i_sMsgFunc, int nCount, List<object> i_pArgs)
        {
            for (int i = 0; i < m_pNetworkHandles.Length; i++)
            {
                m_pNetworkHandles[i].OnReceive(i_sMsgFunc, nCount, i_pArgs);
            }
        }

        /// <summary>
        /// 判断是域名还是ip
        /// </summary>
        /// <param name="i_sHost"></param>
        /// <returns></returns>
        protected bool IsIP(string i_sHost)
        {
            Match pMatch = Regex.Match(i_sHost, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            return pMatch.Success;
        }
    }

    public class NetDataPacket
    {
        public short Length;
        public byte[] Data;

        public NetDataPacket(short length, byte[] data)
        {
            Length = length;
            Data = data;
        }
    }

    /// <summary>
    /// 网络地址
    /// </summary>
    public class NetworkAddress
    {
        private string _m_sStringValue0;
        private int _m_nIntValue0;
        public string host { get { return this._m_sStringValue0; } }
        public int point { get { return this._m_nIntValue0; } }


        public NetworkAddress(string i_sHost, int i_nPoint)
        {
            this._m_sStringValue0 = i_sHost;
            this._m_nIntValue0 = i_nPoint;
        }
    }
}
