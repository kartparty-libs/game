
namespace Framework
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public class BaseSingleton<T_0> where T_0 : BaseSingleton<T_0>
    {
        // ----------------------------------------------------------------------------------------
        // 单例基类

        /// <summary>
        /// 单例
        /// </summary>
        protected static T_0 s_pInstance;

        /// <summary>
        /// 单例获取
        /// </summary>
        /// <value></value>
        public static T_0 Instance
        {
            get
            {
                return s_pInstance;
            }
        }
    }
}
