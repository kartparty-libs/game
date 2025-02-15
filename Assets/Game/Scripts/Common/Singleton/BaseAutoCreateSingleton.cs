
namespace Framework
{
    /// <summary>
    /// 自动注册单例基类
    /// </summary>
    /// <typeparam name="T_0"></typeparam>
    public class BaseAutoCreateSingleton<T_0> : BaseCreateSingleton<T_0> where T_0 : BaseAutoCreateSingleton<T_0>, new()
    {
        // ----------------------------------------------------------------------------------------
        // 单例基类 - 替换

        public new static T_0 Instance
        {
            get
            {
                if (s_pInstance == null)
                {
                    CreateSingleton();
                }
                return s_pInstance;
            }
        }
    }
}
