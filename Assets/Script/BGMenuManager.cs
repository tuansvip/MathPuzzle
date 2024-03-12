using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BGMenuManager : MonoBehaviour
{
    public Color bgColor, numColor;

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }


    private IEnumerator Blink()
    {
        while (true)
        {
            GetComponent<Text>().text = Random.Range(0, 10).ToString();
            GetComponent<Text>().DOColor(numColor, Random.Range(1f, 2f));
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            GetComponent<Text>().DOColor(bgColor, Random.Range(1f, 2f));
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

}
