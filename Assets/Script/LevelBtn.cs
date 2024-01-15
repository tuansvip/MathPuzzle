using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBtn : MonoBehaviour
{
    public int value;
    public bool isUnlocked = false;

    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            SFXManager.instance.PlayClick();
            MenuManager.instance.isStart = true;
            MenuManager.instance.level.SetActive(false);
            MenuManager.instance.camera.GetComponent<Animator>().SetTrigger("Start");
            MenuManager.instance.StartCoroutine(MenuManager.instance.LoadLevel(value));
        });
    }
    private void Update()
    {
        if (isUnlocked)
        {
            GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }
}
