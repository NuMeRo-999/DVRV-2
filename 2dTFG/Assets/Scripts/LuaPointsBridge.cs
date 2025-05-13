using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LuaPointsBridge : MonoBehaviour
{
    public PointsManager pointsManager;

    void OnEnable()
    {
        if (pointsManager == null)
        {
            pointsManager = FindAnyObjectByType<PointsManager>();
        }

        Lua.RegisterFunction("AddPoints", pointsManager, typeof(PointsManager).GetMethod("AddPoints"));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("AddPoints");
    }
}
