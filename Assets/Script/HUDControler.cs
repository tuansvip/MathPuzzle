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
        Time,
        HighscoreEasy,
        HighscoreMedium,
        HighscoreHard,
        LevelText,
        NewGameTxt,
        Heart

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
            case HUDState.Heart:
                if (GameManager.instance.lives == 3)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.SetActive(true);
                } else if (GameManager.instance.lives == 2)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.SetActive(false);
                } else if (GameManager.instance.lives == 1)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(false);
                } else if (GameManager.instance.lives == 0)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(false);
                }
                break;
        }
    }
}
