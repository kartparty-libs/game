
namespace Framework
{
    /// <summary>
    /// 注册单例基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public class BaseRegistSingleton<T_0> : BaseSingleton<T_0> where T_0 : BaseRegistSingleton<T_0>
    {
        // ----------------------------------------------------------------------------------------
        // 注册单例基类

        /// <summary>
        /// 注册单例
        /// </summary>
        public static T_0 RegistSingleton(T_0 i_pSingleton)
        {
            s_pInstance = i_pSingleton;
            return s_pInstance;
        }
    }
}
