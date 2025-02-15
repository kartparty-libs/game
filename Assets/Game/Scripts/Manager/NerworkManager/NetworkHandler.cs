using System.Collections.Generic;

namespace Framework
{
    public interface INetworkHandler
    {
        void OnInitialize();
        void OnConnecting();

        void OnConnectSuc();

        void OnConnectFail();

        void OnConnectClose();

        void OnDisconnect();

        void Send(params object[] i_pArgs);

        void OnReceive(string i_sMsgFunc, int nCount, List<object> i_pArgs);
    }

    public abstract class NetworkHandler<CT0> : BaseAutoCreateSingleton<CT0>, INetworkHandler where CT0 : NetworkHandler<CT0>, new()
    {
        public virtual void OnInitialize()
        {

        }

        public virtual void OnConnecting()
        {

        }

        public virtual void OnConnectSuc()
        {
            EventManager.Instance.Dispatch(EventDefine.Global.OnConnectSuc);
        }

        public virtual void OnConnectFail()
        {
            EventManager.Instance.Dispatch(EventDefine.Global.OnConnectFail);
        }

        public virtual void OnConnectClose()
        {
            EventManager.Instance.Dispatch(EventDefine.Global.OnConnectClose);
        }

        public virtual void OnDisconnect()
        {
            EventManager.Instance.Dispatch(EventDefine.Global.OnDisconnect);
        }

        public virtual void Send(params object[] i_pArgs)
        {
            
        }

        public virtual void OnReceive(string i_sMsgFunc, int nCount, List<object> i_pArgs)
        {

        }
    }
}
