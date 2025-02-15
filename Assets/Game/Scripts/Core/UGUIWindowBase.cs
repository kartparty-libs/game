using Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowBase : UIWindow
{
    private float _updateSeconds;
    protected override void OnStatusChanged(UIStatus value, UIStatus previous)
    {
        base.OnStatusChanged(value, previous);
        if (UIStatus.Open == value)
        {
            OnOpen();
            OnUpdateSeconds();
        }
        else if (UIStatus.Prepare == value)
        {
            OnPrepare();
        }
        else if (UIStatus.Close == value)
        {
            OnClose();
        }
        else if (UIStatus.OpenAfter == value)
        {
            OnOpenAfter();
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        if (this.Window != null)
        {
            var fields = this.Window.gameObject.GetComponent<GameObjectFields>();
            if (fields != null)
            {
                try
                {
                    InjectFields(fields);
                }
                catch (System.Exception)
                {
                    Debug.LogError(this.Window.name + "注入变量时出错");
                }
            }
            // Debug.LogError(this.Window.name + " Awake");
        }
    }
    protected virtual void InjectFields(GameObjectFields value)
    {

    }
    protected virtual void OnPrepare()
    {

    }
    protected virtual void OnOpen()
    {

    }
    protected virtual void OnOpenAfter()
    {

    }
    protected virtual void OnClose()
    {

    }
    protected virtual void OnButtonClick(Button target)
    {
        var audio = target.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        _updateSeconds += deltaTime;
        if (_updateSeconds > 1f)
        {
            _updateSeconds -= 1f;
            OnUpdateSeconds();

        }
    }
    protected virtual void OnUpdateSeconds()
    {

    }
}