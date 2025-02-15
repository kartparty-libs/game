using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this float value)
        {
            return new Vector3(value, value, value);
        }

        // 辅助变量
        static Vector2 tempV2;

        public static Vector2 ToXZ(this Vector3 thiz)
        {
            tempV2.x = thiz.x;
            tempV2.y = thiz.z;
            return tempV2;
        }

        // 辅助变量
        static Vector3 tempV3;

        public static Vector3 ToVector3(this Vector2 thiz)
        {
            tempV3.x = thiz.x;
            tempV3.y = 0;
            tempV3.z = thiz.y;
            return tempV3;
        }

        public static Vector3 ResetY(this Vector3 thiz, float newY = 0)
        {
            thiz.y = newY;
            return thiz;
        }

        public static Vector3 ToVector3(this Vector2 thiz, float y)
        {
            tempV3.x = thiz.x;
            tempV3.y = y;
            tempV3.z = thiz.y;
            return tempV3;
        }

        public static Vector3 ToVector3XY(this Vector2 thiz)
        {
            return new Vector3(thiz.x, thiz.y, 0);
        }

        public static Vector3 ToVector3Y(this Vector2 thiz)
        {
            return new Vector3(0, thiz.y, 0);
        }
    }
}
