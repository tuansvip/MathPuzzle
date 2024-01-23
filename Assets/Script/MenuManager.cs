using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject mainCamera, menu, level, highScore, levelBtnPrefab, lvContainer;

    bool isChooseLevel;
    bool isChooseHighScore;
    public bool isStart;
    private string savePath;
    public PlayerData playerData;
    string encryptKey = "iamnupermane4133bbce2ea2315a1916";
                         



    private void Awake()
    {
        instance = this;
        isChooseLevel = false;
        isChooseHighScore = false;
        isStart = false;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        Application.targetFrameRate = 144;
        playerData = LoadPlayerData();

        for (int  i = 1;  i <= 100;  i++)
        {
            if (LoadPlayerData().unlockLevel >= i)
            {
                GameObject btn = Instantiate(levelBtnPrefab, lvContainer.transform);
                btn.GetComponent<LevelBtn>().value = i;
                btn.GetComponent<LevelBtn>().isUnlocked = true;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                btn.name = "Level " + i;
            }
            else
            {
                GameObject btn = Instantiate(levelBtnPrefab, lvContainer.transform);
                btn.GetComponent<LevelBtn>().value = i;
                btn.GetComponent<LevelBtn>().isUnlocked = false;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                btn.name = "Level " + i;
            }
        }
        ResizeParent();
    }
    void ResizeParent()
    {
        lvContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(lvContainer.GetComponent<RectTransform>().sizeDelta.x, 20 + (0.5f*20));
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
            Debug.Log("Save file found!");
            string json = File.ReadAllText(savePath);
            json = Encryption.DecryptString(encryptKey, json);
            if (JsonUtility.FromJson<PlayerData>(json) == null)
            {
                Debug.LogWarning("Save file incorrect, creating a new one!");
                string json2 = JsonUtility.ToJson(new PlayerData(0, 0, 0, 1, 1));
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
            string json = JsonUtility.ToJson(new PlayerData(0, 0, 0, 1, 1));
            json = Encryption.EncryptString(encryptKey, json);
            File.WriteAllText(savePath, json);
            json = Encryption.DecryptString(encryptKey, json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
    public void Level()
    {
        SFXManager.instance.PlayClick();
        isChooseLevel = true;
        isChooseHighScore = false;
        highScore.SetActive(false);
        level.SetActive(true);
    }
    public void Backtomenu()
    {
        SFXManager.instance.PlayClick();
        isChooseLevel = false;
        isChooseHighScore = false;
        highScore.SetActive(false);
        level.SetActive(false);
    }
    public void HighScore()
    {
        SFXManager.instance.PlayClick();
        isChooseHighScore = true;
        isChooseLevel = false;
        highScore.SetActive(true);
        level.SetActive(false);
    }
    public void PlayHighestLevel()
    {
        SFXManager.instance.PlayClick();
        isStart = true;
        playerData.currentLevel = playerData.unlockLevel;
        SavePlayerData(playerData);
        StartCoroutine(StartLevel());
    }
    public void ContinueGame()
    {
        SFXManager.instance.PlayClick();
        isStart = true;
        StartCoroutine(StartLevel());
    }
    public void LevelSelected(int selectedLevel)
    {
        SFXManager.instance.PlayClick();
        isStart = true;
        playerData.currentLevel = selectedLevel;
        SavePlayerData(playerData);
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("sample");

    }    

    private void Update()
    {
        if (isChooseLevel)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(20, 0, -10), 0.1f);
        } else if (isChooseHighScore)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(20, -20, -10), 0.1f);
        }
        if (!isChooseLevel && !isChooseHighScore)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(20, 20, -10), 0.1f);
        }
        if (isStart)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0, 0, -10), 0.1f);
        }
    }

}
