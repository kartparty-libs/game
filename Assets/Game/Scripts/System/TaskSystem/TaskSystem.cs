using System.Collections.Generic;
using static EnumDefine;

namespace Framework
{
    /// <summary>
    /// 任务系统
    /// </summary>
    public class TaskSystem : BaseSystem<TaskSystem>
    {
        private List<TaskData> taskDatas = new List<TaskData>();
        private bool OkxFinished;
        private bool OkxReceiveAward;
        private List<TaskData> tempList = new List<TaskData>();
        private int DailyTaskCount;
        private int DailyTaskFinishCount;
        private bool _checkInvateUI;
        // -------------------------------------------------------------------------------------------------------------------------------
        // 通知
        public void Clear()
        {
            taskDatas.Clear();
        }
        public void UpdateTaskData(TaskData item)
        {
            taskDatas.Add(item);
        }
        public void Update()
        {
            DailyTaskCount = 0;
            DailyTaskFinishCount = 0;
            var count = 0;
            var achievementCount = 0;
            var achievement_invitefriend = 0;
            var challengeCount = 0;
            var taskCount = 0;
            OkxFinished = false;
            OkxReceiveAward = false;
            var turntableTaskCount = 0;
            foreach (var item in taskDatas)
            {
                var tpl = GameEntry.Table.Task.Get(item.taskCfgId);
                if (tpl == null)
                {
                    continue;
                }
                if (tpl.TaskType == 1)
                {
                    DailyTaskCount++;
                    if (item.taskValue >= tpl.TaskValueParam)
                    {
                        DailyTaskFinishCount++;
                    }
                }
                if (tpl.TitleType == 10)
                {
                    if (item.taskValue > 0)
                    {
                        OkxFinished = true;
                    }
                    if (item.isReceiveAward)
                    {
                        OkxReceiveAward = true;
                    }
                }
                if (item.isReceiveAward)
                {
                    continue;
                }
                if (item.taskValue >= tpl.TaskValueParam)
                {
                    if (tpl.TitleType == 1 || tpl.TitleType == 2 || tpl.TitleType == 10)
                    {
                        taskCount++;
                    }
                    else if (tpl.TitleType == 5 || tpl.TitleType == 6 || tpl.TitleType == 7)
                    {
                        achievementCount++;
                        if (tpl.TitleType == 7)
                        {
                            achievement_invitefriend++;
                        }
                    }
                    else if (tpl.TitleType == 8)
                    {
                        challengeCount++;
                    }
                    else if (tpl.TitleType >= 11 && tpl.TitleType <= 13)
                    {
                        turntableTaskCount++;
                    }

                }
            }
            GameEntry.RedDot.Set(RedDotName.TaskAll, nameof(TaskSystem), taskCount > 0 || achievementCount > 0 || challengeCount > 0);
            GameEntry.RedDot.Set(RedDotName.Task, nameof(TaskSystem), taskCount > 0);
            GameEntry.RedDot.Set(RedDotName.Achievement, nameof(TaskSystem), achievementCount > 0 && IsOkxReceiveAward());
            GameEntry.RedDot.Set(RedDotName.Achievement_InviteFriend, nameof(TaskSystem), achievement_invitefriend > 0 && IsOkxReceiveAward());
            GameEntry.RedDot.Set(RedDotName.Challenge, nameof(TaskSystem), challengeCount > 0 && IsOkxReceiveAward());
            GameEntry.RedDot.Set(RedDotName.OKXTask, nameof(TaskSystem), !OkxFinished);
            GameEntry.RedDot.Set(RedDotName.OKXTaskSuccess, nameof(TaskSystem), OkxFinished);
            GameEntry.RedDot.Set(RedDotName.TurntableTask, nameof(TaskSystem), turntableTaskCount > 0);
            GameEntry.RedDot.Set(RedDotName.IconInvateFriendInfo, nameof(TaskSystem), achievement_invitefriend > 0 && IsOkxReceiveAward());

            EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
        }
        public int GetLoginDays()
        {
            int value = 0;
            foreach (var item in taskDatas)
            {
                Task_Data pTaskCfgData = GameEntry.Table.Task.Get(item.taskCfgId);
                if ((TaskEventEnum)pTaskCfgData.TaskEvent == TaskEventEnum.eAccomplishTaskByEvent)
                {
                    if (item.taskValue > value)
                    {
                        value = (int)item.taskValue;
                    }
                }
            }
            return value;
        }
        public int GetMapStarCount(int mapId)
        {
            int count = 0;
            foreach (var item in taskDatas)
            {
                var tpl = GameEntry.Table.Task.Get(item.taskCfgId);
                if (tpl == null)
                {
                    continue;
                }
                if (tpl.TitleType != 8)
                {
                    continue;
                }
                if (tpl.TaskConditionParams.Length < 1)
                {
                    continue;
                }
                if (tpl.TaskConditionParams[0] == mapId)
                {
                    if (item.isReceiveAward || item.taskValue >= tpl.TaskValueParam)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public int GetMapTotalStarCount()
        {
            int count = 0;
            foreach (var item in taskDatas)
            {
                var tpl = GameEntry.Table.Task.Get(item.taskCfgId);
                if (tpl == null)
                {
                    continue;
                }
                if (tpl.TitleType != 8)
                {
                    continue;
                }
                if (tpl.TaskConditionParams.Length < 1)
                {
                    continue;
                }
                if (item.isReceiveAward || item.taskValue >= tpl.TaskValueParam)
                {
                    count++;
                }
            }
            return count;
        }
        // -------------------------------------------------------------------------------------------------------------------------------
        // 逻辑

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="i_nTaskCfgId"></param>
        /// <returns></returns>
        public TaskData GetTaskData(int i_nTaskCfgId)
        {
            foreach (var item in taskDatas)
            {
                if (item.taskCfgId == i_nTaskCfgId)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取任务列表，基于任务类型
        /// </summary>
        /// <param name="i_eTaskTypeEnum"></param>
        /// <returns></returns>
        public List<TaskData> GetTaskDataList(TaskTypeEnum i_eTaskTypeEnum)
        {
            List<TaskData> tTaskDataList = new List<TaskData>();
            foreach (var item in taskDatas)
            {
                var cfg = GameEntry.Table.Task.Get(item.taskCfgId);
                if (cfg.TaskType == (int)i_eTaskTypeEnum)
                {
                    tTaskDataList.Add(item);
                }
            }
            return tTaskDataList;
        }

        public List<TaskData> GetTaskDataListByTitleType(int value)
        {
            tempList.Clear();
            foreach (var item in taskDatas)
            {
                var cfg = GameEntry.Table.Task.Get(item.taskCfgId);
                if (cfg == null)
                {
                    continue;
                }
                if (cfg.TitleType == value)
                {
                    tempList.Add(item);
                }
            }
            return tempList;
        }
        public TaskData GetTaskDataByJumpId(int value)
        {
            foreach (var item in taskDatas)
            {
                var cfg = GameEntry.Table.Task.Get(item.taskCfgId);
                if (cfg == null)
                {
                    continue;
                }
                if (cfg.JumpUI == value)
                {
                    return item;
                }
            }
            return null;
        }
        public bool HasRedDot(int taskId)
        {
            return IsCompleted(taskId) && !IsReceiveAward(taskId);
        }
        /// <summary>
        /// 任务是否已完成
        /// </summary>
        /// <param name="i_nTaskCfgId"></param>
        /// <returns></returns>
        public bool IsCompleted(int i_nTaskCfgId)
        {
            Task_Data pTaskCfgData = GameEntry.Table.Task.Get(i_nTaskCfgId);
            var data = GetTaskData(i_nTaskCfgId);
            if (data != null && data.taskValue >= pTaskCfgData.TaskValueParam)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 任务是否已领取
        /// </summary>
        /// <param name="i_nTaskCfgId"></param>
        /// <returns></returns>
        public bool IsReceiveAward(int i_nTaskCfgId)
        {
            var data = GetTaskData(i_nTaskCfgId);
            if (data != null)
            {
                return data.isReceiveAward;
            }
            return false;
        }

        /// <summary>
        /// 领取任务奖励
        /// </summary>
        public void TaskReceiveAward(int i_nTaskCfgId)
        {
            if (!this.IsCompleted(i_nTaskCfgId))
            {
                return;
            }

            if (this.IsReceiveAward(i_nTaskCfgId))
            {
                return;
            }
            GameEntry.Http.Handler.TaskReceiveAward(i_nTaskCfgId);
            // GameEntry.Net.Send(CtoS.K_ReceiveTaskAwardReq, i_nTaskCfgId);
        }
        public bool IsOkxFinished()
        {
            return true;
            return OkxFinished;
        }
        public bool IsOkxReceiveAward()
        {
            return true;
            return OkxReceiveAward;
        }
        private int RequestJump7Count;
        private int RequestJump8Count;


        public void RequestJump7()
        {
            var d = GetTaskDataByJumpId(7);
            if (d == null)
            {
                return;
            }
            d.Running = true;
            if (RequestJump7Count < 1)
            {
                RequestJump7Count = 5;
                GameEntry.Timer.Start(6f, fetch7, 1);
                EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
            }
        }
        public void RequestJump8()
        {
            var d = GetTaskDataByJumpId(8);
            if (d == null)
            {
                return;
            }
            d.Running = true;
            if (RequestJump8Count < 1)
            {
                RequestJump8Count = 5;
                GameEntry.Timer.Start(6f, fetch8, 1);
                EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
            }
        }
        private void fetch7()
        {
            var d = GetTaskDataByJumpId(7);
            if (d.taskValue > 0)
            {
                d.Running = false;
                return;
            }
            d.Running = true;
            RequestJump7Count--;
            if (RequestJump7Count < 1)
            {
                d.Running = false;
                EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
                return;
            }
            GameEntry.Http.Handler.FinishFollow();
            GameEntry.Timer.Start(6f, fetch7, 1);
            EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
        }
        private void fetch8()
        {
            RequestJump8Count--;
            var d = GetTaskDataByJumpId(8);
            if (d.taskValue > 0)
            {
                d.Running = false;
                return;
            }
            if (RequestJump8Count < 1)
            {
                d.Running = false;
                EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
                return;
            }
            d.Running = true;
            GameEntry.Http.Handler.FinishFollowCEO();
            GameEntry.Timer.Start(6f, fetch8, 1);
            EventManager.Instance.Dispatch(EventDefine.Global.OnChangeTaskData);
        }
    }
}