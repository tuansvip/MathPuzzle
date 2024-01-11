using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    bool isDragging = false;
    public bool isOnBlank = false;
    public bool isOnOtherAns = false;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    Vector3 blankPosition;
    public Color bgColor;
    public Color wrongColor;
    int blankValue;
    public int blankX = 0, blankY = 0;
    public Image background;

    private void Awake()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }
    //Drag and Drop
    private void OnMouseDown()
    {
        isDragging = true;
        background.color = bgColor;
    }
    private void OnMouseUp()
    {
        isDragging = false;
        if (isOnBlank && !isOnOtherAns)
        {
            SetTarget(blankPosition);
            if (transform.GetComponent<Number>().value == blankValue)
            {
                background.color = bgColor;
            }
            else
            {
                background.color = wrongColor;
            }      
        }
        else
        {
            SetTarget(startPosition);
            background.color = bgColor;

        }
    }

    private void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    //Check if on blank cell
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlankCell")
        {
            isOnBlank = true;
            blankPosition = collision.gameObject.transform.position;
            blankValue = collision.gameObject.GetComponent<Blank>().value;
            blankX = collision.gameObject.GetComponent<Blank>().x;
            blankY = collision.gameObject.GetComponent<Blank>().y;
        }
        if (collision.gameObject.tag == "AnswerCell")
        {
            isOnOtherAns = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AnswerCell")
        {
            isOnOtherAns = false;
        }
        if (collision.gameObject.tag == "BlankCell")
        {
            isOnBlank = false;
            if (transform.GetComponent<Number>().value == collision.GetComponent<Blank>().value && !isOnOtherAns)
                GameManager.instance.check[blankX, blankY] = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.transform.tag == "BlankCell")
        {
            isOnBlank = true;
            if (transform.GetComponent<Number>().value == collision.GetComponent<Blank>().value && !isDragging)
            {
                GameManager.instance.check[blankX, blankY] = true;
            }
        }
    }


    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
    }
}

