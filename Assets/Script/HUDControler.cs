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
        DifImage

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
                if (SceneManager.GetActiveScene().name == "sample"|| SceneManager.GetActiveScene().name == "level")
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
                if (SceneManager.GetActiveScene().name == "sample")
                {
                    GetComponent<TextMeshProUGUI>().text = GameManager.instance.playerData.hint.ToString();
                }
                else
                {
                    GetComponent<Text>().text = MenuManager.instance.playerData.hint.ToString();
                }
                break;
            case HUDState.MoneyText:
                if (SceneManager.GetActiveScene().name == "sample" || SceneManager.GetActiveScene().name == "level")
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
                                GetComponent<TextMeshProUGUI>().text = "+5 Coins";

                                break;
                            case Difficult.Medium:
                                GetComponent<TextMeshProUGUI>().text = "+10 Coins";

                                break;
                            case Difficult.Hard:
                                GetComponent<TextMeshProUGUI>().text = "+15 Coins";

                                break;
                        }
                        break;
                    case PlayerData.Challenge.Daily:
                                GetComponent<TextMeshProUGUI>().text = "+25 Coins";

                        break;
                    case PlayerData.Challenge.Easy:
                                GetComponent<TextMeshProUGUI>().text = "+3 Coins";

                        break;
                    case PlayerData.Challenge.Medium:
                        GetComponent<TextMeshProUGUI>().text = "+4 Coins";

                        break;
                    case PlayerData.Challenge.Hard:
                        GetComponent<TextMeshProUGUI>().text = "+7 Coins";

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
        }
    }
}
