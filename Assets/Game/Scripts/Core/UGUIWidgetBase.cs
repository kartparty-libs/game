using Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIWidgetBase : UIWidget
{
    protected override void OnAwake()
    {
        base.OnAwake();
        if (this.Widget != null)
        {
            var fields = this.Widget.gameObject.GetComponent<GameObjectFields>();
            if (fields != null)
            {
                try
                {
                    InjectFields(fields);
                }
                catch (System.Exception)
                {
                    Debug.LogError(this.Widget.gameObject.name + "注入变量时出错");
                }
            }
        }
    }
    protected virtual void InjectFields(GameObjectFields value)
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
}