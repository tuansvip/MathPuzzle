using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public GameObject calBody;
    public Daily[] slots;
    void Start()
    {
        FlatCalendar flatCalendar;
        flatCalendar = GameObject.Find("FlatCalendar").GetComponent<FlatCalendar>();
        flatCalendar.initFlatCalendar();
        flatCalendar.installDemoData();
        slots = calBody.GetComponentsInChildren<Daily>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Button>().IsActive() && MenuManager.instance.playerData.daily[int.Parse(slots[i].GetComponentInChildren<Text>().text)] == true)
            {
                slots[i].IsPassed = true;
                slots[i].GetComponent<Button>().interactable = false;
                slots[i].GetComponentInChildren<Text>().color = Color.gray;
            }
        }

    }


}
