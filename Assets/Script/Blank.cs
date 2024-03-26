using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Blank : MonoBehaviour
{
    public int value;
    public Color clickedColor;
    public Image bg;
    public int x, y;
    public int index = 0;
    public bool isOnAnswer;
    public bool isSelected = false;
    public bool isFilled = false;
    public Answer answer;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("AnswerCell"))
        {

            isOnAnswer = true;
            answer = collision.GetComponent<Answer>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AnswerCell"))
        {
            isOnAnswer = false;
        }
    }
    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().name == "loading")
        {

            if (Loading.instance.blankTuto == null && Loading.instance.ans1.GetComponent<TutorialAns>().correct)
            {
                // Nếu không, chọn ô hiện tại
                isSelected = true;
                bg.color = clickedColor;
                Loading.instance.blankTuto = this;
            }
        }
        else
        {
            if (!isOnAnswer)
            {
                SFXManager.instance.PlayPaper();

                if (GameManager.instance.selectedBlank == null)
                {
                    // Nếu không, chọn ô hiện tại
                    isSelected = true;
                    bg.color = clickedColor;
                    GameManager.instance.selectedBlank = this;
                }
                else
                {
                    GameManager.instance.selectedBlank.bg.color = Color.white;
                    GameManager.instance.selectedBlank.isSelected = false;
                    GameManager.instance.selectedBlank = null;

                    isSelected = true;
                    bg.color = clickedColor;

                    GameManager.instance.selectedBlank = this;
                }
            }
        }
    }
}

