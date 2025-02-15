using Framework;
using System.IO;
using UnityEngine;

public class UGUIProcessor : UGUIProcessorBase, IUGUIEditorProcessor
{
    public UGUIProcessor()
    {
        _layers = new string[] { "基础", "浮动", "弹窗", "顶层", "提示" };
        _outPathFormat = Path.Combine(Application.dataPath, "Game/Scripts/GUI/{0}.cs");
        _fieldsOutPathFormat = Path.Combine(Application.dataPath, "Game/Scripts/Generate/GUI/{0}.Fields.cs");
        _infoPath = Path.Combine(Application.dataPath, "Game/Scripts/Generate/GUI/GUIPathInfo.cs");
    }
}