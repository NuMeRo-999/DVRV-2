using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LuaPointsBridge : MonoBehaviour
{
    public PointsManager pointsManager;
    public SceneManagerDialog sceneManager;

    void OnEnable()
    {
        if (pointsManager == null)
        {
            pointsManager = FindAnyObjectByType<PointsManager>();
        }

        if (sceneManager == null)
        {
            sceneManager = FindAnyObjectByType<SceneManagerDialog>();
        }


        Lua.RegisterFunction("AddPoints", pointsManager, typeof(PointsManager).GetMethod("AddPoints"));
        Lua.RegisterFunction("FollowPlayer", pointsManager, typeof(PointsManager).GetMethod("FollowPlayer"));
        Lua.RegisterFunction("Heal", pointsManager, typeof(PointsManager).GetMethod("Heal"));
        Lua.RegisterFunction("Exit", sceneManager, typeof(SceneManagerDialog).GetMethod("Exit"));
        Lua.RegisterFunction("LoadNextScene", sceneManager, typeof(SceneManagerDialog).GetMethod("LoadNextScene"));
        Lua.RegisterFunction("ActivateBoss", pointsManager, typeof(PointsManager).GetMethod("ActivateBoss"));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("AddPoints");
        Lua.UnregisterFunction("FollowPlayer");
        Lua.UnregisterFunction("Heal");
        Lua.UnregisterFunction("Exit");
        Lua.UnregisterFunction("LoadNextScene");
        Lua.UnregisterFunction("ActivateBoss");
    }
}
