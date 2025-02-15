using UnityEngine;
using UnityEditor;

public class AutoSpriteImporter : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        // 检查文件扩展名是否为我们想要处理的图片类型
        var path = assetImporter.assetPath;
        if (path.StartsWith("Assets/Art/UI") || path.StartsWith("Assets/Res/Sprites"))
        {
            if (path.EndsWith(".png") || path.EndsWith(".jpg"))
            {
                TextureImporter textureImporter = assetImporter as TextureImporter;

                // 如果texture type不是Sprite（2D and UI），则改变它
                if (textureImporter.textureType != TextureImporterType.Sprite)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                }

                // 其他Sprite导入设置...

                // 重要：应用修改
                textureImporter.SaveAndReimport();
            }
        }
    }
}
