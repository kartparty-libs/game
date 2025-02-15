using System;

namespace Utility
{
    /// <summary>
    /// 异步调用计数器
    /// </summary>
    public class AsyncCounter
    {
        /// <summary>
        /// 总计数
        /// </summary>
        private int _m_nTotal;
        public int GetTotal() { 
            return _m_nTotal; }
        public void SetTotal(int i_nTotal) { _m_nTotal = i_nTotal; }

        /// <summary>
        /// 已记计数
        /// </summary>
        private int _m_nTally;
        public int GetTally() { 
            return _m_nTally; }

        /// <summary>
        /// 抵达回调
        /// </summary>
        private Action _m_fCallBack;
        public void SetCallback(Action i_fCallBack) { _m_fCallBack = i_fCallBack; }

        /// <summary>
        /// 是否开始计数
        /// </summary>
        private bool _m_bIsBegin = false;

        /// <summary>
        /// 重置计数
        /// </summary>
        public void Reset()
        {
            _m_nTotal = 0;
            _m_nTally = 0;
            _m_fCallBack = null;
            _m_bIsBegin = false;
        }

        /// <summary>
        /// 检查计数
        /// </summary>
        private void Check()
        {
            if (_m_bIsBegin && _m_nTally >= _m_nTotal)
            {
                Reach();
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
        /// 增加计数
        /// </summary>
        public void Increase()
        {
            _m_nTally++;
            Check();
        }

        /// <summary>
        /// 开始计数
        /// </summary>
        public void Begin()
        {
            _m_bIsBegin = true;
            Check();
        }
    }
}
