using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    public int value;
    public TextMeshProUGUI textValue;
    public Image bg;
    public int index = 0;
    private void Update()
    {
        textValue.text = value.ToString();
        if (value < 1000)
        {
            textValue.fontSize = 0.46f;
        } else
        {
            textValue.fontSize = 0.34f;
        }
    }
}
