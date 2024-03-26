using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Loading : MonoBehaviour
{
    public static Loading instance;
    float timer = 0;
    public GameObject bar;
    public ParticleSystem winTuto;
    public Blank blankTuto;
    public BoxCollider2D blank1, ans1;
    public BoxCollider2D blank2, ans2;
    public GameObject tuto, canvas;
    public Transform cursor;
    bool tutorial = false;
    public bool tuto1 = false, tuto2 = false, endtuto = false, tuto2next = false;
    public TextMeshProUGUI tutoText;
    Tween tween;
    private void Start()
    {
        Application.targetFrameRate = 144;
        instance = this;
        if (!File.Exists(Application.persistentDataPath + "/IAMNUPERMAN.json"))
        {

            tutorial = true;
        }
    }


    private void Update()
    {
        timer += Time.deltaTime * Random.Range(0f, 1.2f);
        bar.GetComponent<Scrollbar>().size = timer / 3;
        if (timer >= 3 && !tutorial)
        {
            SceneManager.LoadScene("menu");
        }
        if (timer >=3 && tutorial && !tuto1)
        {
            tuto.SetActive(true);
            canvas.SetActive(false);
            blank2.enabled = false;
            ans2.enabled = false;
            blank1.enabled = true;
            ans1.enabled = true;
            cursor.position = ans1.transform.position - new Vector3(-0.8f, 0.8f, 1);
            tween = cursor.DOMove(blank1.transform.position - new Vector3(-0.8f, 0.8f, 1), 2f).SetDelay(0.3f).SetLoops(-1);
            tutoText.text = "Drag a candidate to the correct cell!";
            tuto1 = true;
        }
        if (ans1.GetComponent<TutorialAns>().correct && !tuto2)
        {
            NextTuto();
        }
        if (blankTuto != null && !tuto2next)
        {
            tween.Kill();
            cursor.position = ans2.transform.position - new Vector3(-0.8f, 0.8f, 1);
            cursor.localScale = cursorScale;
            Debug.Log(cursorScale);
            Debug.Log(cursor.localScale);
            DoScaleAnim();
            tutoText.text = "Then select the correct blank candidate";
            tuto2next = true;
        }
        if (ans2.GetComponent<TutorialAns>().correct && !endtuto)
        {
            tween.Kill();
            winTuto.Play();
            cursor.gameObject.SetActive(false);
            tutoText.text = "Congratulations! You have completed the tutorial!";
            endtuto = true;
            StartCoroutine(EndTuto());
        }
    }

    private void DoScaleAnim()
    {
            tween = cursor.DOScale(cursor.localScale * 0.8f, 0.8f).SetEase(Ease.InOutQuad).OnComplete(() => tween = cursor.DOScale(cursorScale, 1.5f).SetEase(Ease.InOutQuad).OnComplete(DoScaleAnim));
    }

    Vector3 cursorScale;
    private IEnumerator EndTuto()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("menu");
    }

    public void NextTuto()
    {
        tween.Kill();
        cursor.position = blank2.transform.position - new Vector3(-0.8f, 0.8f, 1);
        blank1.enabled = false;
        ans1.enabled = false;
        blank2.enabled = true;
        ans2.enabled = true;
        cursorScale = cursor.localScale;
        Debug.Log(cursorScale);
        Debug.Log(cursor.localScale);
        DoScaleAnim();
        tutoText.text = "You can also select an empty cell";
        tuto2 = true;
    }
}
