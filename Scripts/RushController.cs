using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

// 控制题目生成、跳转到下一题、跳出解释界面

public class RushController : MonoBehaviour
{

    [Header("题目数据")]
    public List<KanjiData> test;
    public int testNum;
    public TextAsset csvFile;

    [Header("GO引用")]
    public GameObject title;
    public GameObject des;
    public GameObject detail;

    // 随机化生成题目
    private List<KanjiData> remainingQuestions = new List<KanjiData>();
    private System.Random random = new System.Random();
    private KanjiData currentQuestion;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private bool isDesOpen = false;
    private bool isDetailOpen = false;

    public ClockController clock;

    void Start()
    {
        if (test != null)
        {
            test = LoadFromCsv(csvFile.text);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // 用GetTouch(0)获取首次屏幕触摸的信息
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > Mathf.Abs(endTouchPosition.y - startTouchPosition.y) && endTouchPosition.x < startTouchPosition.x && isDesOpen == false && isDetailOpen == false)
            {
                // 熟悉：左滑，下一个，重新计时，加分
                NextTest();
                GameManager.GainPassPoint();
                // Debug.Log("手指向左滑");
            }
            // if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > Mathf.Abs(endTouchPosition.y - startTouchPosition.y) && endTouchPosition.x > startTouchPosition.x)
            // {
            //     // 熟悉：左滑，下一个，重新计时，加分
            //     if (testNum > 0)
            //     {
            //         testNum--;
            //         NextTest();
            //     }
            //     else
            //     {
            //         testNum = test.Count - 1;
            //         NextTest();
            //     }
            //     // Debug.Log("手指向右滑");
            // }
            if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) < Mathf.Abs(endTouchPosition.y - startTouchPosition.y) && endTouchPosition.y < startTouchPosition.y)
            {
                // Debug.Log("手指下滑");
                // 不认识：上滑，查看详细描述（含例句），下滑返回，计时不停止，扣分
                if (isDesOpen == false && isDetailOpen == true)
                {
                    CloseDetail();
                    isDetailOpen = false;
                    clock.ResumeClock();
                }
            }
            if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) < Mathf.Abs(endTouchPosition.y - startTouchPosition.y) && endTouchPosition.y > startTouchPosition.y)
            {
                // Debug.Log("手指上滑");
                if (isDesOpen == false && isDetailOpen == false)
                {
                    // 查看详细描述
                    OpenDetail();
                    isDetailOpen = true;
                    clock.StopClock();
                    GameManager.GainDetailPoint();
                }
            }
            if (endTouchPosition == startTouchPosition)
            {
                // Debug.Log("手指轻点");
                // 不太熟悉：轻点查看简洁描述，再次轻点返回，计时不停止，不计分
                if (isDesOpen == true && isDetailOpen == false)
                {
                    CloseDes();
                    isDesOpen = false;
                }
                else if (isDesOpen == false && isDetailOpen == false)
                {
                    // 查看简洁描述
                    OpenDes();
                    isDesOpen = true;
                    GameManager.GainCheckPoint();
                }

            }
        }
    }

    public void NextTest()
    {
        if (testNum < test.Count - 1)
        {
            GetRandomKanji();
            title.GetComponent<TextMeshProUGUI>().text = test[testNum].titleKanji;
            des.GetComponent<TextMeshProUGUI>().text = test[testNum].desKanji;
            detail.GetComponent<TextMeshProUGUI>().text = test[testNum].detailKanji;
            clock.ResetClock();
        }
        else
        {
            testNum = 0;
            title.GetComponent<TextMeshProUGUI>().text = test[testNum].titleKanji;
            des.GetComponent<TextMeshProUGUI>().text = test[testNum].desKanji;
            detail.GetComponent<TextMeshProUGUI>().text = test[testNum].detailKanji;
            clock.ResetClock();
        }
    }

    private void OpenDes()
    {
        des.SetActive(true);
        title.SetActive(false);
        detail.SetActive(false);
    }

    private void CloseDes()
    {
        des.SetActive(false);
        title.SetActive(true);
        detail.SetActive(false);
    }

    private void OpenDetail()
    {
        des.SetActive(false);
        title.SetActive(false);
        detail.SetActive(true);
    }

    private void CloseDetail()
    {
        des.SetActive(false);
        title.SetActive(true);
        detail.SetActive(false);
    }

    public void GetRandomKanji()
    {
        testNum = random.Next(0, test.Count);
    }

    // 生成题目
    public List<KanjiData> LoadFromCsv(string csvText)
    {
        List<KanjiData> result = new List<KanjiData>();

        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Skip the header line
        {
            string[] fields = lines[i].Split('\t');

            if (fields.Length >= 3)
            {
                KanjiData item = new KanjiData
                {
                    titleKanji = fields[1],
                    desKanji = fields[2],
                    detailKanji = fields[3].Replace("\"", "").Replace(",", "")
                };
                result.Add(item);
            }
        }

        return result;
    }
}
