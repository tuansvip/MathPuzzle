using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Heart,
        HintText

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
                break;
            case HUDState.HighscoreMedium:
                break;
            case HUDState.HighscoreHard:
                break;
            case HUDState.LevelText:
                if (SceneManager.GetActiveScene().name == "sample")
                {
                    if (GameManager.instance.playerData.chalenge == PlayerData.Chalenge.Level)
                        GetComponent<TextMeshProUGUI>().text = "Level " + GameManager.instance.playerData.currentLevel;
                    else if(GameManager.instance.playerData.chalenge == PlayerData.Chalenge.Daily)
                        GetComponent<TextMeshProUGUI>().text = "Daily Chalenge";
                    else if(GameManager.instance.playerData.chalenge == PlayerData.Chalenge.Hard) 
                        GetComponent<TextMeshProUGUI>().text = "Hard";
                    else if(GameManager.instance.playerData.chalenge == PlayerData.Chalenge.Medium) 
                        GetComponent<TextMeshProUGUI>().text = "Medium";
                    else if(GameManager.instance.playerData.chalenge == PlayerData.Chalenge.Easy) 
                        GetComponent<TextMeshProUGUI>().text = "Easy";
                }
                else
                {
                    GetComponent<Text>().text = "Level " + MenuManager.instance.playerData.currentLevel;
                }        
                break;
            case HUDState.NewGameTxt:
                break;
            case HUDState.Heart:
                break;
            case HUDState.HintText:
                if (SceneManager.GetActiveScene().name == "sample")
                {
                    GetComponent<TextMeshProUGUI>().text = GameManager.instance.playerData.hint.ToString();
                }
                else
                {
                    GetComponent<Text>().text = MenuManager.instance.playerData.hint.ToString();
                }
                break;

        }
    }
}
