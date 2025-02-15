using System;
using UTime = UnityEngine.Time;

namespace Framework
{
    /// <summary>
    /// 时间支持
    /// </summary>
    public class TimeSupport : BaseSupport<TimeSupport>
    {
        public static float RealtimeSinceStartup
        {
            get
            {
                return UTime.realtimeSinceStartup;
            }
        }

        public static float DeltaTime
        {
            get
            {
                return UTime.deltaTime;
            }
        }

        public static float Time
        {
            get
            {
                return UTime.time;
            }
        }

        public static int RenderedFrameCount
        {
            get
            {
                return UTime.renderedFrameCount;
            }
        }

        public static int FrameCount
        {
            get
            {
                return UTime.frameCount;
            }
        }

        public static float TimeScale
        {
            get
            {
                return UTime.timeScale;
            }
            set { UTime.timeScale = value; }
        }

        public static float MaximumParticleDeltaTime
        {
            get
            {
                return UTime.maximumParticleDeltaTime;
            }
            set { UTime.maximumParticleDeltaTime = value; }
        }

        public static float SmoothDeltaTime
        {
            get
            {
                return UTime.smoothDeltaTime;
            }
        }

        public static float MaximumDeltaTime
        {
            get
            {
                return UTime.maximumDeltaTime;
            }
            set { UTime.maximumDeltaTime = value; }
        }

        public static int CaptureFramerate
        {
            get
            {
                return UTime.captureFramerate;
            }
            set { UTime.captureFramerate = value; }
        }

        public static float FixedDeltaTime
        {
            get
            {
                return UTime.fixedDeltaTime;
            }
            set { UTime.fixedDeltaTime = value; }
        }

        public static float UnscaledDeltaTime
        {
            get
            {
                return UTime.unscaledDeltaTime;
            }
        }

        public static float FixedUnscaledTime
        {
            get
            {
                return UTime.fixedUnscaledTime;
            }
        }

        public static float UnscaledTime
        {
            get
            {
                return UTime.unscaledTime;
            }
        }

        public static float FixedTime
        {
            get
            {
                return UTime.fixedTime;
            }
        }

        public static float TimeSinceLevelLoad
        {
            get
            {
                return UTime.timeSinceLevelLoad;
            }
        }

        public static float FixedUnscaledDeltaTime
        {
            get
            {
                return UTime.fixedUnscaledDeltaTime;
            }
        }

        public static bool InFixedTimeStep
        {
            get
            {
                return UTime.inFixedTimeStep;
            }
        }

        public static DateTime ServerTime
        {
            get
            {
                return DateTime.Now.AddSeconds(_s_nDiffTime);
            }
        }

        public static int ServerSeconds { get { TimeSpan span = ServerTime - DateTimeOrigin.ToLocalTime(); return (int)span.TotalSeconds; } }

        private static readonly DateTime _s_pDateTimeOrigin = new DateTime(1970, 1, 1);
        public static DateTime DateTimeOrigin
        {
            get
            {
                return _s_pDateTimeOrigin;
            }
        }

        private static readonly DateTime _s_pMaxDateTime = DateTime.MaxValue;
        public static DateTime MaxDateTime
        {
            get
            {
                return _s_pMaxDateTime;
            }
        }

        public static void SetServerTime(double i_nServerTime, double i_nMilliTime)
        {
            _s_nServerTime = i_nServerTime;
            DateTime pServerDateTime = DateTimeOrigin.AddSeconds(i_nServerTime).ToLocalTime();
            _s_nDiffTime = (double)((pServerDateTime - DateTime.Now).TotalSeconds);
        }

        //上线时服务器同步的时间戳
        private static double _s_nServerTime = 0;

        //服务器和客户端的误差时间
        private static double _s_nDiffTime = 0;

        /// <summary>
        /// 一天秒数
        /// </summary>
        public const int OneDayTotalSecond = 86400;

        /// <summary>
        /// 本地时间戳
        /// </summary>
        /// <value></value>
        public static int localTimeStamp
        {
            get
            {
                return (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// 本地时间戳_今日开始时间
        /// </summary>
        /// <value></value>
        public static int localTimeStamp_StartingTimeToday
        {
            get
            {
                DateTime pCurrTime = DateTime.Now;
                return (int)new DateTimeOffset(new DateTime(pCurrTime.Year, pCurrTime.Month, pCurrTime.Day, 0, 0, 0)).ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="i_nYear"></param>
        /// <param name="i_nMonth"></param>
        /// <param name="i_nDay"></param>
        /// <param name="i_nHour"></param>
        /// <param name="i_nMinute"></param>
        /// <param name="i_nSecond"></param>
        /// <returns></returns>
        public static int GetTimeStamp(int i_nYear, int i_nMonth, int i_nDay, int i_nHour, int i_nMinute, int i_nSecond)
        {
            return (int)new DateTimeOffset(new DateTime(i_nYear, i_nMonth, i_nDay, i_nHour, i_nMinute, i_nSecond)).ToUnixTimeSeconds();
        }
    }
}
