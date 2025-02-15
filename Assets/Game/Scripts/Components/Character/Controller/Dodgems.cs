using System.Collections.Generic;
using Framework;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static EnumDefine;

public class Dodgems : MonoBehaviour
{
    public ICharacter Owner { get; private set; }
    private Dictionary<int, DodgemsData> _overlayTime = new Dictionary<int, DodgemsData>();
    private void Start()
    {
        var colliders = GetComponents<Collider>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }
        }
        colliders = GetComponentsInChildren<Collider>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }
        }
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
        Owner = this.gameObject.GetComponentInParent<ICharacter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Owner == null)
        {
            return;
        }
        var character = other.GetComponent<Dodgems>();
        if (character == null || character.Owner == null)
        {
            return;
        }
        if (Owner is MainCharacter mainCharacter)
        {
            var buff = new VelocityBuff();
            buff.From = character.Owner.GetCharacterInfo().RoleId + "";
            buff.Id = other.GetInstanceID();
            buff.Duration = 0.6f;
            buff.VelocityValue = 15f;
            buff.VelocityDirection = (transform.position - other.transform.position).normalized;
            mainCharacter.AddVelocityBuff(buff);
            mainCharacter.m_Effectctrl.Peng();
        }
        else if (Owner is NetWorkCharacter netWorkCharacter)
        {
            var buff = new VelocityBuff();
            buff.From = character.Owner.GetCharacterInfo().RoleId+ "";
            buff.Id = other.GetInstanceID();
            buff.Duration = 0.6f;
            buff.VelocityValue = 15f;
            buff.VelocityDirection = (transform.position - other.transform.position).normalized;
            if (GameEntry.Context.Gameplay.TableData.Id < 9)
            {
                netWorkCharacter.AddVelocityBuff(buff);
            }
            var id = netWorkCharacter.GetCharacterInfo().RoleId;
            JArray args = new JArray();
            buff.Encode(args);
            GameEntry.Net.Send(CtoS.K_PlayerAddBuffReq, id, BuffType.VelocityBuff, args);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Owner == null)
        {
            return;
        }
        var character = other.GetComponent<Dodgems>();
        if (character == null || character.Owner == null)
        {
            return;
        }
        /*
        if (Owner is MainCharacter mainCharacter)
        {
            var id = other.GetInstanceID();

            foreach (var item in _overlayTime)
            {
                if (item.Key == id)
                {
                    item.Value.StayTime += Time.deltaTime;
                    if (item.Value.StayTime > 0.2f)
                    {
                        var buff = new VelocityBuff();
                        buff.Id = other.GetInstanceID();
                        buff.Time = 0.6f;
                        buff.Timer = 0.6f;
                        buff.VelocityValue = 15f;
                        buff.VelocityDirection = (transform.position - other.transform.position).normalized;
                        mainCharacter.ApplyHit(buff);
                        item.Value.StayTime = 0f;
                    }
                }
            }
            if (!_overlayTime.ContainsKey(id))
            {
                _overlayTime.Add(id, new DodgemsData());
            }
        }
        */
    }
    private void OnTriggerExit(Collider other)
    {
        var id = other.GetInstanceID();
        if (_overlayTime.ContainsKey(id))
        {
            _overlayTime.Remove(id);
        }
    }
}
internal class DodgemsData
{
    public float StayTime;

}