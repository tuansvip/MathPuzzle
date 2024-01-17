using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int value;
    public bool isUnlocked = false;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
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
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    void OnButtonClick()
    {
        MenuManager.instance.LevelSelected(value);
    }
}
