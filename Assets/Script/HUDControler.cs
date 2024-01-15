using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDControler : MonoBehaviour
{
    public enum HUDState
    {
        Win,
        Highscorealert,
        Time,
        HighscoreEasy,
        HighscoreMedium,
        HighscoreHard
    }
    public HUDState state;

    private void Update()
    {
        switch (state)
        {
            case HUDState.Win:
                float highesttimer = GameManager.instance.time;
                GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)highesttimer / 60, (int)highesttimer % 60);
                break;
            case HUDState.Highscorealert:
                if (GameManager.instance.isHighscore)
                {
                    GetComponent<TextMeshProUGUI>().text = "New High Score";
                } else GetComponent<TextMeshProUGUI>().text = "";
                break;
            case HUDState.Time:
                float timer = GameManager.instance.time;
                GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
                break;
            case HUDState.HighscoreEasy:
                if (MenuManager.instance.LoadPlayerData().easy == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                } else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.LoadPlayerData().easy / 60, (int)MenuManager.instance.LoadPlayerData().easy % 60);
                }
                break;
            case HUDState.HighscoreMedium:
                if (MenuManager.instance.LoadPlayerData().medium == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.LoadPlayerData().medium / 60, (int)MenuManager.instance.LoadPlayerData().medium % 60);
                }
                break;
            case HUDState.HighscoreHard:
                if (MenuManager.instance.LoadPlayerData().hard == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.LoadPlayerData().hard / 60, (int)MenuManager.instance.LoadPlayerData().hard % 60);
                }
                break;
        }
    }
}
