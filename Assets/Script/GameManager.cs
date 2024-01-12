using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum Level
    {
        Easy,
        Medium,
        Hard
    }

    [Header("#Game Variable")]
    public int length;
    public bool[,] check;

    [Header("#Game Object")]
    public GameObject playzoneobj;
    public GameObject pool;
    public GameObject playHUD;
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("#Game Component")]
    public GenerateMath playzone;

    [Header("#Game Data")]
    public Level level;
    public float time;
    public bool isHighscore;

    PlayerData playerData;
    float shortestTimeEasy;
    float shortestTimeMedium;
    float shortestTimeHard;
    private string savePath;


    private void Awake()
    {
        isHighscore = false;
        if (instance == null)
        {
            instance = this;
        }
        time = 0;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        Application.targetFrameRate = 144;
        playerData = LoadPlayerData();

    }

    private void Start()
    {
        length = playzone.size;
        check = new bool[length, length];
        for (int i = 0; i < length; i++) { 
            for (int j = 0; j < length; j++)
            {
                check[i, j] = true;
                if (playzone.grid[i,j] != null && playzone.grid[i,j].tag == "BlankCell")
                {
                    check[i, j] = false;
                }
            }
        }
        for(int y = length - 1; y < 0; y--)
        {
                Debug.Log(check[0, y] + " " + check[1,y] + " " + check[2, y] + " " + check[3, y] + " " + check[4, y] + " " + check[5, y] + " " + check[6, y] + " " + check[7, y]);
        }

    }
    private void Update()
    {
        time += Time.deltaTime;
        if (checkWin())
        {
            Victory();
        }
    }

    private void Victory()
    {
        switch (level)
        {
            case Level.Easy:
                if (time < playerData.easy || playerData.easy == 0)
                {
                    Debug.Log(time);
                    isHighscore = true;
                    playerData.easy = time;
                    Debug.Log(playerData.easy);
                    SavePlayerData(playerData);
                }
                break;
            case Level.Medium:
                if (time < playerData.medium || playerData.medium == 0)
                {
                    isHighscore = true;
                    playerData.medium = time;
                    SavePlayerData(playerData);
                }
                break;
            case Level.Hard:
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
        winPanel.SetActive(true);
    }

    public void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log(data.easy);
        File.WriteAllText(savePath, json);
        Debug.Log(LoadPlayerData().easy + " " + LoadPlayerData().easy + " " + LoadPlayerData().easy);
    }

    public PlayerData LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            Debug.Log("Save file found!");
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found, creating a new one!");
            string json = JsonUtility.ToJson(new PlayerData(0, 0, 0));
            File.WriteAllText(savePath, json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
        private bool checkWin()
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
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void Restart()
    {
        Time.timeScale = 1;
        isHighscore = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        playHUD.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        playzoneobj.SetActive(true);
        pool.SetActive(true);
        playHUD.SetActive(true);
        pausePanel.SetActive(false);
    }
    
}
public class PlayerData
{
    public float hard;
    public float medium;
    public float easy;



    public PlayerData(float hard, float medium, float easy)
    {
        this.hard = hard;
        this.medium = medium;
        this.easy = easy;
    }
}