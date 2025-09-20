using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
using Utility;

namespace Framework
{
    /// <summary>
    /// 注册定义
    /// </summary>
    public class RegistDefine
    {
        /// <summary>
        /// 配置注册
        /// </summary>
        public static void ConfigRegist()
        {

        }

        /// <summary>
        /// 管理器注册
        /// </summary>
        public static void ManagerRegist(List<IBaseManager> i_pManagerList)
        {
            // 事件管理器
            i_pManagerList.Add(EventManager.RegistSingleton(new EventManager()));

            // 网络管理器
            i_pManagerList.Add(NetworkManager.RegistSingleton(new NetworkManagerForCommon()));

            // 角色管理器
            i_pManagerList.Add(CharacterManager.RegistSingleton(new CharacterManager()));

            // 本地存储管理器
            i_pManagerList.Add(StorageManager.RegistSingleton(new StorageManager()));
        }

        /// <summary>
        /// 系统注册
        /// </summary>
        public static void SystemRegist(List<IBaseSystem> i_pSystemList)
        {
            // 登录系统
            i_pSystemList.Add(LoginSystem.CreateSingleton());

            // 玩家系统
            i_pSystemList.Add(PlayerSystem.CreateSingleton());

            // 匹配系统
            i_pSystemList.Add(MatchSystem.CreateSingleton());

            // 任务系统
            i_pSystemList.Add(TaskSystem.CreateSingleton());

            // 排行榜系统
            i_pSystemList.Add(RankSystem.CreateSingleton());

            //宝箱系统
            i_pSystemList.Add(TreasureChestSystem.CreateSingleton());
            i_pSystemList.Add(CarCultivateSystem.CreateSingleton());
            i_pSystemList.Add(MiningSystem.CreateSingleton());
            i_pSystemList.Add(ShopSystem.CreateSingleton());
            i_pSystemList.Add(LuckBoxSystem.CreateSingleton());
            i_pSystemList.Add(MapSystem.CreateSingleton());
            i_pSystemList.Add(EnergySystem.CreateSingleton());
            i_pSystemList.Add(ExtendSystem.CreateSingleton());
            i_pSystemList.Add(GiftSystem.CreateSingleton());
            i_pSystemList.Add(LuckyTurntableSystem.CreateSingleton());
            i_pSystemList.Add(ItemSystem.CreateSingleton());
            i_pSystemList.Add(SeasonSystem.CreateSingleton());
            i_pSystemList.Add(ChangeKSystem.CreateSingleton());
            i_pSystemList.Add(InviteSystem.CreateSingleton());
        }
    }
}
