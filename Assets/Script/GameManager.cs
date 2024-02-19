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
    List<int> hardLevel = new List<int> { 14, 20, 26, 37, 43, 49, 56, 58, 66, 73, 88, 111, 119, 125, 129, 134, 137, 145, 150, 166, 179, 191, 195, 214, 219, 227, 236, 239, 244, 253, 257, 271, 279, 286, 290, 295, 302, 305, 311, 316, 324, 341, 353, 359, 370, 393, 400, 405, 410, 415, 426, 431, 438, 444, 447, 455, 469, 473, 488, 493, 502, 505, 516, 525, 533, 539, 544, 550, 556, 558, 576, 581, 588, 589, 595, 611, 615, 643, 651, 659, 670, 687, 688, 693, 696, 701, 707, 715, 741, 746, 751, 755, 761, 766, 774, 784, 790, 801, 805, 825, 829, 835, 841, 845, 855, 862, 866, 871, 880, 887, 895, 904, 915, 920, 930, 936, 942, 944, 949, 958, 975, 980, 985, 995, 1000, 1010, 1029, 1040, 1045, 1049, 1063, 1068, 1090, 1097, 1100, 1105, 1119, 1128, 1145, 1149, 1158, 1166, 1172, 1178, 1183, 1191, 1203, 1205, 1210, 1214, 1218, 1248, 1253, 1262, 1279, 1291, 1297, 1298, 1305, 1316, 1320, 1322, 1326, 1333, 1338, 1346, 1359, 1369, 1370, 1381, 1385, 1392, 1393, 1410, 1413, 1431, 1434, 1445, 1456, 1460, 1469, 1478, 1486, 1499, 1512, 1521, 1522, 1530, 1537, 1550, 1561, 1569, 1575, 1581, 1589, 1595, 1600, 1608, 1623, 1625, 1630, 1635, 1641, 1644, 1647, 1653, 1656, 1667, 1677, 1695, 1715, 1726, 1728, 1741, 1745, 1751, 1755, 1766, 1767, 1774, 1779, 1791, 1794, 1805, 1814, 1821, 1825, 1845, 1854, 1862, 1877, 1880, 1884, 1887, 1896, 1901, 1915, 1919, 1926, 1929, 1930, 1940, 1942, 1949, 1961, 1971, 1980, 1985, 1993, 2000, 2004 };
    List<int> easyLevel = new List<int> { 1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 27, 28, 29, 30, 31, 38, 39, 40, 44, 45, 50, 51, 52, 53, 59, 60, 61, 62, 67, 68, 69, 74, 75, 76, 77, 78, 79, 80, 89, 90, 91, 92, 93 };

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
    public GameObject spawnParent;
    public GameObject losePanel;
    public GameObject musicToggle;
    public GameObject soundToggle;
    public GameObject nextLevelBtn;
    public Blank selectedBlank = null;
    public int maxLevel = 300;

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
        if (instance == null)
        {
            instance = this;
        }
        isHighscore = false;
        time = 120;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        playerData = LoadPlayerData(); 
        soundToggle.GetComponent<ToggleSwitch>().isOn = playerData.isSoundOn;
        musicToggle.GetComponent<ToggleSwitch>().isOn = playerData.isMusicOn;
        Application.targetFrameRate = 144;
        if (hardLevel.IndexOf(playerData.currentLevel) != -1)
        {
            level = Difficult.Hard;
        } else if(easyLevel.IndexOf(playerData.currentLevel) != -1)
        {
            level = Difficult.Easy;
        } else
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
        time -= Time.deltaTime;
        if (checkWin())
        {
            Victory();
        }
        if (lives <= 0 || time <= 0)
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
        if (playerData.currentLevel == maxLevel)
        {
            nextLevelBtn.SetActive(false);
        }
        if (playerData.currentLevel == playerData.unlockLevel && playerData.currentLevel <= maxLevel)
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
            string json = File.ReadAllText(savePath);
            json = Encryption.DecryptString(encryptKey, json);
            if (JsonUtility.FromJson<PlayerData>(json) == null)
            {
                Debug.LogWarning("Save file incorrect, creating a new one!");
                string json2 = JsonUtility.ToJson(new PlayerData(0, 0, 0, 1, 1, true, true));
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
            string json = JsonUtility.ToJson(new PlayerData(0, 0, 0, 1, 1, true, true));
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

    public void SaveSetting()
    {
        playerData.isSoundOn = soundToggle.GetComponent<ToggleSwitch>().isOn;
        playerData.isMusicOn = musicToggle.GetComponent<ToggleSwitch>().isOn;
        SavePlayerData(playerData);
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
        ans.transform.DOMove(targerTransform.position + Vector3.back, 1f);
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
    public bool isMusicOn;
    public bool isSoundOn;

    public PlayerData(float hard, float medium, float easy, int currentLevel, int unlockLevel, bool isMusicOn, bool isSoundOn)
    {
        this.hard = hard;
        this.medium = medium;
        this.easy = easy;
        this.currentLevel = currentLevel;
        this.unlockLevel = unlockLevel;
        this.isMusicOn = isMusicOn;
        this.isSoundOn = isSoundOn;
    }
}