using System;

namespace Framework
{
    /// <summary>
    /// 随机支持
    /// </summary>
    public class RandomSupport : BaseSupport<RandomSupport>
    {
        // ----------------------------------------------------------------------------------------
        // 支持基类 - 覆写

        public override void OnAwake()
        {
            base.OnAwake();

            __s_pRandom = new Random();
        }

        // ----------------------------------------------------------------------------------------
        // 随机支持

        /// <summary>
        /// 随机实例
        /// </summary>
        protected static Random __s_pRandom;

        /// <summary>
        /// 设置种子
        /// </summary>
        /// <param name="i_nSeed"></param>
        public static void SetSeed(int i_nSeed)
        {
            __s_pRandom = new Random(i_nSeed);
        }

        /// <summary>
        /// 返回一个随机单精度浮点数[0f,1f)
        /// </summary>
        /// <returns></returns>
        public static float NextFloat()
        {
            return (float)__s_pRandom.NextDouble();
        }

        /// <summary>
        /// 返回一个随机双精度浮点数[0f,1f)
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return __s_pRandom.NextDouble();
        }

        /// <summary>
        /// 返回一个随机非负整数
        /// </summary>
        /// <returns></returns>
        public static int NextInt()
        {
            return __s_pRandom.Next();
        }

        /// <summary>
        /// 返回一个小于指定最大值的随机非负整数
        /// </summary>
        /// <param name="i_nMaxValue"></param>
        /// <returns></returns>
        public static int NextInt(int i_nMaxValue)
        {
            return __s_pRandom.Next(i_nMaxValue + 1);
        }

        /// <summary>
        /// 返回一个指定范围的随机整数
        /// </summary>
        /// <param name="i_nMinValue"></param>
        /// <param name="i_nMaxValue"></param>
        /// <returns></returns>
        public static int NextInt(int i_nMinValue, int i_nMaxValue)
        {
            return __s_pRandom.Next(i_nMinValue, i_nMaxValue + 1);
        }
    }
}
