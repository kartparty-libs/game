using System;
using System.Collections.Generic;

namespace Framework
{
    public class EventManager : BaseManager<EventManager>
    {
        // ----------------------------------------------------------------------------------------
        // 管理器逻辑

        /// <summary>
        /// 事件容器
        /// </summary>
        protected IEventContainer[] m_pEventContainer = new IEventContainer[EventDefine.Global.Count];

        /// <summary>
        /// 事件容器接口
        /// </summary>
        protected interface IEventContainer { void Clear(); }

        // ----------------------------------------------------------------------------------------
        // 无参数事件容器

        protected class EventContainer : IEventContainer
        {
            protected List<Action> m_pLazyContainer = new List<Action>();

            public void Insert(Action i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch()
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i]();
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist(int i_nEventType, Action i_fReceiver)
        {
            EventContainer pEventContainer = m_pEventContainer[i_nEventType] as EventContainer;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove(int i_nEventType, Action i_fReceiver)
        {
            EventContainer pEventContainer = m_pEventContainer[i_nEventType] as EventContainer;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch(int i_nEventType)
        {
            EventContainer pEventContainer = m_pEventContainer[i_nEventType] as EventContainer;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch();
            }
        }

        public void Clear(int i_nEventType)
        {
            IEventContainer pIEventContainer = m_pEventContainer[i_nEventType] as IEventContainer;
            if (pIEventContainer != null)
            {
                pIEventContainer.Clear();
            }
        }

        // ----------------------------------------------------------------------------------------
        // 一参数事件容器

        protected class EventContainer<CT0> : IEventContainer
        {
            protected List<Action<CT0>> m_pLazyContainer = new List<Action<CT0>>();

            public void Insert(Action<CT0> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0>(int i_nEventType, Action<FT0> i_fReceiver)
        {
            EventContainer<FT0> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0>(int i_nEventType, Action<FT0> i_fReceiver)
        {
            EventContainer<FT0> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0>(int i_nEventType, FT0 i_pVal0)
        {
            EventContainer<FT0> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 二参数事件容器

        protected class EventContainer<CT0, CT1> : IEventContainer
        {
            protected List<Action<CT0, CT1>> m_pLazyContainer = new List<Action<CT0, CT1>>();

            public void Insert(Action<CT0, CT1> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1>(int i_nEventType, Action<FT0, FT1> i_fReceiver)
        {
            EventContainer<FT0, FT1> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1>(int i_nEventType, Action<FT0, FT1> i_fReceiver)
        {
            EventContainer<FT0, FT1> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1)
        {
            EventContainer<FT0, FT1> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 三参数事件容器

        protected class EventContainer<CT0, CT1, CT2> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2>> m_pLazyContainer = new List<Action<CT0, CT1, CT2>>();

            public void Insert(Action<CT0, CT1, CT2> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2>(int i_nEventType, Action<FT0, FT1, FT2> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2>(int i_nEventType, Action<FT0, FT1, FT2> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2)
        {
            EventContainer<FT0, FT1, FT2> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 四参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3>>();

            public void Insert(Action<CT0, CT1, CT2, CT3> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3>(int i_nEventType, Action<FT0, FT1, FT2, FT3> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3>(int i_nEventType, Action<FT0, FT1, FT2, FT3> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3)
        {
            EventContainer<FT0, FT1, FT2, FT3> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 五参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 六参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4, CT5> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4, CT5>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4, CT5>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4, CT5> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4, CT5> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4, CT5 i_pVal5)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4, FT5>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4, FT5>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4, FT5>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4, FT5>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4, FT5 i_pVal5)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 七参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4, CT5, CT6> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4, CT5 i_pVal5, CT6 i_pVal6)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4, FT5, FT6>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4, FT5, FT6>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4, FT5, FT6>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4, FT5 i_pVal5, FT6 i_pVal6)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6);
            }
        }

        // ----------------------------------------------------------------------------------------
        // 八参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4, CT5 i_pVal5, CT6 i_pVal6, CT7 i_pVal7)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4, FT5 i_pVal5, FT6 i_pVal6, FT7 i_pVal7)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7);
            }
        }

        // ----------------------------------------------------------------------------------------
        //  九参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4, CT5 i_pVal5, CT6 i_pVal6, CT7 i_pVal7, CT8 i_pVal8)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7, i_pVal8);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4, FT5 i_pVal5, FT6 i_pVal6, FT7 i_pVal7, FT8 i_pVal8)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7, i_pVal8);
            }
        }

        // ----------------------------------------------------------------------------------------
        //  十参数事件容器

        protected class EventContainer<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9> : IEventContainer
        {
            protected List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9>> m_pLazyContainer = new List<Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9>>();

            public void Insert(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9> i_fEventLister)
            {
                m_pLazyContainer.Add(i_fEventLister);
            }

            public void Remove(Action<CT0, CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9> i_fEventLister)
            {
                m_pLazyContainer.Remove(i_fEventLister);
            }

            public void Dispatch(CT0 i_pVal0, CT1 i_pVal1, CT2 i_pVal2, CT3 i_pVal3, CT4 i_pVal4, CT5 i_pVal5, CT6 i_pVal6, CT7 i_pVal7, CT8 i_pVal8, CT9 i_pVal9)
            {
                if (m_pLazyContainer.Count > 0)
                {
                    for (int i = 0; i < m_pLazyContainer.Count; i++)
                    {
                        m_pLazyContainer[i](i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7, i_pVal8, i_pVal9);
                    }
                }
            }

            public void Clear()
            {
                m_pLazyContainer.Clear();
            }
        }

        public void Regist<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>;
            if (pEventContainer == null)
            {
                pEventContainer = new EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>();
                m_pEventContainer[i_nEventType] = pEventContainer;
            }
            pEventContainer.Insert(i_fReceiver);
        }

        public void Remove<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>(int i_nEventType, Action<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9> i_fReceiver)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>;
            if (pEventContainer != null)
            {
                pEventContainer.Remove(i_fReceiver);
            }
        }

        public void Dispatch<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>(int i_nEventType, FT0 i_pVal0, FT1 i_pVal1, FT2 i_pVal2, FT3 i_pVal3, FT4 i_pVal4, FT5 i_pVal5, FT6 i_pVal6, FT7 i_pVal7, FT8 i_pVal8, FT9 i_pVal9)
        {
            EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9> pEventContainer = m_pEventContainer[i_nEventType] as EventContainer<FT0, FT1, FT2, FT3, FT4, FT5, FT6, FT7, FT8, FT9>;
            if (pEventContainer != null)
            {
                pEventContainer.Dispatch(i_pVal0, i_pVal1, i_pVal2, i_pVal3, i_pVal4, i_pVal5, i_pVal6, i_pVal7, i_pVal8, i_pVal9);
            }
        }
    }
}
