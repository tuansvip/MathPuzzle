using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FlatCalendar flatCalendar;
        flatCalendar = GameObject.Find("FlatCalendar").GetComponent<FlatCalendar>();
        flatCalendar.initFlatCalendar();
        flatCalendar.installDemoData();
    }
}
