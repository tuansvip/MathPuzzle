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
    }
}
