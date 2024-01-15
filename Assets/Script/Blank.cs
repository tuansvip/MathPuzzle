using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting;
using UnityEngine;

public class Blank : MonoBehaviour
{
    public int value;
    public Color clickedColor;
    public int x, y;
    public bool isOnAnswer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AnswerCell"))
        {
            isOnAnswer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AnswerCell"))
        {
            isOnAnswer = false;
        }
    }
}

