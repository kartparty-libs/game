using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class NetworkManagerForCommon : NetworkManager
    {
        protected Socket m_pSocket;
        protected int m_nTimeOut = 5000;
        protected byte[] m_pHead;
        protected short m_nDataLen;
        protected byte m_nSequence = 0;
        protected Thread m_pThreadReceive;
        protected Thread m_pThreadSend;
        protected AutoResetEvent m_pAutoEvent = new AutoResetEvent(false);
        protected Queue m_pPacketSendQueue = Queue.Synchronized(new Queue());
        protected Queue m_pPacketReceiveQueue = Queue.Synchronized(new Queue());
        public override bool IsConnect { get { return m_pSocket != null && m_pSocket.Connected; } }

        public override bool IsUpdate()
        {
            return true;
        }

        public override void OnUpdate(float i_nDelay)
        {
            lock (m_pPacketReceiveQueue)
            {
                while (m_pPacketReceiveQueue.Count > 0)
                {
                    if (!(m_pPacketReceiveQueue.Dequeue() is NetDataPacket pNetDataPacket))
                    {
                        Debug.LogError("网络异常: NetDataPacket为空包!");
                        continue;
                    }

                    if (pNetDataPacket.Length == 0)
                    {
                        Debug.LogError("网络异常: 数据长度为0!");
                        continue;
                    }

                    if (pNetDataPacket.Data == null)
                    {
                        Debug.LogError("网络异常: 数据为null!");
                        continue;
                    }

                    Dispatch(pNetDataPacket.Length, pNetDataPacket.Data);
                }
            }
        }

        public override void OnDestroy()
        {
            Clear();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="i_pNetworkAddress"></param>
        public override void Connect(NetworkAddress i_pNetworkAddress)
        {
            Debug.Log("请求连接：" + " -> " + i_pNetworkAddress.host + " -> " + i_pNetworkAddress.point);

            if (m_pSocket != null)
            {
                Clear();
            }

            OnConnecting();

            //采用TCP方式连接
            m_pSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //服务器IP地址
            IPAddress pAddress;
            if (this.IsIP(i_pNetworkAddress.host))
            {
                pAddress = IPAddress.Parse(i_pNetworkAddress.host);
            }
            else
            {
                // 域名需解析主机名
                IPHostEntry pIPHostEntry = Dns.GetHostEntry(i_pNetworkAddress.host);
                pAddress = pIPHostEntry?.AddressList[0];
            }

            //服务器端口
            IPEndPoint pEndPoint = new IPEndPoint(pAddress, i_pNetworkAddress.point);
            //异步连接
            IAsyncResult pResult = m_pSocket.BeginConnect(pEndPoint, null, m_pSocket);
            //超时监测
            bool success = pResult.AsyncWaitHandle.WaitOne(m_nTimeOut, true);
            if (!success)
            {
                Debug.Log("连接失败：" + " -> " + i_pNetworkAddress.host + " -> " + i_pNetworkAddress.point);
                //关闭
                Clear();
                OnConnectFail();
            }
            else
            {
                if (IsConnect)
                {
                    //与socket建立连接成功，开启线程接收服务端数据
                    m_pThreadReceive = new Thread(new ThreadStart(ReceiveThread))
                    {
                        IsBackground = true
                    };
                    m_pThreadReceive.Start();
                    // 与socket建立连接成功，开启线程发送数据到服务端
                    m_pThreadSend = new Thread(new ThreadStart(SendThread))
                    {
                        IsBackground = true
                    };
                    m_pThreadSend.Start();

                    //打通网关
                    m_nSequence = 0;
                    m_pSocket.Send(Converter.String2ByteArray("tgw_l7_forward\r\nHost:" + pAddress.ToString() + ":" + i_pNetworkAddress.point + "\r\n\r\n\0"));
                    OnConnectSuc();
                }
                else
                {
                    Debug.Log("连接关闭：" + " -> " + i_pNetworkAddress.host + " -> " + i_pNetworkAddress.point);
                    //关闭
                    Clear();
                    OnConnectFail();
                }
            }
        }

        /// <summary>
        /// 接收消息线程
        /// </summary>
        private void ReceiveThread()
        {
            while (IsConnect)
            {
                if (!IsConnect)
                {
                    //与服务器断开连接跳出循环
                    Debug.Log("网络连接断开！");
                    OnDisconnect();
                    Clear();
                    return;
                }
                try
                {
                    //读取消息头部
                    m_pHead = new byte[2];
                    m_pSocket.Receive(m_pHead);
                    //消息包体长度
                    m_nDataLen = Converter.ByteArray2Short(m_pHead, 0);
                    // 读取消息体
                    byte[] pRecvBytesBody = new byte[m_nDataLen];
                    short nBodyLen = m_nDataLen;
                    if (nBodyLen == 0) continue;
                    // 当前需要接收的字节数>0,则循环接收
                    while (nBodyLen > 0)
                    {
                        byte[] pRecvBytes = new byte[nBodyLen < 1024 ? nBodyLen : 1024];
                        int bIsReadBytesBody = 0;
                        if (nBodyLen >= pRecvBytes.Length)
                        {
                            bIsReadBytesBody = m_pSocket.Receive(pRecvBytes, pRecvBytes.Length, 0);
                        }
                        else
                        {
                            bIsReadBytesBody = m_pSocket.Receive(pRecvBytes, nBodyLen, 0);
                        }
                        pRecvBytes.CopyTo(pRecvBytesBody, pRecvBytesBody.Length - nBodyLen);
                        nBodyLen -= (short)bIsReadBytesBody;
                    }
                    // 插入消息队列
                    lock (m_pPacketReceiveQueue)
                    {
                        m_pPacketReceiveQueue.Enqueue(new NetDataPacket(m_nDataLen, pRecvBytesBody));
                    }
                }
                catch (ThreadAbortException) { }
                catch (Exception e)
                {
                    Debug.LogError("网络连接错误：" + e);
                    OnDisconnect();
                    Clear();
                    break;
                }
            }
        }

        /// <summary>
        /// 发送消息线程
        /// </summary>
        private void SendThread()
        {
            // 循环读取数据
            while (IsConnect)
            {
                //阻止发送线程循环操作，为了减少系统消耗.
                try
                {
                    m_pAutoEvent.WaitOne();
                }
                catch (System.Exception)
                {
                    OnDisconnect();
                    Clear();
                    return;
                }

                if (!IsConnect)
                {
                    Debug.Log("网络连接断开！");
                    OnDisconnect();
                    Clear();
                    return;
                }
                try
                {
                    lock (m_pPacketSendQueue)
                    {
                        while (m_pPacketSendQueue.Count > 0)
                        {
                            NetDataPacket pNetDataPacket = m_pPacketSendQueue.Dequeue() as NetDataPacket;
                            byte[] nLength = Converter.Short2ByteArray((short)pNetDataPacket.Data.Length);
                            // 发送
                            m_pSocket.Send(nLength, SocketFlags.None);
                            m_pSocket.Send(pNetDataPacket.Data, SocketFlags.None);
                        }
                    }
                }
                catch (SocketException e)
                {
                    Debug.LogError("网络消息发送错误：" + e);
                    OnDisconnect();
                    Clear();
                    break;
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="i_pSendData"></param>
        /// <param name="i_pSendDataLength"></param>
        public override void Send(byte[] i_pSendData, int i_pSendDataLength)
        {
            if (!IsConnect)
            {
                // Debug.Log("网络连接断开！");
                // OnDisconnect();
                // Clear();
                return;
            };
            try
            {
                //取出发送数据的实际长度
                int nRealLen = i_pSendDataLength + 1;
                //拷贝实际数据
                byte[] pNewData = new byte[nRealLen];
                for (int i = 1; i < nRealLen; i++)
                {
                    pNewData[i] = i_pSendData[i - 1];
                };
                pNewData[0] = m_nSequence;
                // 插入到发送队列
                lock (m_pPacketSendQueue)
                {
                    m_pPacketSendQueue.Enqueue(new NetDataPacket((short)nRealLen, pNewData));
                    //开启发送线程的操作
                    m_pAutoEvent.Set();
                }
                //设置消息队列码
                m_nSequence++;
            }
            catch (SocketException e)
            {
                Debug.LogError("网络消息发送错误：" + e);
            }
        }

        /// <summary>
        /// 派发消息
        /// </summary>
        private void Dispatch(int i_nMsgLength, byte[] i_pMsgData)
        {
            //解码
            NetAnalysis.Decode(i_nMsgLength, i_pMsgData);
            int nCount = NetAnalysis.ReceiveData.Count;
            if (nCount <= 0) return;
            List<object> pReceiveData = NetAnalysis.ReceiveData;
            if (pReceiveData.Count <= 0)
            {
                return;
            }

            OnReceive(pReceiveData[0].ToString(), nCount, Utility.UtilityMethod.CopyObjectList(pReceiveData));
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close()
        {
            OnConnectClose();
            Clear();
        }

        /// <summary>
        /// 清理
        /// </summary>
        private void Clear()
        {
            try
            {
                if (m_pThreadReceive != null)
                {
                    m_pThreadReceive.Abort();
                    m_pThreadReceive = null;
                }
                if (m_pThreadSend != null)
                {
                    m_pThreadSend.Abort();
                    m_pThreadSend = null;
                }

                if (m_pSocket != null)
                {
                    if (m_pSocket.Connected)
                    {
                        m_pSocket.Shutdown(SocketShutdown.Both);
                    }
                    m_pSocket.Close();
                    m_pSocket.Dispose();
                    m_pSocket = null;
                }

                if (m_pPacketSendQueue != null)
                {
                    m_pPacketSendQueue.Clear();
                    m_pPacketSendQueue = Queue.Synchronized(new Queue());
                }

                if (m_pPacketReceiveQueue != null)
                {
                    m_pPacketReceiveQueue.Clear();
                    m_pPacketReceiveQueue = Queue.Synchronized(new Queue());
                }

                if (m_pAutoEvent != null)
                {
                    m_pAutoEvent.Close();
                    m_pAutoEvent.Dispose();
                    m_pAutoEvent = new AutoResetEvent(false);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("出错:" + e.Message);
            }

        }
    }
}
