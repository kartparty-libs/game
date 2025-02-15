using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 匹配角色类
    /// </summary>
    public partial class MatchCharacter : MonoBehaviour
    {
        private long ServerInstId;
        public Transform m_Skin { get; set; }
        public Transform m_CarSkin { get; set; }
        public Transform m_CharacterSkin { get; set; }

        // -----------------------------------------------------------------------------------------------------------------------------

        public void Init(CharacterCreateData characterData)
        {
            this.ServerInstId = characterData.uid;
            this.SetPosition(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).position);
            this.SetRotation(GameEntry.Context.SceneConfig.GetOriginPoint(characterData.playerTrackIdx).rotation);
            this.LoadSkin(characterData.characterRoleCfgId, characterData.characterCarCfgId);
        }

        public virtual void Destroy()
        {
            Destroy(this);
        }

        public virtual Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public virtual void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }
        
        public virtual Quaternion GetRotation()
        {
            return this.transform.rotation;
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            this.transform.rotation = rotation;
        }

        public virtual Vector3 GetVelocity()
        {
            return Vector3.zero;
        }

        public virtual long GetServerInstId()
        {
            return ServerInstId;
        }

        public virtual void OnChangeAniTrigger(int hashId) { }
        public virtual void OnChangeAniFloat(int hashId, float value) { }
        public virtual void OnChangeAniBool(int hashId, bool value) { }
        
        // -----------------------------------------------------------------------------------------------------------------------------
    }
    public partial class MatchCharacter
    {
        public void LoadSkin(int roleId, int carId)
        {
            StartCoroutine(loadSkin(roleId, carId));

        }
        IEnumerator loadSkin(int roleId, int carId)
        {
            if (m_Skin != null)
            {
                m_Skin.gameObject.SetActive(false);
            }
            yield return null;
            var skin = new GameObject("playerSkin");
            m_Skin = skin.transform;
            m_Skin.parent = this.transform;
            m_Skin.localPosition = Vector3.zero;
            m_Skin.localRotation = Quaternion.identity;
            m_Skin.localScale = Vector3.one;

            var role = GameEntry.Table.Role.Get(roleId);
            var car = GameEntry.Table.Car.Get(carId);

            var result = GameEntry.AssetsLoader.LoadAsset(car.Asset);
            result.Result.IsGameObject = true;
            yield return result;
            if (result.Asset is GameObject go_car)
            {
                m_CarSkin = go_car.transform;
                go_car.transform.SetParent(m_Skin, false);
            }
            result = GameEntry.AssetsLoader.LoadAsset(role.Asset);
            result.Result.IsGameObject = true;
            yield return result;
            if (result.Asset is GameObject go_role)
            {
                m_CharacterSkin = go_role.transform;
                go_role.transform.SetParent(m_CarSkin.Find("car_all/car_am/car_Bone003/role"), false);
            }
        }
    }
}