using Framework;
using System;
using System.Collections;
using UnityEngine;

public class ResLoader
{
    private AssetLoader assetLoader;
    public ResLoader(AssetLoader assetLoader)
    {
        this.assetLoader = assetLoader;
    }
    public ResResult LoadAsset(string path, Action<ResResult> callback = null)
    {
        var result = assetLoader.LoadAsset(path);
        // result.IsGameObject = false;
        return new ResResult(result, callback);
    }
    public void Unload(ResResult value)
    {
        if(value==null)
        {

            return;
        }
        assetLoader.Unload(value.Result);
    }
    public void Unload()
    {
        assetLoader.Unload();
    }
    public void Update(float deltaTime, float unscaledDeltaTime)
    {
        if (assetLoader != null)
        {
            assetLoader.Update(deltaTime, unscaledDeltaTime);
        }
    }


}
public class ResResult : CustomYieldInstruction
{
    public IAssetResult Result { get; private set; }
    public ResResult(IAssetResult res, Action<ResResult> callback = null)
    {
        this.Result = res;
        res.SetCompleteCallback(_ =>
        {
            callback?.Invoke(this);
        });
    }
    public object Asset => Result.Asset;
    public override bool keepWaiting => !Result.IsDone;

    public GameObject GetInstance()
    {
        if (Result.Asset != null)
        {
            if (Result.Asset is GameObject obj)
            {
                return GameObject.Instantiate(obj);
            }
        }
        return null;
    }
    public void SetSceneAllowActivation(bool value)
    {
        this.Result.SetSceneAllowActivation(value);
    }
    public void SetSceneActive()
    {
        Result.SetSceneActive();
    }
}