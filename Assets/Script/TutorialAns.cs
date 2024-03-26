using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAns : MonoBehaviour
{
    public bool correct = false;
    bool isDragging = false;
    Vector3 startPos;
    Transform blank;
    private void Awake()
    {
        startPos = transform.position;
    }
    private void OnMouseDown()
    {
        if (Loading.instance.blankTuto != null)
        {
            transform.DOMove(Loading.instance.blankTuto.transform.position, 0.25f);
            correct = true;
            return;
        }
        if (!Loading.instance.ans1.GetComponent<TutorialAns>().correct) { 
            isDragging = true;
        }
    }
    private void OnMouseUp()
    {
        isDragging = false;
        if (blank != null)
        {
            transform.DOMove(blank.position, 0.25f);
            correct = true;
        }
        else if (Loading.instance.blankTuto == null)
        {
            transform.DOMove(startPos, 0.25f);
            correct = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlankCell")
        {
            blank = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BlankCell")
        {
            blank = null;
        }
    }
    private void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.up * 2;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
