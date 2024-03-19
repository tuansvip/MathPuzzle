using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuBarBtn : MonoBehaviour
{
    public static MenuBarBtn instance;
    public Color selectedBGColor, selectedIconColor, selectedStrokeColor, bgColor, iconColor, strokeColor;
    public GameObject homeBtn, shopBtn, dailyBtn, bg;
    public float yOffSet;
    public float yOrigin;
    private void Awake()
    {
        instance = this;
    }

    public void SelectHome()
    {
        homeBtn.transform.GetChild(0).GetComponent<Image>().DOColor(selectedIconColor, 0.5f);
        homeBtn.GetComponent<Outline>().DOColor(selectedStrokeColor, 0.5f);
        homeBtn.GetComponent<Image>().DOColor(selectedBGColor, 0.5f);
        homeBtn.transform.DOMoveY(yOrigin + yOffSet, 0.5f);
        homeBtn.transform.DOScale(1.5f, 0.5f);
        shopBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        shopBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        shopBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        shopBtn.transform.DOMoveY(yOrigin, 0.5f);
        shopBtn.transform.DOScale(1f, 0.5f);
        dailyBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        dailyBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        dailyBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        dailyBtn.transform.DOMoveY(yOrigin, 0.5f);
        dailyBtn.transform.DOScale(1f, 0.5f);
        bg.SetActive(true);
    }
    public void SelectShop()
    {
        shopBtn.transform.GetChild(0).GetComponent<Image>().DOColor(selectedIconColor, 0.5f);
        shopBtn.GetComponent<Outline>().DOColor(selectedStrokeColor, 0.5f);
        shopBtn.GetComponent<Image>().DOColor(selectedBGColor, 0.5f);
        shopBtn.transform.DOMoveY(yOrigin + yOffSet, 0.5f);
        shopBtn.transform.DOScale(1.5f, 0.5f);
        homeBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        homeBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        homeBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        homeBtn.transform.DOMoveY(yOrigin, 0.5f);
        homeBtn.transform.DOScale(1f, 0.5f);
        dailyBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        dailyBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        dailyBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        dailyBtn.transform.DOMoveY(yOrigin, 0.5f);
        dailyBtn.transform.DOScale(1f, 0.5f);
        bg.SetActive(true);
    }
    public void SelectDaily()
    {
        dailyBtn.transform.GetChild(0).GetComponent<Image>().DOColor(selectedIconColor, 0.5f);
        dailyBtn.GetComponent<Outline>().DOColor(selectedStrokeColor, 0.5f);
        dailyBtn.GetComponent<Image>().DOColor(selectedBGColor, 0.5f);
        dailyBtn.transform.DOMoveY(yOrigin + yOffSet, 0.5f);
        dailyBtn.transform.DOScale(1.5f, 0.5f);
        homeBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        homeBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        homeBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        homeBtn.transform.DOMoveY(yOrigin, 0.5f);
        homeBtn.transform.DOScale(1f, 0.5f);
        shopBtn.transform.GetChild(0).GetComponent<Image>().DOColor(iconColor, 0.5f);
        shopBtn.GetComponent<Outline>().DOColor(strokeColor, 0.5f);
        shopBtn.GetComponent<Image>().DOColor(bgColor, 0.5f);
        shopBtn.transform.DOMoveY(yOrigin, 0.5f);
        shopBtn.transform.DOScale(1f, 0.5f);
        bg.SetActive(true);
    }

}
