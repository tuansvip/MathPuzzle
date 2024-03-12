using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    public int value;
    public TextMeshProUGUI textValue;
    private void Update()
    {
        textValue.text = value.ToString();
        if (value < 100)
        {
            textValue.fontSize = 0.6f;
        } else if (value < 1000)
        {
            textValue.fontSize = 0.46f;
        } else
        {
            textValue.fontSize = 0.34f;
        }
    }
}
