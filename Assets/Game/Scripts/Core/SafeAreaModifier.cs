using Framework;
using UnityEngine;

public class SafeAreaModifier : IScreenSafeAreaModifier
{
    public Rect Modify(Rect safeArea, ScreenOrientation orientation, Vector2 canvasSize)
    {
        var finalScale = 1f;
        var root = GameEntry.GUI.Config.GetRootContainer();
        if (GameEntry.Context.EnableRotation)
        {


            if (Screen.width < Screen.height)
            {
                finalScale *= (float)Screen.height / (float)Screen.width;
                root.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                root.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            root.localRotation = Quaternion.Euler(0, 0, 0);
        }
        GameEntry.GUI.ScreenInfo.CustomScale = finalScale;
        GameEntry.GUI.ScreenInfo.AdjustScreenSize();
        return safeArea;
    }
}