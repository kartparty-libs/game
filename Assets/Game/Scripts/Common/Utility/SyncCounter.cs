using System;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// 同步调用计数器
    /// </summary>
    public class SyncCounter
    {
        /// <summary>
        /// 调用容器
        /// </summary>
        /// <typeparam name="Action"></typeparam>
        /// <returns></returns>
        private Queue<Action> _m_pSyncAction = new Queue<Action>();

        /// <summary>
        /// 完成回调
        /// </summary>
        private Action _m_fCallBack;

        /// <summary>
        /// 设置完成回调
        /// </summary>
        /// <param name="i_fCallBack"></param>
        public void SetCallBack(Action i_fCallBack) { _m_fCallBack = i_fCallBack; }

        /// <summary>
        /// 是否抵达计数
        /// </summary>
        /// <returns></returns>
        private bool IsReach()
        {
            if (_m_pSyncAction.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 抵达计数
        /// </summary>
        private void Reach()
        {
            if (_m_fCallBack != null)
            {
                _m_fCallBack();
            }
            Reset();
        }

        /// <summary>
        /// 重置计数
        /// </summary>
        public void Reset()
        {
            _m_pSyncAction.Clear();
            _m_fCallBack = null;
        }

        /// <summary>
        /// 开始计数
        /// </summary>
        public void Begin()
        {
            if (IsReach())
            {
                Reach();
            }
            else
            {
                _m_pSyncAction.Dequeue();
                // Framework.TimerManager.Instance.OnceFrame(1, _m_pSyncAction.Dequeue());
            }
        }

        public void Add_0(Action<Action> i_pAsyncCall)
        {
            _m_pSyncAction.Enqueue(delegate ()
            {
                i_pAsyncCall.Invoke(Begin);
            });
        }

        public void Add_1<FT0>(Action<FT0, Action> i_pAsyncCall, FT0 i_pParam0)
        {
            _m_pSyncAction.Enqueue(delegate ()
            {
                i_pAsyncCall.Invoke(i_pParam0, Begin);
            });
        }

        public void Add_2<FT0, FT1>(Action<FT0, FT1, Action> i_pAsyncCall, FT0 i_pParam0, FT1 i_pParam1)
        {
            _m_pSyncAction.Enqueue(delegate ()
            {
                i_pAsyncCall.Invoke(i_pParam0, i_pParam1, Begin);
            });
        }

        public void Add_3<FT0, FT1, FT2>(Action<FT0, FT1, FT2, Action> i_pAsyncCall, FT0 i_pParam0, FT1 i_pParam1, FT2 i_pParam2)
        {
            _m_pSyncAction.Enqueue(delegate ()
            {
                i_pAsyncCall.Invoke(i_pParam0, i_pParam1, i_pParam2, Begin);
            });
        }
    }
}
