using System;

namespace Framework
{
    /// <summary>
    /// 管理器基类接口
    /// </summary>
    public interface IBaseManager
    {
        /// <summary>
        /// 唤起通知
        /// </summary>
        void OnAwake();

        /// <summary>
        /// 初始化通知
        /// </summary>
        void OnInit();

        /// <summary>
        /// 加载通知
        /// </summary>
        void OnLoad(Action i_fCallback);

        /// <summary>
        /// 启动通知
        /// </summary>
        void OnStart();

        /// <summary>
        /// 销毁通知
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// 是否物理刷新
        /// </summary>
        /// <returns></returns>
        bool IsFixedUpdate();

        /// <summary>
        /// 物理刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        void OnFixedUpdate(float i_nDelay);

        /// <summary>
        /// 是否逻辑刷新
        /// </summary>
        /// <returns></returns>
        bool IsUpdate();

        /// <summary>
        /// 逻辑刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        void OnUpdate(float i_nDelay);

        /// <summary>
        /// 是否渲染刷新
        /// </summary>
        /// <returns></returns>
        bool IsLateUpdate();

        /// <summary>
        /// 渲染刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        void OnLateUpdate(float i_nDelay);
    }

    /// <summary>
    /// 管理器基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public abstract class BaseManager<T_0> : BaseRegistSingleton<T_0>, IBaseManager where T_0 : BaseManager<T_0>
    {
        // ----------------------------------------------------------------------------------------
        // 管理器基类

        /// <summary>
        /// 唤起通知
        /// </summary>
        public virtual void OnAwake() { }

        /// <summary>
        /// 初始化通知
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// 加载通知
        /// </summary>
        public virtual void OnLoad(Action i_fCallback)
        {
            UnityEngine.Debug.Log(this.GetType().Name + "_OnLoad");
            if (i_fCallback != null) { i_fCallback(); }
        }

        /// <summary>
        /// 启动通知
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// 销毁通知
        /// </summary>
        /// <returns></returns>
        public virtual void OnDestroy() { }

        /// <summary>
        /// 是否物理刷新
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFixedUpdate() { return false; }

        /// <summary>
        /// 物理刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        public virtual void OnFixedUpdate(float i_nDelay) { }

        /// <summary>
        /// 是否逻辑刷新
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUpdate() { return false; }

        /// 逻辑刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        public virtual void OnUpdate(float i_nDelay) { }

        /// <summary>
        /// 是否渲染刷新
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLateUpdate() { return false; }

        /// <summary>
        /// 渲染刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        public virtual void OnLateUpdate(float i_nDelay) { }
    }
}