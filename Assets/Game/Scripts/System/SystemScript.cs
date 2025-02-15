using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Framework
{
    public class SystemScript : MonoBehaviour
    {
        private static IBaseSystem[] _s_pSystemTable;
        private static IBaseSystem[] _s_pUpdateSystemTable;

        void Awake()
        {
            List<IBaseSystem> pSystemTable = new List<IBaseSystem>();
            RegistDefine.SystemRegist(pSystemTable);
            _s_pSystemTable = pSystemTable.ToArray();
            pSystemTable.Clear();


            for (int i = 0; i < _s_pSystemTable.Length; i++)
            {
                if (_s_pSystemTable[i].IsUpdate())
                {

                    pSystemTable.Add(_s_pSystemTable[i]);
                }
            }
            _s_pUpdateSystemTable = pSystemTable.ToArray();
            pSystemTable.Clear();

            for (int i = 0; i < _s_pSystemTable.Length; i++)
            {
                _s_pSystemTable[i].OnAwake();
            }
        }

        void Start()
        {
            for (int i = 0; i < _s_pSystemTable.Length; i++)
            {
                _s_pSystemTable[i].OnStart();
            }
        }


        void Update()
        {
            for (int i = 0; i < _s_pUpdateSystemTable.Length; i++)
            {
                _s_pUpdateSystemTable[i].OnUpdate(Time.deltaTime);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < _s_pSystemTable.Length; i++)
            {
                _s_pSystemTable[i].OnDestroy();
            }

        }
    }
}