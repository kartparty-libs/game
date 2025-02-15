using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLang : MonoBehaviour
{
    private static Dictionary<int, TextLang> _dict = new Dictionary<int, TextLang>(100);
    private static int id = 1;
    public int LangId;
    public Action<string> OnLangChanged;
    private int _id;
    private TMPro.TextMeshProUGUI _text;
    private Text _text2;
    private void Awake()
    {
        if (LangId < 1) return;
        _text = GetComponent<TMPro.TextMeshProUGUI>();
        _text2 = GetComponent<Text>();
        if (_text == null && _text2 == null) return;
        _id = id++;
        _dict.Add(_id, this);
        UpdateLang();
    }
    private void OnDestroy()
    {
        _dict.Remove(_id);
    }
    private void UpdateLang()
    {
        var txt = GameEntry.Table.Lang.Get(LangId);
        if (txt == null)
        {
            SetText("$" + LangId);
            return;
        }
        SetText(txt.Text);
        OnLangChanged?.Invoke(txt.Text);
    }
    public void SetText(string value)
    {
        if (_text != null)
        {
            _text.text = value;
        }
        if (_text2 != null)
        {
            _text2.text = value;
        }
    }
    public static void UpdateAll()
    {
        foreach (var item in _dict)
        {
            item.Value.UpdateLang();
        }
    }
}