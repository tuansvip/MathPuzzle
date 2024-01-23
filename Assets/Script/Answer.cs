using DG.Tweening;
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
    Collider2D anotherCol;
    Transform blank;

    private void Awake()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }
    //Drag and Drop
    private void OnMouseDown()
    {
        if (GameManager.instance.selectedBlank != null)
        {
            SFXManager.instance.PlayPaper();
            SetTarget(GameManager.instance.selectedBlank.transform.position);
            transform.position = GameManager.instance.selectedBlank.transform.position;
            transform.DOScale(GameManager.instance.selectedBlank.transform.localScale, 0.5f).SetEase(Ease.OutQuart);
            if (transform.GetComponent<Number>().value == GameManager.instance.selectedBlank.value)
            {
                background.color = bgColor;
            }
            else
            {
                background.color = wrongColor;
            }
            GameManager.instance.selectedBlank.GetComponent<Blank>().bg.color = Color.white;
            GameManager.instance.selectedBlank.GetComponent<Blank>().isSelected = false;
            GameManager.instance.selectedBlank = null;
        }
        else
        {
            isDragging = true;
            background.color = bgColor;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);

        }

    }
    private void OnMouseUp()
    {
        transform.position = transform.position + Vector3.back * -5;
        if (!isOnOtherAns && isOnBlank)
        {
            if (isDragging) SFXManager.instance.PlayPaper();
            SetTarget(blankPosition);
            transform.DOScale(blank.localScale, 0.5f).SetEase(Ease.OutQuart);
            if (transform.GetComponent<Number>().value == blankValue)
            {
                background.color = bgColor;
            }
            else
            {
                GameManager.instance.lives--;
                background.color = wrongColor;
            }      
        }
        else if (isOnOtherAns && isOnBlank)
        {
            if (isDragging) SFXManager.instance.PlayPaper();
            transform.DOScale(blank.localScale, 0.5f).SetEase(Ease.OutQuart);
            if (transform.GetComponent<Number>().value == blankValue)
            {
                background.color = bgColor;
            }
            else
            {
                GameManager.instance.lives--;
                background.color = wrongColor;
            }
            SetTarget(blankPosition);
            anotherCol.GetComponent<Answer>().transform.localScale = Vector3.one;
            anotherCol.GetComponent<Answer>().SetTarget(anotherCol.GetComponent<Answer>().startPosition);
            anotherCol.GetComponent<Answer>().background.color = anotherCol.GetComponent<Answer>().bgColor;

        } else if(!isOnBlank)
        {
            SetTarget(startPosition + Vector3.back);
            background.color = bgColor;
            transform.localScale = Vector3.one;
        }
        isDragging = false;

    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        targetPosition.z = -1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlankCell")
        {
            isOnBlank = true;
            blankPosition = collision.gameObject.transform.position;
            blankValue = collision.gameObject.GetComponent<Blank>().value;
            blankX = collision.gameObject.GetComponent<Blank>().x;
            blankY = collision.gameObject.GetComponent<Blank>().y;
            blank = collision.gameObject.transform;
        }
        if (collision.gameObject.tag == "AnswerCell")
        {
            isOnOtherAns = true;
            anotherCol = collision;
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
            blankPosition = collision.gameObject.transform.position;
            blankValue = collision.gameObject.GetComponent<Blank>().value;
            blankX = collision.gameObject.GetComponent<Blank>().x;
            blankY = collision.gameObject.GetComponent<Blank>().y;
            blank = collision.gameObject.transform;
            if (transform.GetComponent<Number>().value == collision.GetComponent<Blank>().value && !isDragging)
            {
                GameManager.instance.check[blankX, blankY] = true;

            }

        }
        if (collision.CompareTag("AnswerCell"))
        {
            isOnOtherAns = true;
        }
    }


    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -6;
            transform.position =  mousePosition + Vector3.up;
        }
        else
        {
            if (targetPosition == startPosition)
            {
                background.color = bgColor;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
    }
}

