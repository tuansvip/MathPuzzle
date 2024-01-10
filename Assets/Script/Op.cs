using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public TextMeshProUGUI textValue;
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
                textValue.text = "*";
                break;
            case Operations.divide:
                textValue.text = "/";
                break;
            case Operations.equal:
                textValue.text = "=";
                break;
        }
    }
}
