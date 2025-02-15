using System.IO;
using System.Collections;
using Framework;
using UnityEngine;

public class GamePreload : FsmStateAction
{
    protected override void OnEnter()
    {
        base.OnEnter();
        GameEntry.Coroutine.Start(loadTable(), "preload");
        GameEntry.Coroutine.Start(GameEntry.Table.Load(GameEntry.AssetsLoader), "loadtable");
        var s = "{\"serverId\":1,\"roleId\":10000020,\"name\":\"q11\",\"headId\":1}";
        Utils.Json.ToObject<RankOtherInfo_PlayerInfo>(s);
#if UNITY_EDITOR
        var path = Path.Combine(Application.dataPath, "Res/DataTable");
        if (!Directory.Exists(path))
        {
            Debug.LogError("先生成数据表");
        }
#endif
    }
    protected override void OnUpdate(float deltaTime, float progress)
    {
        base.OnUpdate(deltaTime, progress);
    }
    IEnumerator loadTable()
    {
        var count = GameEntry.Table.Count;
        while (GameEntry.Table.LoadCount < count)
        {
            yield return null;
        }
        this.Finished = true;
    }
}