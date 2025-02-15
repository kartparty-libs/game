using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Framework
{
    public class CSharpNetworkHandler : NetworkHandler<CSharpNetworkHandler>
    {
        protected Dictionary<string, MethodInfo> m_pReceiver = new Dictionary<string, MethodInfo>();

        public virtual void RegistReceiver(string i_sMsgFunc, MethodInfo i_fReceiver)
        {
            m_pReceiver.Add(i_sMsgFunc, i_fReceiver);
        }

        public override void OnInitialize()
        {
            // 注册 StoC 下所有的静态公共方法
            Type pStoCType = typeof(StoC);
            MethodInfo[] pMethodInfos = pStoCType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            for (int i = 0; i < pMethodInfos.Length; i++)
            {
                RegistReceiver(pMethodInfos[i].Name, pMethodInfos[i]);
            }
        }

        public override void Send(params object[] i_pArgs)
        {
            return;
            if (sendPrepare(i_pArgs))
            {
                NetworkManager.Instance.Send(NetAnalysis.SendData, NetAnalysis.SendDataLength);
            }
        }
        private bool sendPrepare(params object[] i_pArgs)
        {
            object[] pParameters = i_pArgs;
            for (int i = 0; i < pParameters.Length; i++)
            {
                if (pParameters[i] == null ||
                    pParameters[i].GetType() == typeof(int) ||
                    pParameters[i].GetType() == typeof(short) ||
                    pParameters[i].GetType() == typeof(long) ||
                    pParameters[i].GetType() == typeof(float) ||
                    pParameters[i].GetType() == typeof(double) ||
                    pParameters[i].GetType() == typeof(bool) ||
                    pParameters[i].GetType() == typeof(string) ||
                    pParameters[i].GetType() == typeof(JArray) ||
                    pParameters[i].GetType() == typeof(JObject))
                {
                    if (pParameters[i].GetType() == typeof(JArray) || pParameters[i].GetType() == typeof(JObject))
                    {
                        pParameters[i] = "_t" + JsonConvert.SerializeObject(pParameters[i]);
                    }
                    else if (pParameters[i].GetType() == typeof(float))
                    {
                        pParameters[i] = (double)pParameters[i];
                    }
                }
                else
                {
                    Debug.LogError("存在网络消息不支持的参数类型！");
                    return false;
                }
            }
            NetAnalysis.CSharpEncode(pParameters);
            return true;
        }

        public override void OnReceive(string i_sMsgFunc, int nCount, List<object> i_pArgs)
        {
            if (!m_pReceiver.TryGetValue(i_sMsgFunc, out MethodInfo fOutReceiveListener))
            {
                return;
            }

            //转换table数据类型
            for (int i = 1; i < i_pArgs.Count; i++)
            {
                object param = i_pArgs[i];
                if (param == null || param.GetType() != typeof(string))
                {
                    continue;
                }
                string str = param.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                string first = str.Substring(0, 3);
                if (first == "_t[")
                {
                    string json = str.Substring(2, str.Length - 2);
                    i_pArgs[i] = JsonConvert.DeserializeObject(json);
                }
                else if (first == "_t{")
                {
                    string json = str.Substring(2, str.Length - 2);
                    i_pArgs[i] = JsonConvert.DeserializeObject<JObject>(json);
                }
            }
            i_pArgs.RemoveAt(0);
            // Debug.LogWarning(i_sMsgFunc + " : " + i_pArgs.Count);
#if UNITY_EDITOR
            try
            {
                object[] t = i_pArgs.ToArray();
                fOutReceiveListener.Invoke(null, t);
            }
            catch (System.Exception e)
            {
                Debug.LogError("CSharpNetworkHandler.OnReceive" + " -> " + i_sMsgFunc + " -> " + e.ToString());
            }
#else
            fOutReceiveListener.Invoke(null, i_pArgs.ToArray());
#endif
        }
    }
}
