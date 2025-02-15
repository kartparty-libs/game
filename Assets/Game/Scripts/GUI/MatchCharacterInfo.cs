
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchCharacterInfo : UIWidgetBase
{
    public void UpdateInfo(CharacterCreateData info)
    {
        this.name_TextMeshProUGUI.text=info.playerName;
    }
}