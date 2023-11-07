using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameManager instance;
    public ClockController clock;

    public Image playerRed;
    public Image playerBlue;

    public TextMeshProUGUI countRed;
    public TextMeshProUGUI countBlue;

    void Start()
    {
        instance = FindObjectOfType<GameManager>();
        playerRed.fillAmount = 0;
        playerBlue.fillAmount = 0;
    }

    void Update()
    {
        if (instance != null)
        {
            instance.CombatBlue();

            float fillAmountRed = (float)instance.pointRed / instance.pointTotal;
            float fillAmountBlue = (float)instance.pointBlue / instance.pointTotal;

            playerRed.fillAmount = fillAmountRed;
            playerBlue.fillAmount = fillAmountBlue;

            countRed.text = instance.pointRed.ToString();
            countBlue.text = instance.pointBlue.ToString();

            if (instance.IsAsyncMode())
            {
                if (instance.gameTime == 0)
                {
                    if (instance.pointRed >= instance.pointBlue)
                    {
                        GameManager.VictoryRed();
                    }
                    else if (instance.pointRed < instance.pointBlue)
                    {
                        GameManager.VictoryBlue();
                    }
                }

                if (playerRed.fillAmount == 1 && instance.gameTime > 0)
                {
                    Debug.Log("1");
                    instance.pointTotal *= 2;
                }

                if (playerBlue.fillAmount == 1 && instance.gameTime > 0)
                {
                    instance.pointTotal *= 2;
                }
            }

            if (!instance.IsAsyncMode())
            {
                if (instance.gameTime == 0)
                {
                    if (instance.pointRed >= instance.pointBlue)
                    {
                        GameManager.VictoryRed();
                    }
                    else if (instance.pointRed < instance.pointBlue)
                    {
                        GameManager.VictoryBlue();
                    }
                }

                if (playerRed.fillAmount == 1 && instance.gameTime > 0)
                {
                    GameManager.VictoryRed();
                }

                if (playerBlue.fillAmount == 1 && instance.gameTime > 0)
                {
                    GameManager.VictoryBlue();
                }
            }

        }
    }
}
