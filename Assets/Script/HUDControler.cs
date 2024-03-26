using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

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
        HintText,
        MoneyText,
        ReceiveMoney,
        DifText,
        DifImage,
        MonthYear

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
                if (SceneManager.GetActiveScene().name == "challenge"|| SceneManager.GetActiveScene().name == "level")
                {
                    if (GameManager.instance.playerData.challenge == PlayerData.Challenge.Level)
                        GetComponent<TextMeshProUGUI>().text = "Level " + GameManager.instance.playerData.currentLevel;
                    else if(GameManager.instance.playerData.challenge == PlayerData.Challenge.Daily)
                        GetComponent<TextMeshProUGUI>().text = "Daily Challenge";
                    else if(GameManager.instance.playerData.challenge == PlayerData.Challenge.Hard) 
                        GetComponent<TextMeshProUGUI>().text = "Hard";
                    else if(GameManager.instance.playerData.challenge == PlayerData.Challenge.Medium) 
                        GetComponent<TextMeshProUGUI>().text = "Medium";
                    else if(GameManager.instance.playerData.challenge == PlayerData.Challenge.Easy) 
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
                GetComponent<Text>().text = GameManager.instance.playerData.hint.ToString();
                if (GameManager.instance.playerData.hint == 0)
                {
                    GetComponent<Text>().text = "";
                }

                break;
            case HUDState.MoneyText:
                if (SceneManager.GetActiveScene().name == "challenge" || SceneManager.GetActiveScene().name == "level")
                {
                    GetComponent<Text>().text = GameManager.instance.playerData.money.ToString();
                }
                else
                {
                    GetComponent<Text>().text = MenuManager.instance.playerData.money.ToString();
                }
                break;
            case HUDState.ReceiveMoney:
                switch (GameManager.instance.playerData.challenge)
                {
                    case PlayerData.Challenge.Level:
                        switch (GameManager.instance.level)
                        {
                            case Difficult.Easy:
                                GetComponent<TextMeshProUGUI>().text = "+5";

                                break;
                            case Difficult.Medium:
                                GetComponent<TextMeshProUGUI>().text = "+10";

                                break;
                            case Difficult.Hard:
                                GetComponent<TextMeshProUGUI>().text = "+15";

                                break;
                        }
                        break;
                    case PlayerData.Challenge.Daily:
                                GetComponent<TextMeshProUGUI>().text = "+25";

                        break;
                    case PlayerData.Challenge.Easy:
                                GetComponent<TextMeshProUGUI>().text = "+3";

                        break;
                    case PlayerData.Challenge.Medium:
                        GetComponent<TextMeshProUGUI>().text = "+4";

                        break;
                    case PlayerData.Challenge.Hard:
                        GetComponent<TextMeshProUGUI>().text = "+7";

                        break;
                }
                break;
            case HUDState.DifText:
                switch (MenuManager.instance.selectedLevel)
                {
                    case 3:
                        GetComponent<TextMeshProUGUI>().text = "Easy";
                        break;
                    case 4:
                        GetComponent<TextMeshProUGUI>().text = "Normal";
                        break;
                    case 5:
                        GetComponent<TextMeshProUGUI>().text = "Hard";
                        break;
                }
                break;
            case HUDState.DifImage:
                switch (MenuManager.instance.selectedLevel) 
                {                     
                    case 3:
                        GetComponent<Image>().sprite = MenuManager.instance.easyImg;
                        break;
                    case 4:
                        GetComponent<Image>().sprite = MenuManager.instance.normalImg;
                        break;
                    case 5:
                        GetComponent<Image>().sprite = MenuManager.instance.hardImg;
                        break;
                }
                break;
            case HUDState.MonthYear:
                GetComponent<Text>().text = System.DateTime.Now.ToString("MMMM yyyy");
                break;
        }
    }
}
