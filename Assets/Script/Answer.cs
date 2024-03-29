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
    public bool isDragging = false;
    public bool isOnBlank = false;
    public bool isOnOtherAns = false;
    public bool isCorrect = false;
    public bool isHinting = false;
    public Vector3 startPosition;
    public Vector3 startScale;
    public Vector3 targetPosition;
    Vector3 blankPosition;
    public Color bgColor;
    public Color wrongColor;
    public Color endColor;
    int blankValue;
    public int blankX = 0, blankY = 0;
    public Image background;
    Collider2D anotherCol;
    public Transform blank;

    private void Awake()
    {
        startPosition = transform.position;
        if (GameManager.instance.IsMobile())
        {
            startPosition *= Camera.main.orthographicSize * 2 * Camera.main.aspect / 10;
        }
        targetPosition = startPosition;

    }
    //Drag and Drop
    private void OnMouseDown()
    {
        if (GameManager.instance.isPause) return;
        if (GameManager.instance.selectedBlank != null)
        {
            GameManager.instance.AddState();
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
                if (GameManager.instance.playerData.isVibrateOn) Handheld.Vibrate();
                SFXManager.instance.PlayWrong();
            }
            GameManager.instance.selectedBlank.GetComponent<Blank>().bg.color = Color.white;
            GameManager.instance.selectedBlank.GetComponent<Blank>().isSelected = false;
            GameManager.instance.selectedBlank = null;
        }
        else
        {
            isDragging = true;
            background.color = bgColor;
            transform.DOScale(startScale, 0.5f).SetEase(Ease.OutQuart);
            transform.DOScale(startScale, 0.5f).SetEase(Ease.OutQuart);

        }

    }
    private void OnMouseUp()
    {
        transform.position = transform.position + Vector3.back * -5;
        if (!isOnOtherAns && isOnBlank)
        {
            GameManager.instance.AddState();
            if (isDragging) SFXManager.instance.PlayPaper();
            SetTarget(blankPosition);
            transform.DOScale(GameManager.instance.spawnParent.transform.localScale, 0.5f).SetEase(Ease.OutQuart);
            if (transform.GetComponent<Number>().value == blankValue)
            {
                background.color = bgColor;
            }
            else
            {
                background.color = wrongColor;
                if (GameManager.instance.playerData.isVibrateOn) Handheld.Vibrate();
                SFXManager.instance.PlayWrong();
            }      
        }
        else if (isOnOtherAns && isOnBlank)
        {
            GameManager.instance.AddState();
            if (isDragging) SFXManager.instance.PlayPaper();
            transform.DOScale(GameManager.instance.spawnParent.transform.localScale, 0.5f).SetEase(Ease.OutQuart);
            if (transform.GetComponent<Number>().value == blankValue)
            {
                background.color = bgColor;
            }
            else
            {
                background.color = wrongColor;
                if (GameManager.instance.playerData.isVibrateOn) Handheld.Vibrate();
                SFXManager.instance.PlayWrong();
            }
            SetTarget(blankPosition);
            anotherCol.GetComponent<Answer>().transform.localScale = startScale;
            anotherCol.GetComponent<Answer>().SetTarget(anotherCol.GetComponent<Answer>().startPosition);
            anotherCol.GetComponent<Answer>().background.color = anotherCol.GetComponent<Answer>().bgColor;

        } else if(!isOnBlank)
        {
            SetTarget(startPosition);
            background.color = bgColor;
            transform.localScale = startScale;
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
            {                
                isCorrect = false;
            }
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
            if (transform.GetComponent<Number>().value == collision.GetComponent<Blank>().value && !isDragging && !isHinting)
            {
                isCorrect = true;
            }

        }
        if (collision.CompareTag("AnswerCell"))
        {
            isOnOtherAns = true;
        }
    }


    void Update()
    {
        if (isHinting) return;
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -6;
            transform.position =  mousePosition + Vector3.up * 2;
            background.color = bgColor;
        }
        else
        {
            if (isOnBlank)
            {
                transform.DOScale(GameManager.instance.spawnParent.transform.localScale, 0.5f).SetEase(Ease.OutQuart);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
                
            }
            else
            {
                transform.DOScale(startScale, 0.5f).SetEase(Ease.OutQuart);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
                background.color = bgColor;
            }
            if (targetPosition == startPosition)            {
                transform.DOScale(startScale, 0.5f).SetEase(Ease.OutQuart);
                background.color = bgColor;
            }
        }
        if (GameManager.instance.checkWin())
        {
            SetTarget(blank.position);
        }
    }
}

