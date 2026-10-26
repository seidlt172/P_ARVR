using UnityEngine;
using UnityEngine.SceneManagement;

// Creates the bonus features (HUD, sound, confetti, extra lights) in code every time a scene
// with PickUps is loaded, including after a restart. The existing scene objects stay untouched.
public static class BonusBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Create();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Create();
    }

    private static void Create()
    {
        if (GameObject.FindGameObjectWithTag("PickUp") == null) return;
        if (Object.FindFirstObjectByType<GameHUD>() != null) return;

        GameObject root = new GameObject("BonusFeatures");
        root.AddComponent<GameHUD>();
        root.AddComponent<PickUpSound>();
        root.AddComponent<ConfettiCelebration>();
        root.AddComponent<ExtraLights>();
    }
}
