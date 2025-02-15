using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 本地存储管理器
    /// </summary>
    public class StorageManager : BaseManager<StorageManager>
    {
        private LitJson.JsonData _m_pTempJsonData = new LitJson.JsonData();
        protected StringBuilder m_pStringCache = new StringBuilder(256);
        private int _m_nDelaySaveTimerInstId = -1;

        public bool HasKey(string i_sKey)
        {
            return PlayerPrefs.HasKey(i_sKey);
        }

        public void SetBool(string i_sKey, bool i_bVal)
        {
            PlayerPrefs.SetInt(i_sKey, i_bVal == true ? 1 : 0);
            this.DelaySaveData();
        }

        public bool GetBool(string i_sKey, bool i_bDefaultVal = false)
        {
            return PlayerPrefs.GetInt(i_sKey, i_bDefaultVal == true ? 1 : 0) == 1;
        }

        public void SetInt(string i_sKey, int i_nVal)
        {
            PlayerPrefs.SetInt(i_sKey, i_nVal);
            this.DelaySaveData();
        }

        public int GetInt(string i_sKey, int i_nDefaultVal = 0)
        {
            return PlayerPrefs.GetInt(i_sKey, i_nDefaultVal);
        }

        public void SetFloat(string i_sKey, float i_nVal)
        {
            PlayerPrefs.SetFloat(i_sKey, i_nVal);
            this.DelaySaveData();
        }

        public float GetFloat(string i_sKey, float i_nDefaultVal = 0f)
        {
            return PlayerPrefs.GetFloat(i_sKey, i_nDefaultVal);
        }

        public void SetString(string i_sKey, string i_sVal)
        {
            PlayerPrefs.SetString(i_sKey, i_sVal);
            this.DelaySaveData();
        }

        public string GetString(string i_sKey, string i_sDefaultVal = "")
        {
            return PlayerPrefs.GetString(i_sKey, i_sDefaultVal);
        }

        public void SetArray<FT0>(string i_sKey, FT0 i_pVal)
        {
            SetString(i_sKey, LitJson.JsonMapper.ToJson(i_pVal));
            this.DelaySaveData();
        }

        public FT0 GetArray<FT0>(string i_sKey, FT0 i_pDefaultVal)
        {
            var sVal = PlayerPrefs.GetString(i_sKey);
            if (sVal == null)
            {
                return i_pDefaultVal;
            }
            else
            {
                return LitJson.JsonMapper.ToObject<FT0>(sVal);
            }
        }

        public void SetMap<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pVal)
        {
            foreach (var pItem in i_pVal)
            {
                _m_pTempJsonData.Add(pItem.Key);
                _m_pTempJsonData.Add(pItem.Value);
            }
            SetString(i_sKey, _m_pTempJsonData.ToJson());
            _m_pTempJsonData.Clear();
            this.DelaySaveData();
        }

        public Dictionary<FT0, FT1> GetMap<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pDefaultVal)
        {
            var sVal = PlayerPrefs.GetString(i_sKey);
            if (sVal == null)
            {
                return i_pDefaultVal;
            }
            else
            {
                var pMapJson = LitJson.JsonMapper.ToObject<LitJson.JsonData>(sVal);
                var pMap = new Dictionary<FT0, FT1>();
                if (pMapJson != null)
                    for (int i = 0; i < pMapJson.Count; i += 2)
                    {
                        var pKeyJson = pMapJson[i].ToJson();
                        var pValueJson = pMapJson[i + 1].ToJson();
                        FT0 pKey = (FT0)Convert.ChangeType(pKeyJson, typeof(FT0));
                        FT1 pValue = (FT1)Convert.ChangeType(pValueJson, typeof(FT1));
                        pMap.Add(pKey, pValue);
                    }
                return pMap;
            }
        }

        public void SaveData()
        {
            PlayerPrefs.Save();
        }

        public void DelaySaveData()
        {
            if (this._m_nDelaySaveTimerInstId == -1)
            {
                this._m_nDelaySaveTimerInstId = GameEntry.Timer.Start(0.001f, delegate ()
                {
                    this._m_nDelaySaveTimerInstId = -1;
                    this.SaveData();
                }, 0);
            }
        }

        public void Delete(string i_sKey)
        {
            PlayerPrefs.DeleteKey(i_sKey);
            this.SaveData();
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            this.SaveData();
        }

        /// <summary>
        /// 获取玩家存储键
        /// </summary>
        /// <param name="i_sStorageKey"></param>
        /// <returns></returns>
        public string GetPlayerStorageKey(string i_sStorageKey, int i_nCfgId = -1)
        {
            m_pStringCache.Clear();
            m_pStringCache.Append(PlayerSystem.Instance.GetName());
            m_pStringCache.Append("_");
            m_pStringCache.Append(PlayerSystem.Instance.GetCreateTime());
            m_pStringCache.Append("_");
            m_pStringCache.Append(i_sStorageKey);
            if (i_nCfgId > -1)
            {
                m_pStringCache.Append("_");
                m_pStringCache.Append(i_nCfgId);
            }
            return m_pStringCache.ToString();
        }

        public bool HasKeyByPlayer(string i_sKey)
        {
            return this.HasKey(this.GetPlayerStorageKey(i_sKey));
        }

        public void SetBoolByPlayer(string i_sKey, bool i_bVal)
        {
            this.SetInt(this.GetPlayerStorageKey(i_sKey), i_bVal == true ? 1 : 0);
        }

        public bool GetBoolByPlayer(string i_sKey, bool i_bDefaultVal = false)
        {
            return this.GetInt(this.GetPlayerStorageKey(i_sKey), i_bDefaultVal == true ? 1 : 0) == 1;
        }

        public void SetIntByPlayer(string i_sKey, int i_nVal)
        {
            this.SetInt(this.GetPlayerStorageKey(i_sKey), i_nVal);
        }

        public int GetIntByPlayer(string i_sKey, int i_nDefaultVal = 0)
        {
            return this.GetInt(this.GetPlayerStorageKey(i_sKey), i_nDefaultVal);
        }

        public void SetFloatByPlayer(string i_sKey, float i_nVal)
        {
            this.SetFloat(this.GetPlayerStorageKey(i_sKey), i_nVal);
        }

        public float GetFloatByPlayer(string i_sKey, float i_nDefaultVal = 0f)
        {
            return this.GetFloat(this.GetPlayerStorageKey(i_sKey), i_nDefaultVal);
        }

        public void SetStringByPlayer(string i_sKey, string i_sVal)
        {
            this.SetString(this.GetPlayerStorageKey(i_sKey), i_sVal);
        }

        public string GetStringByPlayer(string i_sKey, string i_sDefaultVal = "")
        {
            return this.GetString(this.GetPlayerStorageKey(i_sKey), i_sDefaultVal);
        }

        public void SetArrayByPlayer<FT0>(string i_sKey, FT0 i_pVal)
        {
            this.SetArray<FT0>(this.GetPlayerStorageKey(i_sKey), i_pVal);
        }

        public FT0 GetArrayByPlayer<FT0>(string i_sKey, FT0 i_pDefaultVal)
        {
            return this.GetArray<FT0>(this.GetPlayerStorageKey(i_sKey), i_pDefaultVal);
        }

        public void SetMapByPlayer<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pVal)
        {
            this.SetMap<FT0, FT1>(this.GetPlayerStorageKey(i_sKey), i_pVal);
        }

        public Dictionary<FT0, FT1> GetMapByPlayer<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pDefaultVal)
        {
            return this.GetMap<FT0, FT1>(this.GetPlayerStorageKey(i_sKey), i_pDefaultVal);
        }

        public void DeleteByPlayer(string i_sKey)
        {
            this.Delete(this.GetPlayerStorageKey(i_sKey));
        }

        //-----------------------------------------------------------------------------------------------------------------------
        // 玩家战斗数据附加功能cfgId存储

        private HashSet<string> tPlayerCombatDataKeys = new HashSet<string>();

        public bool HasKeyByPlayerCombatData(string i_sKey, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.HasKey(sKey);
        }

        public void SetBoolByPlayerCombatData(string i_sKey, bool i_bVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetInt(sKey, i_bVal == true ? 1 : 0);
        }

        public bool GetBoolByPlayerCombatData(string i_sKey, bool i_bDefaultVal = false, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetInt(sKey, i_bDefaultVal == true ? 1 : 0) == 1;
        }

        public void SetIntByPlayerCombatData(string i_sKey, int i_nVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetInt(sKey, i_nVal);
        }

        public int GetIntByPlayerCombatData(string i_sKey, int i_nDefaultVal = 0, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetInt(sKey, i_nDefaultVal);
        }

        public void SetFloatByPlayerCombatData(string i_sKey, float i_nVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetFloat(sKey, i_nVal);
        }

        public float GetFloatByPlayerCombatData(string i_sKey, float i_nDefaultVal = 0f, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetFloat(sKey, i_nDefaultVal);
        }

        public void SetStringByPlayerCombatData(string i_sKey, string i_sVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetString(sKey, i_sVal);
        }

        public string GetStringByPlayerCombatData(string i_sKey, string i_sDefaultVal = "", int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetString(sKey, i_sDefaultVal);
        }

        public void SetArrayByPlayerCombatData<FT0>(string i_sKey, FT0 i_pVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetArray<FT0>(sKey, i_pVal);
        }

        public FT0 GetArrayByPlayerCombatData<FT0>(string i_sKey, FT0 i_pDefaultVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetArray<FT0>(sKey, i_pDefaultVal);
        }

        public void SetMapByPlayerCombatData<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            this.SetMap<FT0, FT1>(sKey, i_pVal);
        }

        public Dictionary<FT0, FT1> GetMapByPlayerCombatData<FT0, FT1>(string i_sKey, Dictionary<FT0, FT1> i_pDefaultVal, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Add(sKey);
            return this.GetMap<FT0, FT1>(sKey, i_pDefaultVal);
        }

        public void DeleteByPlayerCombatData(string i_sKey, int i_nCfgId = -1)
        {
            string sKey = this.GetPlayerStorageKey(i_sKey, i_nCfgId);
            this.tPlayerCombatDataKeys.Remove(sKey);
            this.Delete(sKey);
        }

        //-----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 清理玩家战斗重连数据
        /// </summary>
        public void ClearPlayerCombatReconnectData()
        {
            if (this.tPlayerCombatDataKeys.Count > 0)
            {
                foreach (var item in this.tPlayerCombatDataKeys)
                {
                    StorageManager.Instance.Delete(item);
                }
                this.tPlayerCombatDataKeys.Clear();
            }
        }
    }
}
