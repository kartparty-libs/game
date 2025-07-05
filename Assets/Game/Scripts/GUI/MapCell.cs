
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MapCell : UIWidgetBase
{
    public int UnlockLevel { get; private set; }
    public void SetData(int mapId)
    {
        var data = GameEntry.Table.Map.Get(mapId);
        UnlockLevel = data.CarLevelLimit;
        GameEntry.Atlas.SetSprite(this.icon_Image, data.Icon, false, true);
        this.lv_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1408, UnlockLevel);
        UpdateInfo();
    }
    public void UpdateInfo()
    {
        var lv = PlayerSystem.Instance.CarLevel;
        Utils.Unity.SetActive(this.lock_GameObject, lv < UnlockLevel);
    }
}