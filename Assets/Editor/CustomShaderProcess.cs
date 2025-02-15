using System.Collections.Generic;
using Framework;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomShaderProcess : IPreprocessShaders
{
    public int callbackOrder { get { return 0; } }
    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        var config = ShaderConfig.GetInstance();
        if (config == null) return;
        var shaderName = shader.name;
        var len = data.Count;
        while (len-- > 0)
        {
            var keywordset = data[len].shaderKeywordSet;
            var list = keywordset.GetShaderKeywords();
            var c = list.Length;
            if (c < 1) continue;
            foreach (var item in list)
            {
                if (config.IsEnabledKeyword(shader, item))
                {
                    continue;
                }
                keywordset.Disable(item);
                // Debug.LogError("disable " + name);
                c--;
            }
            if (c < 1)
            {
                data.RemoveAt(len);
                // Debug.LogError("remove " + shaderName);
            }
        }
    }
}
