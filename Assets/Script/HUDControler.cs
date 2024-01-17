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
        HighscoreHard,
        LevelText,
        NewGameTxt
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
                if (MenuManager.instance.playerData.easy == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                } else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.playerData.easy / 60, (int)MenuManager.instance.playerData.easy % 60);
                }
                break;
            case HUDState.HighscoreMedium:
                if (MenuManager.instance.playerData.medium == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.playerData.medium / 60, (int)MenuManager.instance.playerData.medium % 60);
                }
                break;
            case HUDState.HighscoreHard:
                if (MenuManager.instance.playerData.hard == 0)
                {
                    GetComponent<TextMeshProUGUI>().text = "No Highscore";
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)MenuManager.instance.playerData.hard / 60, (int)MenuManager.instance.playerData.hard % 60);
                }
                break;
            case HUDState.LevelText:
                GetComponent<TextMeshProUGUI>().text = "Level " + GameManager.instance.playerData.currentLevel;
                break;
            case HUDState.NewGameTxt:
                if (MenuManager.instance.playerData.currentLevel == 1 && MenuManager.instance.playerData.unlockLevel == 1 && MenuManager.instance.playerData.easy == 0 && MenuManager.instance.playerData.medium == 0 && MenuManager.instance.playerData.hard == 0)
                {
                    GetComponent<TextMeshProUGUI>().text =    "NEW GAME";
                } else GetComponent<TextMeshProUGUI>().text = "CONTINUE";
                break;
        }
    }
}
