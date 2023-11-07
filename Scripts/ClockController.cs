using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    public float totalTime = 10.0f; // 总计时时间（秒）
    private float currentTime;       // 当前计时时间
    public Image timerCircle;       // 圆圈背景的Image组件
    public GameObject timeText;

    public GameManager gameManager;

    public RushController instance;

    private bool isTimerRunning = true; // 用于控制计时器是否运行

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        currentTime = totalTime;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // 更新计时器
            currentTime -= Time.deltaTime;
            gameManager.gameTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                instance.NextTest();
            }
            if (gameManager.gameTime < 0)
            {
                gameManager.gameTime = 0;
            }

            // 将时间格式化为24小时制时刻
            string timeString = FormatTime(gameManager.gameTime);
            timeText.GetComponent<TextMeshProUGUI>().text = timeString;

            // 更新UI元素
            float fillAmount = currentTime / totalTime;
            timerCircle.fillAmount = fillAmount;

            // 检查计时是否已经结束
            if (currentTime <= 0)
            {
                isTimerRunning = false; // 计时结束，停止计时器
                Debug.Log("End"); // 做计时结束后的操作
                // 例如，播放声音、触发事件、结束游戏等
            }
            if (gameManager.gameTime <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }
    }

    public void ResetClock()
    {
        currentTime = totalTime;
        isTimerRunning = true; // 重置计时器并启动
    }

    public void StopClock()
    {
        isTimerRunning = false; // 停止计时器
    }

    public void ResumeClock()
    {
        isTimerRunning = true; // 继续计时
    }

    // 自定义函数将秒数转换为24小时制时刻
    string FormatTime(float seconds)
    {
        int hours = (int)seconds / 3600;
        int minutes = ((int)seconds % 3600) / 60;
        int secs = (int)seconds % 60;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, secs);
    }
}
