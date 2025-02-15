using System;

namespace Framework
{
    /// <summary>
    /// 支持基类接口
    /// </summary>
    public interface IBaseSupport
    {
        /// <summary>
        /// 唤起通知
        /// </summary>
        void OnAwake();

        /// <summary>
        /// 加载通知
        /// </summary>
        void OnLoad(Action i_fCallback);

        /// <summary>
        /// 销毁通知
        /// </summary>
        /// <returns></returns>
        void OnDestroy();
    }

    /// <summary>
    /// 支持基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public abstract class BaseSupport<T_0> : BaseCreateSingleton<T_0>, IBaseSupport where T_0 : BaseSupport<T_0>, new()
    {
        // ----------------------------------------------------------------------------------------
        // 支持基类

        /// <summary>
        /// 唤起通知
        /// </summary>
        public virtual void OnAwake() { }

        /// <summary>
        /// 加载通知
        /// </summary>
        public virtual void OnLoad(Action i_fCallback) { if (i_fCallback != null) { i_fCallback(); } }

        /// <summary>
        /// 销毁通知
        /// </summary>
        /// <returns></returns>
        public virtual void OnDestroy() { }
    }
}










