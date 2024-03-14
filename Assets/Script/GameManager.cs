﻿using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
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
    public Stack<List<Vector3>> answerPositionHistory = new Stack<List<Vector3>>();
    public enum Difficult
    {
        Easy,
        Medium,
        Hard
    }
    List<int> hardLevel = new List<int> { 14, 20, 26, 37, 43, 49, 56, 58, 66, 73, 88, 111, 119, 125, 129, 134, 137, 145, 150, 166, 179, 191, 195, 214, 219, 227, 236, 239, 244, 253, 257, 271, 279, 286, 290, 295, 302, 305, 311, 316, 324, 341, 353, 359, 370, 393, 400, 405, 410, 415, 426, 431, 438, 444, 447, 455, 469, 473, 488, 493, 502, 505, 516, 525, 533, 539, 544, 550, 556, 558, 576, 581, 588, 589, 595, 611, 615, 643, 651, 659, 670, 687, 688, 693, 696, 701, 707, 715, 741, 746, 751, 755, 761, 766, 774, 784, 790, 801, 805, 825, 829, 835, 841, 845, 855, 862, 866, 871, 880, 887, 895, 904, 915, 920, 930, 936, 942, 944, 949, 958, 975, 980, 985, 995, 1000, 1010, 1029, 1040, 1045, 1049, 1063, 1068, 1090, 1097, 1100, 1105, 1119, 1128, 1145, 1149, 1158, 1166, 1172, 1178, 1183, 1191, 1203, 1205, 1210, 1214, 1218, 1248, 1253, 1262, 1279, 1291, 1297, 1298, 1305, 1316, 1320, 1322, 1326, 1333, 1338, 1346, 1359, 1369, 1370, 1381, 1385, 1392, 1393, 1410, 1413, 1431, 1434, 1445, 1456, 1460, 1469, 1478, 1486, 1499, 1512, 1521, 1522, 1530, 1537, 1550, 1561, 1569, 1575, 1581, 1589, 1595, 1600, 1608, 1623, 1625, 1630, 1635, 1641, 1644, 1647, 1653, 1656, 1667, 1677, 1695, 1715, 1726, 1728, 1741, 1745, 1751, 1755, 1766, 1767, 1774, 1779, 1791, 1794, 1805, 1814, 1821, 1825, 1845, 1854, 1862, 1877, 1880, 1884, 1887, 1896, 1901, 1915, 1919, 1926, 1929, 1930, 1940, 1942, 1949, 1961, 1971, 1980, 1985, 1993, 2000, 2004 };
    List<int> easyLevel = new List<int> { 1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 27, 28, 29, 30, 31, 38, 39, 40, 44, 45, 50, 51, 52, 53, 59, 60, 61, 62, 67, 68, 69, 74, 75, 76, 77, 78, 79, 80, 89, 90, 91, 92, 93 };

    [Header("#Game Variable")]
    public int length;
    public int stepCount;

    [Header("#Game Object")]
    public GameObject playzoneobj;
    public GameObject pool;
    public GameObject playHUD;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject supportPanel;
    public GameObject ansSpawn;
    public GameObject spawn;
    public GameObject spawnParent;
    public GameObject losePanel;
    public GameObject musicToggle;
    public GameObject soundToggle;
    public GameObject vibrateToggle;
    public GameObject x2CoinBtn;
    public GameObject gameplay;
    public Blank selectedBlank = null;
    public int maxLevel = 2004;

    [Header("#Prefabs")]
    public GameObject answerPrefab;
    public GameObject blankPrefab;
    public GameObject numberPrefab;
    public GameObject opPrefab;
    public GameObject _75coinPrefab;


    [Header("#Game Component")]
    public GenerateMath playzone;
    public PlayerData playerData;

    [Header("#Game Data")]
    public Difficult level;
    public float time;
    public bool isHighscore;
    public bool hintMoving = false;

    public bool isSorted = false;
    bool isStart = false;
    private string savePath;
    string encryptKey = "iamnupermane4133bbce2ea2315a1916";
    public bool isPause = false;
    int loseCount;
    private void Awake()
    {
        loseCount = 0;
        if (instance == null)
        {
            instance = this;
        }
        Debug.Log("Height: " + Screen.height + "; Width: " + Screen.width);
        isHighscore = false;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        playerData = LoadPlayerData(); 
        Debug.Log("Money =" + playerData.money);
        Debug.Log("Music: " + playerData.isMusicOn + "; Sound: " + playerData.isSoundOn + "; Vib: " + playerData.isVibrateOn);
        soundToggle.GetComponent<ToggleSwitch>().isOn = playerData.isSoundOn;
        musicToggle.GetComponent<ToggleSwitch>().isOn = playerData.isMusicOn;
        vibrateToggle.GetComponent<ToggleSwitch>().isOn = playerData.isVibrateOn;
        Application.targetFrameRate = 144;
        if (playerData.challenge == PlayerData.Challenge.Easy)
        {
            level = Difficult.Easy;
            time = 45;
        }
        else if (playerData.challenge == PlayerData.Challenge.Medium)
        {
            level = Difficult.Medium;
            time = 60;
        }
        else if (playerData.challenge == PlayerData.Challenge.Hard || playerData.challenge == PlayerData.Challenge.Daily)
        { 
            level = Difficult.Hard;
            time = 90;
        }
        else if (playerData.challenge == PlayerData.Challenge.Level)
        {
            if (hardLevel.IndexOf(playerData.currentLevel) != -1)
            {
                level = Difficult.Hard;
                time = 90;
            }
            else if (easyLevel.IndexOf(playerData.currentLevel) != -1)
            {
                level = Difficult.Easy;
                time = 45;
            }
            else
            {
                level = Difficult.Medium;
                time = 60;
            }
        }

        Debug.Log("Day: " + playerData.day + ";  Difficult: " + level + "; Chalenge: " + playerData.challenge);
        if (SceneManager.GetActiveScene().name == "sample")
        {
            playzone.Generate(level);
            length = playzone.size;
            if (playerData.challenge == PlayerData.Challenge.Level)
            {
                SaveLevel("/Level" +playerData.currentLevel + ".json");
            }
        } 
        if (playerData.challenge == PlayerData.Challenge.Level && SceneManager.GetActiveScene().name == "level")
        {
            LoadLevel("Level" + playerData.currentLevel);
            playzone.Generate(level);
        }
        Debug.Log(playerData.challenge);
        Debug.Log(playerData.currentLevel);
        Debug.Log("Width " + Screen.width + " Heigh " + Screen.height + "\n" + Camera.main.orthographicSize);
        isStart = true;
        //auto generate level
/*        if (SceneManager.GetActiveScene().name == "sample" && playerData.challenge == PlayerData.Challenge.Level && playerData.currentLevel < 2005)
        {
            Victory();
            SceneManager.LoadScene("menu");
        }*/
        //
    }
    public bool IsMobile()
    {
        float screenRatio = (float)Screen.height / (float)Screen.width;

        // Xác định loại thiết bị dựa trên kích thước màn hình
        if (screenRatio < 1.77f)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    public void ResizeGameplay()
    {
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;     
        gameplay.transform.localScale = new Vector3(cameraWidth / 10, cameraWidth / 10, 1);
    }

    static bool CheckLevelExistence(string name)
    {
        string prefabPath = "Assets/Level/" + name;

        if (File.Exists(prefabPath))
        {
            Debug.Log("Prefab exists at path: " + prefabPath);
            return true;
        }
        else
        {
            Debug.LogWarning("Prefab does not exist at path: " + prefabPath);
            return false;
        }
    }
    public void SaveLevel(string fileName)
    {
        LevelModel levelModel = new LevelModel();
        levelModel.ArrayModel = playzone.gridModel;
        Debug.Log("ArrayModel: " + levelModel.ArrayModel.Length);

        levelModel.ArrayValue = playzone.values;
        Debug.Log("ArrayValue: " + levelModel.ArrayValue.Length);
        GameObject[] listAns = GameObject.FindGameObjectsWithTag("AnswerCell");

        string json = JsonConvert.SerializeObject(levelModel);
        File.WriteAllText("Assets/Resources/Level/" + fileName, json);
        Debug.Log("Level exist " + File.Exists("Assets/Resources/Level/" + fileName));
    }
    public void LoadLevel(string path)
    {           
        LevelModel levelModel = new LevelModel();

        TextAsset json = Resources.Load<TextAsset>(Path.Combine("Level" , path));
        string level = json.text;
        levelModel = JsonConvert.DeserializeObject<LevelModel>(level);
        playzone.gridModel = levelModel.ArrayModel;
        playzone.values = levelModel.ArrayValue;
    }

    public void AddState()
    {
        answerPositionHistory.Push(ansSpawn.GetComponentsInChildren<Answer>().Select(x => x.targetPosition).ToList());
    }
    public void UndoState()
    {
        if (answerPositionHistory.Count == 0) return;
        List<Vector3> tempPos = answerPositionHistory.Pop();
        for (int i = 0; i < tempPos.Count; i++)
        {
            ansSpawn.transform.GetChild(i).GetComponent<Answer>().targetPosition = tempPos[i];
        }

    }

    private void Update()
    {
     
        if (!isStart || isPause) return;
        time -= Time.deltaTime;
        if (checkWin())
        {
            Victory();
        }
        if (time <= 0)
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
        losePanel.SetActive(true);
        if (loseCount > 0)
        {
            GameObject.Find("Buy +60s").GetComponent<Button>().onClick.RemoveListener(Buy60s);
            GameObject.Find("Buy +60s").GetComponent<Button>().onClick.AddListener(Home);
            GameObject.Find("+60s Text").GetComponent<TextMeshProUGUI>().text = "Home";
            GameObject.Find("+45s").GetComponent<Button>().interactable = false;
        }
    }

    public void Victory()
    {
        SFXManager.instance.PlayWin();
        isStart = false;
        switch (playerData.challenge)
        {
            case PlayerData.Challenge.Level:
                switch (level)
                {
                    case Difficult.Easy:
                        playerData.money += 5;
                        SavePlayerData(playerData);

                        break;
                    case Difficult.Medium:
                        playerData.money += 10;
                        SavePlayerData(playerData);
                        break;
                    case Difficult.Hard:
                        playerData.money += 15;
                        SavePlayerData(playerData);
                        break;
                }
                if (playerData.challenge == PlayerData.Challenge.Level && playerData.currentLevel < maxLevel)
                {
                    playerData.currentLevel++;
                }
                playerData.hasLevel = false;
                break;
            case PlayerData.Challenge.Daily:
                playerData.money += 25;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Easy:
                playerData.money += 3;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Medium:
                playerData.money += 4;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Hard:
                playerData.money += 7;
                SavePlayerData(playerData);
                break;
        }
        isPause = true;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        playHUD.SetActive(false);

        if(playerData.challenge == PlayerData.Challenge.Daily)
        {
            playerData.daily[playerData.day] = true;
        }
        SavePlayerData(playerData);
        winPanel.SetActive(true);
        StartCoroutine(MoveMoney(10, GameObject.Find("CoinWin").GetComponent<RectTransform>()));
    }
    public void NextLevel()
    {
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
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
    public bool checkWin()
    {
        bool win = true;


        GameObject[] listAns;
        listAns = GameObject.FindGameObjectsWithTag("AnswerCell");
        if (listAns.Length == 0) return false;
        foreach (GameObject ans in listAns)
        {
            if (!ans.GetComponent<Answer>().isCorrect)
            {
                win = false;
                break;
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
        isPause = true;
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        pausePanel.SetActive(true);
    }
    public void Resume()
    {
        isPause = false;
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;;
        pausePanel.SetActive(false);
        supportPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void SaveSetting()
    {
        playerData.isSoundOn = soundToggle.GetComponent<ToggleSwitch>().isOn;
        playerData.isMusicOn = musicToggle.GetComponent<ToggleSwitch>().isOn;
        playerData.isVibrateOn = vibrateToggle.GetComponent<ToggleSwitch>().isOn;
        SavePlayerData(playerData);
    }
    public void Hint()
    {
        if (playerData.money >= 75 && !hintMoving)
        {
            hintMoving = true;
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
            ans.transform.DOMove(targerTransform.position + Vector3.back * 2, 1f);
            ans.transform.DOMove(originalTransform.position, 1f).SetDelay(1.5f).OnComplete(()=> hintMoving = false);
            playerData.money -= 75;
            GameObject minus75 = Instantiate(_75coinPrefab, GameObject.Find("MoneyGameplay").transform);
            minus75.GetComponent<RectTransform>().position = GameObject.Find("MoneyGameplay").GetComponent<RectTransform>().position;
            minus75.GetComponent<RectTransform>().DOMove(minus75.GetComponent<RectTransform>().position + Vector3.down, 1f);
            minus75.GetComponent<Text>().DOColor(new Color(255, 0, 0, 0), 1f).OnComplete(() => Destroy(minus75));
        }
        if (playerData.money < 75)
        {
            isPause = true;
            SFXManager.instance.PlayClick();
            Time.timeScale = 1;
            supportPanel.SetActive(true);
        }

        SavePlayerData(playerData);
    }
    public void HintAds()
    {
        //ADs here
        Resume();
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
        ans.transform.DOMove(targerTransform.position + Vector3.back * 2, 1f);
        ans.transform.DOMove(originalTransform.position, 1f).SetDelay(1.5f);
    }
    public void BuySupport()
    {
        Time.timeScale = 1;
        isStart = true;
        losePanel.SetActive(false);
        playzoneobj.SetActive(true);
        pool.SetActive(true);
        time += 60;
        loseCount++;
        StartCoroutine(HintAfterBuySupport());
    }
    private IEnumerator HintAfterBuySupport()
    {
        yield return new WaitForSeconds(0.2f);
        HintAds();
    }
    
    public void Buy60s()
    {
        Time.timeScale = 1;
        isStart = true;
        losePanel.SetActive(false);
        playzoneobj.SetActive(true);
        pool.SetActive(true);
        time+= 60;
        loseCount++;
    }
    public void Sorting()
    {
        if (isSorted)
        {
            playzone.GetComponent<GenerateMath>().SuffleAnswers();
            isSorted = false;
        }
        else
        {
            playzone.GetComponent<GenerateMath>().SortAnswers();
            isSorted = true;
        }
    }
    public void X2Coin()
    {
        switch (playerData.challenge)
        {
            case PlayerData.Challenge.Level:
                switch (level)
                {
                    case Difficult.Easy:
                        playerData.money += 5;
                        SavePlayerData(playerData);

                        break;
                    case Difficult.Medium:
                        playerData.money += 10;
                        SavePlayerData(playerData);
                        break;
                    case Difficult.Hard:
                        playerData.money += 15;
                        SavePlayerData(playerData);
                        break;
                }
                break;
            case PlayerData.Challenge.Daily:
                playerData.money += 25;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Easy:
                playerData.money += 3;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Medium:
                playerData.money += 4;
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Hard:
                playerData.money += 7;
                SavePlayerData(playerData);
                break;
        }
        SavePlayerData(playerData);
        x2CoinBtn.GetComponent<Button>().interactable = false;
        x2CoinBtn.transform.GetChild(1).GetComponent<Button>().interactable = false;
        x2CoinBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Claimed";
        x2CoinBtn.transform.GetChild(2).gameObject.SetActive(false);
        StartCoroutine(MoveMoney(10, GameObject.Find("X2Coin").GetComponent<RectTransform>()));
    }
    public void Ads45S()
    {
        Time.timeScale = 1;
        isStart = true;
        losePanel.SetActive(false);
        playzoneobj.SetActive(true);
        pool.SetActive(true);
        time += 45;
        loseCount++;
    }

    public GameObject coinPrefab;
    private IEnumerator MoveMoney(int count, RectTransform pack)
    {
        Debug.Log("MoveMoney");
        for (int i = 1; i <= count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject coin1 = Instantiate(coinPrefab, GameObject.Find("HUD").transform);
            coin1.GetComponent<RectTransform>().position = pack.position;
            Vector3[] waypoints = new Vector3[3];
            waypoints[0] = pack.position;
            waypoints[2] = GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position;
            waypoints[1] = new Vector3(Random.Range(pack.position.x, GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position.x),
                                        Random.Range(pack.position.y, GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position.y), 0);

            coin1.transform.DOPath(waypoints, 0.5f, PathType.CatmullRom)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() => Destroy(coin1));
        }

    }
} 

public class PlayerData
{
    public enum Challenge
    {
        Easy,
        Medium,
        Hard,
        Level,
        Daily
    }
    public int currentLevel;
    public Challenge challenge;
    public int money;
    public int hint;
    public bool noAds;
    public bool isMusicOn;
    public bool isSoundOn;
    public bool isVibrateOn;
    public int day;
    public int month = 0;
    public bool[] daily;
    public bool hasLevel;
    public bool firstPurchased = false;
    public PlayerData(int currentLevel, Challenge chalenge, int money, int hint, bool noAds, bool isMusicOn, bool isSoundOn, bool isVibrateOn, int day)
    {
        this.currentLevel = currentLevel;
        this.challenge = chalenge;
        this.money = money;
        this.hint = hint;
        this.noAds = noAds;
        this.isMusicOn = isMusicOn;
        this.isSoundOn = isSoundOn;
        this.isVibrateOn = isVibrateOn;
        this.day = day;
        daily = new bool[37];
        for (int i = 0; i < 37; i++)
        {
            daily[i] = false;
        }
    }
}