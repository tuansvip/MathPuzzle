using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public enum SwitchType
    {
        Sound,
        Music,
        Vibration
    }

    public bool isOn;
    public GameObject switchObj;
    public RectTransform pointOn;
    public RectTransform pointOff;
    public SwitchType switchType;
    public Color onColor, offColor;
    public TextMeshProUGUI textOn, textOff;

    private void OnEnable()
    {
        Init();
    }

    public void Click()
    {
        SFXManager.instance.PlayClick();
        if (!isOn)
        {
            switchObj.GetComponent<RectTransform>().DOMove(pointOn.position, 0.2f);
            GetComponent<Image>().DOColor(onColor, 0.1f);
            if (switchType == SwitchType.Sound)
            {
                SFXManager.instance.UnmuteSfx();
            }
            else if (switchType == SwitchType.Music)
            {
                SFXManager.instance.UnmuteMusic();
            }
            textOn.gameObject.SetActive(true);
            textOff.gameObject.SetActive(false);
            isOn = true;
        }
        else
        {
            GetComponent<Image>().DOColor(offColor, 0.1f);
            switchObj.GetComponent<RectTransform>().DOMove(pointOff.position, 0.2f);
            if (switchType == SwitchType.Sound)
            {
                SFXManager.instance.MuteSfx();
            }
            else if (switchType == SwitchType.Music)
            {
                SFXManager.instance.MuteMusic();
            }
            textOff.gameObject.SetActive(true);
            textOn.gameObject.SetActive(false);
            isOn = false;
        }
    }
    public void Init()
    {
        if (isOn)
        {
            switchObj.GetComponent<RectTransform>().position =  pointOn.position;
            GetComponent<Image>().color = onColor;
            if (switchType == SwitchType.Sound)
            {
                SFXManager.instance.UnmuteSfx();
            }
            else if (switchType == SwitchType.Music)
            {
                SFXManager.instance.UnmuteMusic();
            }
            textOn.gameObject.SetActive(true);
            textOff.gameObject.SetActive(false);
        }
        else
        {
            GetComponent<Image>().color = offColor;
            switchObj.GetComponent<RectTransform>().position = pointOff.position;            
            if (switchType == SwitchType.Sound)
            {
                SFXManager.instance.MuteSfx();
            }
            else if (switchType == SwitchType.Music)
            {
                SFXManager.instance.MuteMusic();
            }
            textOff.gameObject.SetActive(true);
            textOn.gameObject.SetActive(false);
        }
    }
}
