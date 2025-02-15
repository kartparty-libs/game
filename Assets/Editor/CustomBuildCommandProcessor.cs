using System.IO;
using Framework;
using Framework.DataTable;
using UnityEditor;
using UnityEngine;

public class CustomBuildCommandProcessor : BuildCommandProcessor
{
    public override void BuildBundlePrepare(BuildCommandContext context)
    {
        base.BuildBundlePrepare(context);
        if (string.IsNullOrEmpty(context.GetStringArg("PathExportBundle")))
        {
            var path = Path.GetFullPath(Application.dataPath + "/../output_bundle");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        var builder = new DataTableBuilder();
        builder.ClearCache();
        builder.Start();
        builder.Generate();
    }
    public override void ExportProjectPrepare(BuildCommandContext context)
    {
        base.ExportProjectPrepare(context);
        Debug.LogWarning("***ExportProjectPrepare");
        if (string.IsNullOrEmpty(context.GetStringArg("PathExportProject")))
        {
            var path = Path.GetFullPath(Application.dataPath + "/../output_project");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
    public override void ExportProjectBefore(BuildCommandContext context)
    {
        base.ExportProjectBefore(context);
        Debug.LogWarning("***ExportProjectBefore");
        if (context.BuildTarget == UnityEditor.BuildTarget.StandaloneWindows)
        {
            context.SetLocationName(PlayerSettings.productName + ".exe");
        }


    }
    public override void BuildStart(BuildCommandContext context)
    {
        base.BuildStart(context);
        Debug.LogWarning("***BuildStart");

#if HybridCLR
        Debug.LogError("***HybridCLR");
        HybridCLR.Editor.Settings.HybridCLRSettings.Instance.enable = true;
        HybridCLR.Editor.Settings.HybridCLRSettings.Save();
        HybridCLR.Editor.Commands.PrebuildCommand.GenerateAll();
        Startup.copyAll();
#else
        HybridCLR.Editor.Settings.HybridCLRSettings.Instance.enable = false;
        HybridCLR.Editor.Settings.HybridCLRSettings.Save();
        var dst = Path.Combine(Application.streamingAssetsPath, "dlls");
        if (Directory.Exists(dst))
        {
            Directory.Delete(dst, true);
        }
#endif
    }

}