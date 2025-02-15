
using Framework;

/// <summary>
/// 系统基类接口
/// </summary>
public interface IBaseSystem
{
    /// <summary>
    /// 唤起通知
    /// </summary>
    void OnAwake();

    /// <summary>
    /// 启动通知
    /// </summary>
    void OnStart();

    /// <summary>
    /// 销毁通知
    /// </summary>
    /// <returns></returns>
    void OnDestroy();

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
    /// 每日刷新通知
    /// </summary>
    void OnRefresh_EveryDay();
}

/// <summary>
/// 系统基类
/// </summary>
/// <typeparam name="T_0"></typeparam>
public abstract class BaseSystem<T_0> : BaseCreateSingleton<T_0>, IBaseSystem where T_0 : BaseSystem<T_0>, new()
{
    // ----------------------------------------------------------------------------------------
    // 系统基类

    /// <summary>
    /// 唤起通知
    /// </summary>
    public virtual void OnAwake() { }

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
    /// 是否逻辑刷新
    /// </summary>
    /// <returns></returns>
    public virtual bool IsUpdate() { return false; }

    /// <summary>
    /// 逻辑刷新通知
    /// </summary>
    /// <param name="i_nDelay"></param>
    public virtual void OnUpdate(float i_nDelay) { }

    /// <summary>
    /// 每日刷新通知
    /// </summary>
    public virtual void OnRefresh_EveryDay() { }
}
