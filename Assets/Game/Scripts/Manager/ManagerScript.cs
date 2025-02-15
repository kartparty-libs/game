using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Framework
{
    public class ManagerScript : MonoBehaviour
    {
        private static IBaseManager[] _s_pManagerTable;
        private static IBaseManager[] _s_pFixedUpdateManagerTable;
        private static IBaseManager[] _s_pUpdateManagerTable;
        private static IBaseManager[] _s_pLateUpdateManagerTable;

        void Awake()
        {
            List<IBaseManager> pManagerTable = new List<IBaseManager>();
            RegistDefine.ManagerRegist(pManagerTable);
            _s_pManagerTable = pManagerTable.ToArray();
            pManagerTable.Clear();

            for (int i = 0; i < _s_pManagerTable.Length; i++)
            {
                if (_s_pManagerTable[i].IsFixedUpdate())
                {
                    pManagerTable.Add(_s_pManagerTable[i]);
                }
            }
            _s_pFixedUpdateManagerTable = pManagerTable.ToArray();
            pManagerTable.Clear();

            for (int i = 0; i < _s_pManagerTable.Length; i++)
            {
                if (_s_pManagerTable[i].IsUpdate())
                {

                    pManagerTable.Add(_s_pManagerTable[i]);
                }
            }
            _s_pUpdateManagerTable = pManagerTable.ToArray();
            pManagerTable.Clear();

            for (int i = 0; i < _s_pManagerTable.Length; i++)
            {
                if (_s_pManagerTable[i].IsLateUpdate())
                {
                    pManagerTable.Add(_s_pManagerTable[i]);
                }
            }
            _s_pLateUpdateManagerTable = pManagerTable.ToArray();
            pManagerTable.Clear();

            for (int i = 0; i < _s_pManagerTable.Length; i++)
            {
                _s_pManagerTable[i].OnAwake();
            }
        }

        void Start()
        {
            ManagerLoad(() =>
            {
                for (int i = 0; i < _s_pManagerTable.Length; i++)
                {
                    _s_pManagerTable[i].OnStart();
                }
            });
        }

        void FixedUpdate()
        {
            for (int i = 0; i < _s_pFixedUpdateManagerTable.Length; i++)
            {
                _s_pFixedUpdateManagerTable[i].OnFixedUpdate(Time.fixedDeltaTime);
            }
        }

        void Update()
        {
            for (int i = 0; i < _s_pUpdateManagerTable.Length; i++)
            {
                _s_pUpdateManagerTable[i].OnUpdate(Time.deltaTime);
            }
        }

        void LateUpdate()
        {
            for (int i = 0; i < _s_pLateUpdateManagerTable.Length; i++)
            {
                _s_pLateUpdateManagerTable[i].OnLateUpdate(Time.deltaTime);
            }

        }

        void OnDestroy()
        {
            for (int i = 0; i < _s_pManagerTable.Length; i++)
            {
                _s_pManagerTable[i].OnDestroy();
            }

        }

        public static void ManagerLoad(Action i_fCallBack = null)
        {
            int nCount = _s_pManagerTable.Length;
            if (nCount > 0)
            {
                SyncCounter pSyncCounter = new SyncCounter();

                for (int i = 0; i < _s_pManagerTable.Length; i++)
                {
                    pSyncCounter.Add_0(_s_pManagerTable[i].OnLoad);
                }

                pSyncCounter.SetCallBack(delegate ()
                {
                    if (i_fCallBack != null) { i_fCallBack(); }
                });
                pSyncCounter.Begin();
            }
            else
            {
                if (i_fCallBack != null) { i_fCallBack(); }
            }
        }
    }
}