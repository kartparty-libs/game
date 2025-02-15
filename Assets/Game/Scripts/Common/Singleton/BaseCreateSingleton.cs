
namespace Framework
{
    /// <summary>
    /// 创建单例基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public class BaseCreateSingleton<T_0> : BaseSingleton<T_0> where T_0 : BaseCreateSingleton<T_0>, new()
    {
        // ----------------------------------------------------------------------------------------
        // 单例基类 - 替换

        public new static T_0 Instance
        {
            get
            {
                return s_pInstance;
            }
        }

        // ----------------------------------------------------------------------------------------
        // 创建单例基类

        /// <summary>
        /// 创建单例
        /// </summary>
        public static T_0 CreateSingleton()
        {
            s_pInstance = new T_0();
            return s_pInstance;
        }
    }
}
