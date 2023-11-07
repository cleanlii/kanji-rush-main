using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    [Header("数据统计")]
    public int timeCount; // 游玩时间

    [Header("成就状态")]
    public Achievement[] deathCountAchievement;

    [Header("游戏状态")]
    [SerializeField] GameState gameState = GameState.Playing;
    public bool flagMode = false;

    [Header("积分规则")]
    public int pointPass = 5;
    public int pointCheck = -2;
    public int pointDetail = -4;
    public float gameTime = 100.0f;

    [Header("积分状态")]
    public int pointRed;
    public int pointBlue;
    public int pointTotal;

    [Header("对战AI参数")]
    public float minInterval = 0.5f; // 最小响应时间
    public float maxInterval = 2.0f; // 最大响应时间
    private float nextCallTime;
    public int PassWeight = 3; // 权重
    public int CheckWeight = 1;
    public int DetailWeight = 1;

    public event Action TimeCountAction;

    float timeSpend = 0.0f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        SetNextCallTime();
    }

    private void Start()
    {
        // FindObjectOfType<GameManager>().DeathCountAction += GetDeathCount;
    }

    void Update()
    {
        timeSpend += Time.deltaTime;
        timeCount = (int)timeSpend;
        if (instance.TimeCountAction != null)
        {
            instance.TimeCountAction();
        }
    }

    public void StartAsyncGame()
    {
        SceneManager.LoadScene("AsyncScene");
        instance.gameState = GameState.AsyncMode;
    }
    public void StartSyncGame()
    {
        SceneManager.LoadScene("QuizScene");
        instance.gameState = GameState.SyncMode;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        instance.pointRed = 0;
        instance.pointBlue = 0;
        instance.gameTime = 100.0f;
    }

    public bool IsAsyncMode()
    {
        if (flagMode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void GainPassPoint()
    {
        instance.pointRed += instance.pointPass;
    }

    public static void GainCheckPoint()
    {
        instance.pointRed += instance.pointCheck;
    }
    public static void GainDetailPoint()
    {
        instance.pointRed += instance.pointDetail;
    }

    public static void VictoryRed()
    {
        instance.ResetGame();
        SceneManager.LoadScene("StartMenu");
    }

    public static void VictoryBlue()
    {
        instance.ResetGame();
        SceneManager.LoadScene("StartMenu");
    }

    public void CombatBlue()
    {
        if (Time.time >= nextCallTime)
        {
            // 根据权重随机选择要调用的函数
            int randomFunction = UnityEngine.Random.Range(1, PassWeight + CheckWeight + DetailWeight + 1);

            if (randomFunction <= PassWeight)
            {
                instance.pointBlue += instance.pointPass;
            }
            else if (randomFunction <= PassWeight + CheckWeight)
            {
                instance.pointBlue += instance.pointCheck;
            }
            else
            {
                instance.pointBlue += instance.pointDetail;
            }

            // 更新下一次调用的时间
            SetNextCallTime();
        }
    }

    private void SetNextCallTime()
    {
        // 使用随机值来计算下一次调用的时间
        nextCallTime = Time.time + UnityEngine.Random.Range(minInterval, maxInterval);
    }
}

[System.Serializable]
public enum GameState
{
    // 枚举
    SyncMode,
    AsyncMode,
    Playing,
    Paused,
    GameOver
}

[System.Serializable]
public class Achievement
{
    public string achievementName;
    public string achievementDescription;
    public bool unlocked;
}