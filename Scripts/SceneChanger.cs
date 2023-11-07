using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // 留坑：GameManger里保留切换function，但是返回时button挂载显示missing
    public GameManager instance;
    public void StartAsyncGame()
    {
        instance.flagMode = true;
        SceneManager.LoadScene("AsyncScene");
    }
    public void StartSyncGame()
    {
        instance.flagMode = false;
        SceneManager.LoadScene("QuizScene");
    }

}
