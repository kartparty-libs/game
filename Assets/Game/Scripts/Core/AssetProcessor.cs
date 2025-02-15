using System.IO;
using Framework;
using Unity.Collections;
using System.Collections.Generic;

public class AssetProcessor : AssetProcessorBase
{
    private static string encryptStr = "afie9_i3*&3";
    private static byte[] encryptData;
    public static byte[] GetData()
    {
        if (encryptData == null)
        {
            encryptData = System.Text.Encoding.UTF8.GetBytes(encryptStr);
        }
        return encryptData;
    }
    private Dictionary<string, MemoryStream> _decryptMap = new Dictionary<string, MemoryStream>();
    private Dictionary<string, byte[]> _decryptFileMap = new Dictionary<string, byte[]>();
    public override bool DontUnloadBundle(string path)
    {
        if (path.Contains("scenes/"))
        {
            // Debug.LogError("DontUnloadBundle " + path);
            //return true;
        }
        return false;
    }
    protected override byte[] GetEncryptData()
    {
        if (encryptData == null)
        {
            encryptData = System.Text.Encoding.UTF8.GetBytes(encryptStr);
        }
        return encryptData;
    }
}