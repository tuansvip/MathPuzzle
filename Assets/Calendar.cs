using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public Color selectedColor, normalColor, passedColor, uninteracColor, textColor;
    public GameObject adsIcon;
    public Scrollbar progressBar;
    int dayOffset, monthDays;
    int passedCounter = 0;
    private void Start()
    {
        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        monthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        DayOfWeek firstDayOfWeek = firstDay.DayOfWeek;
        dayOffset = (int)firstDayOfWeek;

        for (int i = 1; i <= dayOffset; i++)
        {
            GameObject.Find("Slot " + i).SetActive(false);
        }
        for (int i = dayOffset + 1; i <= dayOffset + monthDays; i++)
        {
            GameObject dayBtn = GameObject.Find("Slot " + i);
            dayBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i - dayOffset).ToString();
            if (i - dayOffset == DateTime.Now.Day)
            {
                dayBtn.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (MenuManager.instance.playerData.daily[i- dayOffset])
            {
                passedCounter++;                
                dayBtn.GetComponent<Button>().interactable = false;
                dayBtn.GetComponent<Image>().color = passedColor;
                dayBtn.transform.GetChild(2).gameObject.SetActive(MenuManager.instance.playerData.daily[i - dayOffset] ? true : false);
                dayBtn.transform.GetChild(1).gameObject.SetActive(false);
                
            }
            if (i - dayOffset > DateTime.Now.Day)
            {
                dayBtn.GetComponent<Button>().interactable = false;
                dayBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = uninteracColor;
            }
        }
        for (int i = dayOffset + DateTime.Now.Day; i >= dayOffset + 1 ; i--)
        {
            if (!MenuManager.instance.playerData.daily[i - dayOffset])
            {
                SelectDay(i);
                break;
            }
        }
        for (int i = dayOffset + monthDays + 1; i <= 37; i++)
        {
            GameObject.Find("Slot " + i).SetActive(false);
        }
        if (passedCounter == 0)
        {
            progressBar.size = 0;
        }
        else if (passedCounter <3 && passedCounter >= 1)
        {
            progressBar.size = 0.2f + (passedCounter - 1) * 0.1f;
            if (!MenuManager.instance.playerData.monthGift[0])
            {
                GameObject.Find("Gift 0").transform.GetChild(3).gameObject.SetActive(true);
            }
        } else if (passedCounter >= 3 && passedCounter < 7)
        {
            progressBar.size = 0.4f + (passedCounter - 3) * 0.05f;
            if (!MenuManager.instance.playerData.monthGift[1])
            {
                GameObject.Find("Gift 1").transform.GetChild(3).gameObject.SetActive(true);
            }
        } else if (passedCounter >= 7 && passedCounter < 15)
        {
            progressBar.size = 0.6f + (passedCounter - 7) * 0.025f;
            if (!MenuManager.instance.playerData.monthGift[2])
            {
                GameObject.Find("Gift 2").transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        else if (passedCounter >= 15 && passedCounter <= 25)
        {
            progressBar.size = 0.8f + (passedCounter - 15) * 0.02f;
            if (!MenuManager.instance.playerData.monthGift[3])
            {
                GameObject.Find("Gift 3").transform.GetChild(3).gameObject.SetActive(true);
            }
        } else if (passedCounter > 25)
        {
            progressBar.size = 1;
            if (!MenuManager.instance.playerData.monthGift[4])
            {
                GameObject.Find("Gift 4").transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (MenuManager.instance.playerData.monthGift[i])
            {
                GameObject gift4 = GameObject.Find("Gift " + i);
                gift4.GetComponent<Button>().interactable = false;
                gift4.transform.GetChild(1).gameObject.SetActive(false);
                gift4.transform.GetChild(2).gameObject.SetActive(true);
            }
        }

    }

    public void SelectDay(int day)
    {
        for (int i = dayOffset + 1; i <= dayOffset + monthDays; i++)
        {
            GameObject dayBtn = GameObject.Find("Slot " + i);
            dayBtn.GetComponent<Image>().color = MenuManager.instance.playerData.daily[i - dayOffset] ? passedColor : normalColor;

            dayBtn.transform.GetComponentInChildren<TextMeshProUGUI>().color = i > dayOffset + DateTime.Now.Day ? uninteracColor : textColor ;
        }
        GameObject.Find("Slot " + day).GetComponent<Image>().color = selectedColor;
        GameObject.Find("Slot " + day).transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        MenuManager.instance.playerData.day = day - dayOffset;
        MenuManager.instance.selectedDaily = day - dayOffset;
        MenuManager.instance.SavePlayerData(MenuManager.instance.playerData);
        
        if (day - dayOffset < DateTime.Now.Day)
        {
            adsIcon.SetActive(true);
            MenuManager.instance.adsDaily = true;
        }
        else
        {
            adsIcon.SetActive(false);
            MenuManager.instance.adsDaily = false;
        }
    }
    public void ReceiveMonthlyGift(int i)
    {
        if (MenuManager.instance.playerData.monthGift[i])
        {
            return;
        }
        MenuManager.instance.playerData.daily[i] = true;
        switch (i)
        {
            case 0:
                if (passedCounter < 1) return;
                StartCoroutine(MenuManager.instance.MoneyIncrease(50));  
                StartCoroutine(MenuManager.instance.MoveMoney(5, GameObject.Find("Gift " + i).GetComponent<RectTransform>()));
                break;
            case 1:
                if (passedCounter < 3) return;
                MenuManager.instance.playerData.hint += 1;
                StartCoroutine(MenuManager.instance.MoveHint(1, GameObject.Find("Gift " + i).GetComponent<RectTransform>()));
                break;
            case 2:
                if (passedCounter < 7) return;
                MenuManager.instance.playerData.hint += 2;
                StartCoroutine(MenuManager.instance.MoveHint(2, GameObject.Find("Gift " + i).GetComponent<RectTransform>()));
                break;
            case 3:
                if (passedCounter < 15) return;
                StartCoroutine(MenuManager.instance.MoneyIncrease(200));
                StartCoroutine(MenuManager.instance.MoveMoney(20, GameObject.Find("Gift " + i).GetComponent<RectTransform>()));
                break;
            case 4:
                if (passedCounter < 25) return;
                StartCoroutine(MenuManager.instance.MoneyIncrease(500));
                StartCoroutine(MenuManager.instance.MoveMoney(50, GameObject.Find("Gift " + i).GetComponent<RectTransform>()));
                break;
                
        }
        MenuManager.instance.playerData.monthGift[i] = true;
        GameObject gift4 = GameObject.Find("Gift " + i);
        gift4.GetComponent<Button>().interactable = false;
        gift4.transform.GetChild(1).gameObject.SetActive(false);
        gift4.transform.GetChild(2).gameObject.SetActive(true);
        gift4.transform.GetChild(3).gameObject.SetActive(false);
        MenuManager.instance.SavePlayerData(MenuManager.instance.playerData);


    }
}
