using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ToggleSwitch;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Panels")]
    public GameObject selectChalengePanel;
    public GameObject selectChalengePanel_Child;
    public GameObject homePanel, shopPanel, dailyPanel;

    [Header("GameObjects")]
    public GameObject canvas;

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
                string json2 = JsonUtility.ToJson(new PlayerData(1, PlayerData.Chalenge.Level, 0, 0, false, true, true, true));
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
            string json = JsonUtility.ToJson(new PlayerData(1, PlayerData.Chalenge.Level, 0, 0, false, true, true, true));
            json = Encryption.EncryptString(encryptKey, json);
            File.WriteAllText(savePath, json);
            json = Encryption.DecryptString(encryptKey, json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

    public void PlayChalenge()
    {
        selectChalengePanel.SetActive(true);
        selectChalengePanel_Child.GetComponent<RectTransform>().DOMove(Vector3.zero + canvas.GetComponent<RectTransform>().position, 1f);
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
        homePanel.SetActive(true);

        shopPanel.SetActive(false);
        dailyPanel.SetActive(false);
    }
    public void Shop()
    {
        shopPanel.SetActive(true);

        homePanel.SetActive(false);
        dailyPanel.SetActive(false);
    }
    public void Daily()
    {
        dailyPanel.SetActive(true);

        homePanel.SetActive(false);
        shopPanel.SetActive(false);
    }
}
