using System;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 数学支持
    /// </summary>
    public class MathSupport : BaseSupport<MathSupport>
    {
        public const float PI = 3.14159274f;

        public static int FloorToInt(float i_nVal)
        {
            return Mathf.FloorToInt(i_nVal);
        }

        public static int CeilToInt(float i_nVal)
        {
            return Mathf.CeilToInt(i_nVal);
        }

        public static int Max(int i_nVal0, int i_nVal1)
        {
            return Mathf.Max(i_nVal0, i_nVal1);
        }

        public static float Max(float i_nVal0, float i_nVal1)
        {
            return Mathf.Max(i_nVal0, i_nVal1);
        }

        public static int Min(int i_nVal0, int i_nVal1)
        {
            return Mathf.Min(i_nVal0, i_nVal1);
        }

        public static float Min(float i_nVal0, float i_nVal1)
        {
            return Mathf.Min(i_nVal0, i_nVal1);
        }

        public static float Abs(float i_nVal)
        {
            return Mathf.Abs(i_nVal);
        }

        public static int Abs(int i_nVal)
        {
            return Mathf.Abs(i_nVal);
        }

        public static float Pow(float i_nVal, float i_nPow)
        {
            return Mathf.Pow(i_nVal, i_nPow);
        }

        public static float Sqrt(float i_nVal)
        {
            return Mathf.Sqrt(i_nVal);
        }

        public static float Sin(float i_nVal)
        {
            return Mathf.Sin(i_nVal);
        }

        public static float Asin(float i_nVal)
        {
            return Mathf.Asin(i_nVal);
        }

        public static float Cos(float i_nVal)
        {
            return Mathf.Cos(i_nVal);
        }
    }
}
