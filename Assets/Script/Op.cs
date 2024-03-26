using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Op : MonoBehaviour
{

    public enum Operations
    {
        plus,
        minus,
        multiply,
        divide,
        equal
    }
    public Operations op;
    public Image bg;
    public TextMeshProUGUI textValue;
    public int index = 0;
    private void Update()
    {
        switch (op)
        {
            case Operations.plus:
                textValue.text = "+";
                break;
            case Operations.minus:
                textValue.text = "-";
                break;
            case Operations.multiply:
                textValue.text = "x";
                break;
            case Operations.divide:
                textValue.text = "�";
                break;
            case Operations.equal:
                textValue.text = "=";
                break;
        }
    }
}
