using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum Difficult
    {
        Easy,
        Medium,
        Hard
    }
    List<int> hardLevel = new List<int> { 14, 20, 26, 37, 43, 49, 56, 58, 66, 73, 88, 100 };
    List<int> easyLevel = new List<int> { 1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 27, 28, 29, 30, 31, 38, 39, 40, 44, 45, 50, 51, 52, 53, 59, 60, 61, 62, 67, 68, 69, 74, 75, 76, 77, 78, 79, 80, 89, 90, 91, 92, 93 };
    List<int> mediumLevel = new List<int> { 8, 9, 10, 11, 12, 13, 18, 19, 21, 22, 23, 24, 25, 32, 33, 34, 35, 36, 41, 42, 46, 47, 48, 54, 55, 57, 63, 64, 65, 70, 71, 72, 81, 82, 83, 84, 85, 86, 87, 94, 95, 96, 97, 98, 99 };

    [Header("#Game Variable")]
    public int length;
    public bool[,] check;

    [Header("#Game Object")]
    public GameObject playzoneobj;
    public GameObject pool;
    public GameObject playHUD;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject ansSpawn;
    public GameObject spawn;
    public GameObject losePanel;
    public Blank selectedBlank = null;

    [Header("#Game Component")]
    public GenerateMath playzone;
    public PlayerData playerData;

    [Header("#Game Data")]
    public Difficult level;
    public float time;
    public bool isHighscore;
    public int lives = 3;


    float shortestTimeEasy;
    float shortestTimeMedium;
    float shortestTimeHard;
    bool isStart = false;
    private string savePath;
    string encryptKey = "iamnupermane4133bbce2ea2315a1916";

    private void Awake()
    {
        Debug.Log(Time.timeScale);
        if (instance == null)
        {
            instance = this;
        }
        isHighscore = false;
        time = 0;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        playerData = LoadPlayerData();
        Application.targetFrameRate = 144;
        if (hardLevel.IndexOf(playerData.currentLevel) != -1)
        {
            level = Difficult.Hard;
        } else if(easyLevel.IndexOf(playerData.currentLevel) != -1)
        {
            level = Difficult.Easy;
        } else if(mediumLevel.IndexOf(playerData.currentLevel) != -1)
        {
            level = Difficult.Medium;
        } 
        Debug.Log("Level: " + playerData.currentLevel + ";  Difficult: " + level);
        playzone.GetComponent<GenerateMath>().Generate(level);
        length = playzone.size;
        check = new bool[length, length];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length ; j++)
            {
                check[i, j] = true;
                if (playzone.grid[i, j] != null && playzone.grid[i, j].CompareTag("BlankCell"))
                {
                    check[i, j] = false;
                }
            }
        }

        isStart = true;
    }

    private void Update()
    {
        if (!isStart) return;
        time += Time.deltaTime;
        if (checkWin())
        {
            Victory();
        }
        if (lives <= 0)
        {
            Defeat();
        }
    }



    private void Defeat()
    {
        SFXManager.instance.PlayLose();
        isStart = false;
        Time.timeScale = 0;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        playHUD.SetActive(false);
        losePanel.SetActive(true);
    }

    public void Victory()
    {
        SFXManager.instance.PlayWin();
        isStart = false;
        switch (level)
        {
            case Difficult.Easy:
                if (time < playerData.easy || playerData.easy == 0)
                {
                    Debug.Log(time);
                    isHighscore = true;
                    playerData.easy = time;
                    Debug.Log(playerData.easy);
                    SavePlayerData(playerData);
                }
                break;
            case Difficult.Medium:
                if (time < playerData.medium || playerData.medium == 0)
                {
                    isHighscore = true;
                    playerData.medium = time;
                    SavePlayerData(playerData);
                }
                break;
            case Difficult.Hard:
                if (time < playerData.hard || playerData.hard == 0)
                {
                    isHighscore = true;
                    playerData.hard = time;
                    SavePlayerData(playerData);
                }
                break;
        }
        Time.timeScale = 0;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        playHUD.SetActive(false);
        if (playerData.currentLevel == playerData.unlockLevel)
        {
            playerData.unlockLevel++;
        }
        SavePlayerData(playerData);
        winPanel.SetActive(true);
    }
    public void NextLevel()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        playerData.currentLevel++;
        isHighscore = false;
        SavePlayerData(playerData);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    public bool checkWin()
    {
        bool win = true;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (check[i, j] == false)
                {
                    win = false;
                }
            }
        }
        return win;
    }

    public void Home()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void Restart()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        isHighscore = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Pause()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 0;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        playHUD.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void Resume()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        playzoneobj.SetActive(true);
        pool.SetActive(true);
        playHUD.SetActive(true);
        pausePanel.SetActive(false);
    }


    public void Hint()
    {
        List<GameObject> listAns = new List<GameObject>();
        for (int i = 0; i < ansSpawn.transform.childCount; i++)
        {
            listAns.Add(ansSpawn.transform.GetChild(i).gameObject);
        }
        GameObject ans = listAns[0];
        foreach (GameObject answer in listAns)
        {
            if (!answer.GetComponent<Answer>().isOnBlank)
            {
                ans = answer;
                break;
            }
        }
        List<GameObject> listobj = playzone.GetComponent<GenerateMath>().grid.Cast<GameObject>().ToList();
        Transform originalTransform = ans.transform;
        Transform targerTransform = ans.transform;
        foreach (GameObject obj in listobj)
        {
            if (obj != null && obj.CompareTag("BlankCell") && obj.GetComponent<Blank>().value == ans.GetComponent<Number>().value && !obj.GetComponent<Blank>().isOnAnswer)
            {
                targerTransform = obj.transform;
                break;
            }
        }
        Color originalColor = ans.GetComponent<Answer>().bgColor;
        ans.GetComponent<Answer>().background.color = new Color(0.9608f, 0.4588f, 0.8824f);
        ans.transform.DOMove(targerTransform.position, 1f);
        ans.transform.DOMove(originalTransform.position, 1f).SetDelay(1f);
        StartCoroutine(ChangeColor(ans.GetComponent<Answer>(), originalColor));
    }

    private IEnumerator ChangeColor(Answer answer, Color originalColor)
    {
        yield return new WaitForSeconds(2f);
        answer.background.color = originalColor;
    }
} 

public class PlayerData
{
    public float hard;
    public float medium;
    public float easy;
    public int currentLevel;
    public int unlockLevel;

    public PlayerData(float hard, float medium, float easy, int currentLevel, int unlockLevel)
    {
        this.hard = hard;
        this.medium = medium;
        this.easy = easy;
        this.currentLevel = currentLevel;
        this.unlockLevel = unlockLevel;
    }
}