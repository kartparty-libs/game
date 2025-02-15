using System.IO;
using Framework;
using UnityEngine;

public class BuildBundleCmd : IAssetBundleBuildProcessor
{
    public void After(BundleConfig config)
    {
        // Debug.LogError("After");
    }

    public string AssetBuildToBundle(string path)
    {
        return string.Empty;
    }

    public void Before(BundleConfig config)
    {
        // Debug.LogError("Before");
    }

    public bool Encrypt(string path)
    {
        Debug.LogError("Encrypt " + path);
        var bytes = File.ReadAllBytes(path);
        Framework.Utils.Encrypt.XorSelf(bytes, AssetProcessor.GetData());
        File.WriteAllBytes(path, bytes);
        return true;
    }

    public void Export(BundleConfig config, string path)
    {
    }
}