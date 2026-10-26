using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Bonus tasks 7f (score), 7g (countdown timer) and 7i ("Congratulations!").
// The HUD is created in code by BonusBootstrap, so the existing scene objects stay untouched.
public class GameHUD : MonoBehaviour
{
    public static event System.Action PickUpCollected;
    public static event System.Action AllCollected;

    public float timeLimit = 60f;

    private GameObject[] pickUps;
    private Text scoreText;
    private Text timerText;
    private Text messageText;
    private Text hintText;
    private float timeLeft;
    private int lastCollected;
    private bool gameOver;

    private void Start()
    {
        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        timeLeft = timeLimit;
        BuildUI();
        UpdateTexts();
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        timeLeft -= Time.deltaTime;

        int collected = CollectedCount();
        for (int i = lastCollected; i < collected; i++)
            PickUpCollected?.Invoke();
        lastCollected = collected;

        if (collected == pickUps.Length)
        {
            EndGame("Congratulations!");
            AllCollected?.Invoke();
        }
        else if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame("Time's up!");
        }

        UpdateTexts();
    }

    private int CollectedCount()
    {
        int collected = 0;
        foreach (GameObject p in pickUps)
            if (!p.activeSelf) collected++;
        return collected;
    }

    private void UpdateTexts()
    {
        scoreText.text = "Score: " + CollectedCount() + " / " + pickUps.Length;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    private void EndGame(string message)
    {
        gameOver = true;
        messageText.text = message;
        hintText.text = "Press R to restart";

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.enabled = false;
    }

    private void BuildUI()
    {
        GameObject canvasObj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler));
        canvasObj.transform.SetParent(transform);
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        scoreText = CreateText(canvasObj.transform, "ScoreText", 48, TextAnchor.UpperLeft,
            new Vector2(0, 1), new Vector2(30, -20), new Vector2(700, 80));
        timerText = CreateText(canvasObj.transform, "TimerText", 48, TextAnchor.UpperRight,
            new Vector2(1, 1), new Vector2(-30, -20), new Vector2(700, 80));
        messageText = CreateText(canvasObj.transform, "MessageText", 120, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(1600, 200));
        hintText = CreateText(canvasObj.transform, "HintText", 40, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(0, -80), new Vector2(1600, 80));
    }

    private static Text CreateText(Transform parent, string name, int fontSize, TextAnchor alignment,
        Vector2 anchor, Vector2 position, Vector2 size)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Text text = obj.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;

        Outline outline = obj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(3, -3);
        return text;
    }
}
