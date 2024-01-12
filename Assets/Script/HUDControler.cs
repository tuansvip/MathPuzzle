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
        Time
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
                    GetComponent<TextMeshProUGUI>().text = "New Highscore";
                } else GetComponent<TextMeshProUGUI>().text = "";
                break;
            case HUDState.Time:
                float timer = GameManager.instance.time;
                GetComponent<TextMeshProUGUI>().text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
                break;
        }
    }
}
