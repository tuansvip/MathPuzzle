using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ToggleSwitch;
using Path = System.IO.Path;

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
    public GameObject musicSwitch;
    public GameObject soundSwitch;
    public GameObject vibrateSwitch;


    [Header("Buttons")]
    public Button homeButton;
    public Button shopButton;
    public Button dailyButton;
    public Button btnLeft;
    public Button btnRight;

    [Header("Others")]
    public Sprite hardImg;
    public Sprite normalImg;
    public Sprite easyImg;
    public int maxLevel = 2004;
    public int selectedDaily = DateTime.Today.Day;               
    public bool isStart;
    private string savePath;
    string encryptKey = "iamnupermane4133bbce2ea2315a1916";
    public PlayerData playerData;
    public bool isSetting;
    public int selectedLevel = 1;

    private void Awake()
    {
        instance = this;
        isStart = false;
        isSetting = false;
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
        musicSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isMusicOn;
        soundSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isSoundOn;
        vibrateSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isVibrateOn;
        
        shopPanel.GetComponent<RectTransform>().localPosition = Vector3.left * 10000;
        dailyPanel.GetComponent<RectTransform>().localPosition = Vector3.right * 10000;

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
        //Auto Generate Level
        /*        if (playerData.currentLevel < 2005)
                {
                    StartGame(1);
                }*/
        MenuBarBtn.instance.yOrigin = MenuBarBtn.instance.homeBtn.transform.position.y;
        MenuBarBtn.instance.SelectHome();
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
                string json2 = JsonUtility.ToJson(new PlayerData(1, PlayerData.Challenge.Level, 0, 0, false, true, true, true, DateTime.Now.Day));
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
            string json = JsonUtility.ToJson(new PlayerData(1, PlayerData.Challenge.Level, 0, 0, false, true, true, true, DateTime.Now.Day));
            json = Encryption.EncryptString(encryptKey, json);
            File.WriteAllText(savePath, json);
            json = Encryption.DecryptString(encryptKey, json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

    public void PlayChalenge()
    {
        StartGame(selectedLevel);
    }

    static bool CheckLevelExistence(string name)
    {
        TextAsset asset = Resources.Load<TextAsset>(Path.Combine("Level", name));
        if (asset == null)
        {
            return false;
        } else
        {
            return true;
        }
    }

    public void StartGame(int chalenge)
    {
        switch (chalenge)
        {
            case 1:
                playerData.challenge = PlayerData.Challenge.Level;
                if (CheckLevelExistence("Level" + playerData.currentLevel))
                {
                    SavePlayerData(playerData);
                    SceneManager.LoadScene("level");
                }
                else
                {
                    SavePlayerData(playerData);
                    SceneManager.LoadScene("sample");
                }

        break;
            case 2:
                if (playerData.day < 0 || playerData.daily[playerData.day]) return;
                playerData.challenge = PlayerData.Challenge.Daily;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 3:
                playerData.challenge = PlayerData.Challenge.Easy;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 4:
                playerData.challenge = PlayerData.Challenge.Medium;
                SavePlayerData(playerData);
                SceneManager.LoadScene("sample");
                break;
            case 5:
                playerData.challenge = PlayerData.Challenge.Hard;
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
        MenuBarBtn.instance.SelectHome();
    }
    public void Shop()
    {
        shopPanel.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
        homePanel.GetComponent<RectTransform>().DOMove(Vector3.right * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        dailyPanel.GetComponent<RectTransform>().DOMove(Vector3.right * 20000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        MenuBarBtn.instance.SelectShop();
    }
    public void Daily()
    {
        Calendar cal = GameObject.Find("Calendar").GetComponent<Calendar>();
        dailyPanel.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f);
        homePanel.GetComponent<RectTransform>().DOMove(Vector3.left * 10000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        shopPanel.GetComponent<RectTransform>().DOMove(Vector3.left * 20000 + new Vector3(0, canvas.GetComponent<RectTransform>().position.y, 0), 0.25f);
        MenuBarBtn.instance.SelectDaily();
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
        settingsPanel_Child.GetComponent<RectTransform>().DOMove(canvas.GetComponent<RectTransform>().position, 0.25f).OnComplete(() => { isSetting = true; });
    }
    public void CloseSetting()
    {
        settingsPanel_Child.GetComponent<RectTransform>().DOMove(Vector3.up * 10000 + canvas.GetComponent<RectTransform>().position, 0.5f).OnComplete(() => { settingsPanel.SetActive(false); isSetting = false; });
    }
    public void BtnLeftClick()
    {
        switch(selectedLevel){
            case 4:
                selectedLevel = 3;
                break;
            case 5:
                selectedLevel = 4;
                break;
            default: break;
        }
    }
    public void BtnRightClick()
    {
        switch(selectedLevel){
            case 3:
                selectedLevel = 4;
                break;
            case 4:
                selectedLevel = 5;
                break;
            default: break;
        }
    }
}
