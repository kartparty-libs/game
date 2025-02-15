
namespace Framework
{
    /// <summary>
    /// 全局定义
    /// </summary>
    public class GlobalDefine
    {
        // ----------------------------------------------------------------------------------------
        // 客户端定义

        /// <summary>
        /// 配置布局
        /// </summary>
        public static int ConfigLayout = EnumDefine.ConfigLayout.BinaryLayout;

        // ----------------------------------------------------------------------------------------
        // 配置定义

        /// <summary>
        /// 预配置文件名
        /// </summary>
        public const string Config_Include = "IncludeCfg_C";

        // ----------------------------------------------------------------------------------------
        // 资源目录定义

        /// <summary>
        /// 配置目录路径
        /// </summary>
        public const string Config_FilePath = "Res/Configs/";

        /// <summary>
        /// JSON配置文件扩展名
        /// </summary>
        public const string Config_FileExtendForJSON = ".json";

        /// <summary>
        /// 二进制配置文件扩展名
        /// </summary>
        public const string Config_FileExtendForBinary = ".bytes";

        // ----------------------------------------------------------------------------------------
        // 网络定义
#if UNITY_EDITOR_OSX
        public const string Network_ServerHost = "test-game-alb-4db1c468c3d30d1a.elb.ap-southeast-1.amazonaws.com"; // 外网
#elif UNITY_EDITOR
        /// <summary>
        /// 网络_服务器地址
        /// </summary>
        // public const string Network_ServerHost = "192.168.1.56"; // 本地
        public const string Network_ServerHost = "127.0.0.1"; // 本地
#else
        // public const string Network_ServerHost = "152.136.200.148"; // 外网
        public const string Network_ServerHost = "test-game-alb-4db1c468c3d30d1a.elb.ap-southeast-1.amazonaws.com"; // 外网
#endif
#if UNITY_EDITOR_OSX
    public const int Network_ServerPort = 8888; // 外网
#elif UNITY_EDITOR
        /// <summary>
        /// 网络_服务器端口
        /// </summary>
        public const int Network_ServerPort = 9885; // 本地
#else
        public const int Network_ServerPort = 8888; // 外网
#endif

#if UNITY_EDITOR
        /// <summary>
        /// 网络_服务器Id
        /// </summary>
        public const int Network_ServerId = 1; // 本地
#else
        public const int Network_ServerId = 1; // 外网
#endif
    }
}
