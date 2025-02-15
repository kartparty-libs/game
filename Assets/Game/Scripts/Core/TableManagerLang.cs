using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableManager
{
    private string _currentLang;
    private CoroutineID _loadLangId;
    //branch name
    public void LoadLang(string lang)
    {
        if (_currentLang == lang)
        {
            return;
        }
        if (_loadLangId != null && !_loadLangId.Finished)
        {
            return;
        }
        GameEntry.Table.Lang.SetBranch(lang);
        _currentLang = lang;
        _loadLangId = GameEntry.Coroutine.Start(DoLoadLang());
    }
    private IEnumerator DoLoadLang()
    {
        yield return LoadItem(GameEntry.AssetsLoader, _configs[GameEntry.Table.Lang.GetTableName()]);
        TextLang.UpdateAll();
        _loadLangId = null;
    }
}
