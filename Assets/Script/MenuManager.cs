using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ToggleSwitch;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Panels")]
    public GameObject selectChalengePanel;
    public GameObject selectChalengePanel_Child;
    public GameObject homePanel, shopPanel, dailyPanel;
    public GameObject settingsPanel;
    public GameObject settingsPanel_Child;

    [Header("GameObjects")]
    public GameObject canvas;
    public ToggleSwitch musicSwitch;
    public ToggleSwitch soundSwitch;
    public ToggleSwitch vibrateSwitch;


    [Header("Buttons")]
    public Button homeButton;
    public Button shopButton;
    public Button dailyButton;

    [Header("Others")]
    public int maxLevel = 2004;
    public int selectedDaily = DateTime.Today.Day;               
    public bool isStart;
    private string savePath;
    public PlayerData playerData;
    string encryptKey = "iamnupermane4133bbce2ea2315a1916";
    

    private void Awake()
    {
        instance = this;
        isStart = false;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        Application.targetFrameRate = 144;
        playerData = LoadPlayerData();
        if (playerData.month != DateTime.Now.Month)
        {
            playerData.month = DateTime.Now.Month;
            playerData.daily = new bool[37];
            for (int i = 0; i < 37; i++)
            {
                playerData.daily[i] = false;
            }
            SavePlayerData(playerData);
        }
        musicSwitch.isOn = playerData.isMusicOn;
        soundSwitch.isOn = playerData.isSoundOn;
        vibrateSwitch.isOn = playerData.isVibrateOn;
        
        shopPanel.GetComponent<RectTransform>().localPosition = Vector3.left * 10000;
        dailyPanel.GetComponent<RectTransform>().localPosition = Vector3.right * 10000;
        homeButton.GetComponent<Image>().color = Color.red;
    }
    private void Start()
    {
        if (playerData.isMusicOn)
        {
            SFXManager.instance.UnmuteMusic();
        }
        else
        {
            SFXManager.instance.MuteMusic();
        }
        if (playerData.isSoundOn)
        {
            SFXManager.instance.UnmuteSfx();
        }
        else
        {
            SFXManager.instance.MuteSfx();
        }

    }

    public void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        json = Encryption.EncryptString(encryptKey, json);
        File.WriteAllText(savePath, json);
    }

    public PlayerData LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            json = Encryption.DecryptString(encryptKey, json);
            if (JsonUtility.FromJson<PlayerData>(json) == null)
            {
                Debug.LogWarning("Save file incorrect, creating a new one!");
                string json2 = JsonUtility.ToJson(new PlayerData(1, PlayerData.Chalenge.Level, 0, 0, false, true, true, true, DateTime.Now.Day));
                json2 = Encryption.EncryptString(encryptKey, json2);
                File.WriteAllText(savePath, json2);
                json2 = Encryption.DecryptString(encryptKey, json2);
                return JsonUtility.FromJson<PlayerData>(json2);
            }
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found, creating a new one!");
            string json = JsonUtility.ToJson(new PlayerData(1, PlayerData.Chalenge.Level, 0, 0, false, true, true, true, DateTime.Now.Day));
            json = Encryption.EncryptString(encryptKey, json);
            File.WriteAllText(savePath, json);
            json = Encryption.DecryptString(encryptKey, json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

    public void PlayChalenge()
    {
        selectChalengePanel.SetActive(true);
        selectChalengePanel_Child.GetComponent<RectTransform>().DOMove(Vector3.zero + canvas.GetComponent<RectTransform>().position, 0.5f);
    }
    public void CloseChalenge()
    {
        selectChalengePanel_Child.GetComponent<RectTransform>().DOMove(Vector3.right * 10000 + canvas.GetComponent<RectTransform>().position, 0.4f).OnComplete(() => selectChalengePanel.SetActive(false));
    }

        
    

    public void StartGame(int chalenge)
    {
        switch (chalenge)
        {
            case 1:
                playerData.chalenge = PlayerData.Chalenge.Level;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 2:
                if (playerData.day < 0 || playerData.daily[playerData.day]) return;
                playerData.chalenge = PlayerData.Chalenge.Daily;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 3:
                playerData.chalenge = PlayerData.Chalenge.Easy;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 4:
                playerData.chalenge = PlayerData.Chalenge.Medium;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 5:
                playerData.chalenge = PlayerData.Chalenge.Hard;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;

        }     
    }
    
    public void Home()
    {
        homePanel.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
        shopPanel.GetComponent<RectTransform>().DOMove(Vector3.left * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        dailyPanel.GetComponent<RectTransform>().DOMove(Vector3.right * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        homeButton.GetComponent<Image>().color = Color.red;
        shopButton.GetComponent<Image>().color = Color.white;
        dailyButton.GetComponent<Image>().color = Color.white;
    }
    public void Shop()
    {
        shopPanel.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
        homePanel.GetComponent<RectTransform>().DOMove(Vector3.right * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        dailyPanel.GetComponent<RectTransform>().DOMove(Vector3.right * 20000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        homeButton.GetComponent<Image>().color = Color.white;
        shopButton.GetComponent<Image>().color = Color.red;
        dailyButton.GetComponent<Image>().color = Color.white;
    }
    public void Daily()
    {
        Calendar cal = GameObject.Find("Calendar").GetComponent<Calendar>();
        dailyPanel.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
        homePanel.GetComponent<RectTransform>().DOMove(Vector3.left * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        shopPanel.GetComponent<RectTransform>().DOMove(Vector3.left * 20000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        homeButton.GetComponent<Image>().color = Color.white;
        shopButton.GetComponent<Image>().color = Color.white;
        dailyButton.GetComponent<Image>().color = Color.red;
    }
    public void SaveSetting()
    {
        playerData.isSoundOn = musicSwitch.GetComponent<ToggleSwitch>().isOn;
        playerData.isMusicOn = soundSwitch.GetComponent<ToggleSwitch>().isOn;
        playerData.isVibrateOn = vibrateSwitch.GetComponent<ToggleSwitch>().isOn;
        SavePlayerData(playerData);
    }
    public void SettingClick()
    {
        settingsPanel.SetActive(true);
        settingsPanel_Child.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
    }
    public void CloseSetting()
    {
        settingsPanel_Child.GetComponent<RectTransform>().DOMove(Vector3.up * 10000 + canvas.GetComponent<RectTransform>().position, 0.25f).OnComplete(() => settingsPanel.SetActive(false));
    }
}
