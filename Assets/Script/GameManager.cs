using DG.Tweening;
using DG.Tweening.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using UnityEngine.Events;
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
    public GameObject vibrationToggle;
    public GameObject x2CoinBtn;
    public GameObject gameplay;
    public GameObject HUD;
    public Blank selectedBlank = null;

    [Header("#Prefabs")]
    public GameObject answerPrefab;
    public GameObject blankPrefab;
    public GameObject numberPrefab;
    public GameObject opPrefab;
    public GameObject _200coinPrefab;


    [Header("#Game Component")]
    public GenerateMath playzone;
    public PlayerData playerData;
    public RenderTexture myRenderTexture;
    public Camera winCamera;
    public ParticleSystem winParticle;
    public Color winColor;

    [Header("#Game Data")]
    public int maxLevel = 2004;
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
        DOTween.Clear();
        DOTween.SetTweensCapacity(2000, 500);
        loseCount = 0;
        if (instance == null)
        {
            instance = this;
        }
        isHighscore = false;
        savePath = Application.persistentDataPath + "/IAMNUPERMAN.json";
        playerData = LoadPlayerData(); 
        soundToggle.GetComponent<ToggleSwitch>().isOn = playerData.isSoundOn;
        musicToggle.GetComponent<ToggleSwitch>().isOn = playerData.isMusicOn;
        vibrationToggle.GetComponent<ToggleSwitch>().isOn = playerData.isVibrateOn;
        soundToggle.GetComponent<ToggleSwitch>().Init();
        musicToggle.GetComponent<ToggleSwitch>().Init();
        vibrationToggle.GetComponent<ToggleSwitch>().Init();
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

        if (SceneManager.GetActiveScene().name == "challenge") 
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

        isStart = true;
        //auto generate level
/*        if (SceneManager.GetActiveScene().name == "challenge" && playerData.challenge == PlayerData.Challenge.Level && playerData.currentLevel < 2005)
        {
            WinGame();
            SceneManager.LoadScene("menu");

        }*/
        //
        if (IsMobile())
        {
            ResizeGameplay();
        }
    }
    private void WinGame()
    {
        isStart = false;
        isPause = true;
        winParticle.Play();
        switch (playerData.challenge)
        {
            case PlayerData.Challenge.Level:
                switch (level)
                {
                    case Difficult.Easy:
                        SavePlayerData(playerData);

                        break;
                    case Difficult.Medium:
                        SavePlayerData(playerData);
                        break;
                    case Difficult.Hard:
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
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Easy:
                PlayerPrefs.SetInt("EasyCount", PlayerPrefs.GetInt("EasyCount") + 1);
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Medium:
                PlayerPrefs.SetInt("NormalCount", PlayerPrefs.GetInt("NormalCount") + 1);
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Hard:
                PlayerPrefs.SetInt("HardCount", PlayerPrefs.GetInt("HardCount") + 1);
                SavePlayerData(playerData);
                break;
        }
        if (playerData.challenge == PlayerData.Challenge.Daily)
        {
            playerData.daily[playerData.day] = true;
        }
        SavePlayerData(playerData);
        winPanel.SetActive(true);
    }
    public float SafeAreaOffset()
    {
        {
            Rect safeArea = Screen.safeArea;

            // Chuyển đổi cạnh trên của safe area từ pixels sang world space
            Vector3 safeAreaTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, safeArea.yMax, Camera.main.nearClipPlane));

            // Chuyển đổi cạnh trên của camera từ viewport space (0-1) sang world space
            Vector3 cameraTopEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));

            // Tính độ lệch giữa cạnh trên của camera và cạnh trên của safe area trong world space
            float topEdgeDifference = safeAreaTopEdge.y - cameraTopEdge.y;

            return topEdgeDifference;

        }
    }
    public bool IsMobile()
    {
        float screenRatio = (float)Screen.height / (float)Screen.width;

        if (screenRatio < 1.77f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void ResizeGameplay()
    {
        float originalTopEdgeY = gameplay.transform.position.y + (gameplay.transform.localScale.y * 0.5f);
        float cameraWidth = Camera.main.aspect / 0.5625f;
        gameplay.transform.localScale = new Vector3(gameplay.transform.localScale.y * cameraWidth, gameplay.transform.localScale.y * cameraWidth, 1);
        float newTopEdgeY = gameplay.transform.position.y + (gameplay.transform.localScale.y * 0.5f);
        float yShift = originalTopEdgeY - newTopEdgeY;
        gameplay.transform.position += new Vector3(0, yShift + SafeAreaOffset(), 0);
        foreach(Answer ans in ansSpawn.GetComponentsInChildren<Answer>())
        {
            ans.startPosition += Vector3.up * yShift + Vector3.up * SafeAreaOffset();
            ans.targetPosition += Vector3.up * yShift + Vector3.up * SafeAreaOffset();
        }
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

        levelModel.ArrayIndex = playzone.indexMath;

        levelModel.ArrayValue = playzone.values;
        Debug.Log("ArrayValue: " + levelModel.ArrayValue.Length);
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
        playzone.indexMath = levelModel.ArrayIndex;
    }
    public void AddState()
    {
        answerPositionHistory.Push(ansSpawn.GetComponentsInChildren<Answer>()
            .Select(x => x.targetPosition).ToList());
    }
    public void UndoState()
    {
        if (answerPositionHistory.Count == 0 || isPause) return;
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
    public void Defeat()
    {
        SFXManager.instance.PlayLose();
        isStart = false;
        foreach (Answer ans in ansSpawn.GetComponentsInChildren<Answer>())
        {
            ans.isDragging = false;
        }
        Time.timeScale = 0;
        playzoneobj.SetActive(false);
        pool.SetActive(false);
        losePanel.SetActive(true);
        if (loseCount > 0)
        {
            GameObject.Find("Buy +60s").GetComponent<Button>()
                .onClick.RemoveListener(Buy60s);
            GameObject.Find("Buy +60s").GetComponent<Button>()
                .onClick.AddListener(Home);
            GameObject.Find("+60s Text").GetComponent<TextMeshProUGUI>().text = "Home";
            GameObject.Find("+45s").GetComponent<Button>()
                .interactable = false;
        }
    }
    public void Victory()
    {
        SFXManager.instance.PlayWin();
        isStart = false;
        isPause = true;
        winParticle.Play();
        StartCoroutine(VictoryEffect());

    }
    private IEnumerator VictoryEffect()
    {
        
        IEnumerable<int> allValues = playzone.indexMath.Cast<int>();
        int maxIndex = allValues.Max();
        Debug.Log("Max index: " + maxIndex);
        for (int i = maxIndex; i > 0; i--)
        {
            for (int j = 0; j < spawn.transform.childCount; j++)
            {
                Debug.Log("Index: " + i);
                switch (spawn.transform.GetChild(j).tag)
                {
                    case "NumberCell":
                        if (spawn.transform.GetChild(j).GetComponent<Number>().index == i)
                        {
                            if (spawn.transform.GetChild(j).GetComponent<Number>().bg!= null)
                            {
                                spawn.transform.GetChild(j).GetComponent<Number>().bg
                                    .DOColor(winColor, 0.3f);
                            }
                            Debug.Log("Found");
                            yield return new WaitForSeconds(0.08f);
                        }
                        break;
                    case "OpCell":
                        if (spawn.transform.GetChild(j).GetComponent<Op>().index == i)
                        {
                            if (spawn.transform.GetChild(j).GetComponent<Op>().bg != null)
                            {
                                spawn.transform.GetChild(j).GetComponent<Op>().bg
                                    .DOColor(winColor, 0.3f);
                            }
                            Debug.Log("Found");

                            yield return new WaitForSeconds(0.08f);
                        }

                        break;
                    case "BlankCell":

                        if (spawn.transform.GetChild(j).GetComponent<Blank>().index == i)
                        {
                            if (spawn.transform.GetChild(j).GetComponent<Blank>().bg != null)
                            {
                                spawn.transform.GetChild(j).GetComponent<Blank>().bg
                                    .DOColor(winColor, 0.3f);
                            }
                            Debug.Log("Found");

                            yield return new WaitForSeconds(0.08f);
                        }
                        break;
                }

            }

        }
        StartCoroutine(ShowPanelWin());
    }
    private IEnumerator ShowPanelWin()
    {
        yield return new WaitForSeconds(0.5f);
        switch (playerData.challenge)
        {
            case PlayerData.Challenge.Level:
                switch (level)
                {
                    case Difficult.Easy:
                        StartCoroutine(MoneyIncrease(5));
                        SavePlayerData(playerData);

                        break;
                    case Difficult.Medium:
                        StartCoroutine(MoneyIncrease(10));
                        SavePlayerData(playerData);
                        break;
                    case Difficult.Hard:
                        StartCoroutine(MoneyIncrease(15));
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
                StartCoroutine(MoneyIncrease(25));
                winPanel.transform.GetChild(1).gameObject.SetActive(true);
                winPanel.transform.GetChild(2).gameObject.SetActive(false);
                winPanel.transform.GetChild(3).gameObject.SetActive(false);
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Easy:
                StartCoroutine(MoneyIncrease(3));
                PlayerPrefs.SetInt("EasyCount", PlayerPrefs.GetInt("EasyCount") + 1);
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Medium:
                StartCoroutine(MoneyIncrease(4));
                PlayerPrefs.SetInt("NormalCount", PlayerPrefs.GetInt("NormalCount") + 1);
                SavePlayerData(playerData);
                break;
            case PlayerData.Challenge.Hard:
                StartCoroutine(MoneyIncrease(7));
                PlayerPrefs.SetInt("HardCount", PlayerPrefs.GetInt("HardCount") + 1);
                SavePlayerData(playerData);
                break;
        }
        if (playerData.challenge == PlayerData.Challenge.Daily)
        {
            playerData.daily[playerData.day] = true;
        }
        SavePlayerData(playerData);
        winPanel.SetActive(true);
        ShowWinImg();
        StartCoroutine(MoveMoney(10, GameObject.Find("CoinWin").GetComponent<RectTransform>()));
    }
    public void ShowWinImg()
    {
        playHUD.SetActive(false);
        pool.transform.GetChild(0).gameObject.SetActive(false);
        HUD.GetComponent<Canvas>().sortingOrder = -20;
        if (gameplay != null)
        {
            gameplay.transform.DOMove(gameplay.transform.position + Vector3.up * 1.3f, 0.7f);
            gameplay.transform.DOScale(gameplay.transform.localScale / 1.5f, 0.7f);
        }

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
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            DOTween.KillAll();
            DOTween.Clear();
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        });
        ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("placement", e);

    }
    public void Restart()
    {
        UnityEvent eReward = new UnityEvent();
        eReward.AddListener(() =>
        {
            DOTween.KillAll();
            DOTween.Clear();
            SFXManager.instance.PlayClick();
            Time.timeScale = 1;
            isHighscore = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
    }
    public void Pause()
    {
        if (isPause) return;
        isPause = true;
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;
        pausePanel.SetActive(true);
        playHUD.SetActive(false);
    }
    public void Resume()
    {
        isPause = false;
        SFXManager.instance.PlayClick();
        Time.timeScale = 1;;
        pausePanel.SetActive(false);
        supportPanel.SetActive(false);
        losePanel.SetActive(false);
        playHUD.SetActive(true);
    }
    public void SaveSetting()
    {
        playerData.isSoundOn = soundToggle.GetComponent<ToggleSwitch>().isOn;
        playerData.isMusicOn = musicToggle.GetComponent<ToggleSwitch>().isOn;
        playerData.isVibrateOn = vibrationToggle.GetComponent<ToggleSwitch>().isOn;
        SavePlayerData(playerData);
    }
    public void Hint()
    {
        if (isPause) return;
        if (playerData.hint > 0 && !hintMoving)
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
            ans.GetComponent<Answer>().isHinting = true;
            if (ans.transform != null)
            {
                ans.transform.DOMove(targerTransform.position + Vector3.back * 2, 1f);
            }
            StartCoroutine(HintMoveBack(ans, originalTransform));
            playerData.hint--;

        }
        else
        {
            if (playerData.hint < 1)
            {
                isPause = true;
                SFXManager.instance.PlayClick();
                Time.timeScale = 1;
                supportPanel.SetActive(true);
            }
        }

        SavePlayerData(playerData);
    }
    private IEnumerator HintMoveBack(GameObject ans, Transform originalTransform)
    {
        yield return new WaitForSeconds(1f);
        if ( ans.transform != null)
        {        
            ans.transform.DOMove(originalTransform.position, 1f)
                .OnComplete(() => {
                hintMoving = false;
                ans.GetComponent<Answer>().isHinting = false;});
        }
    }
    public void HintAds()
    {
        UnityEvent eReward = new UnityEvent();
        eReward.AddListener(() =>
        {
            Resume();
            playerData.hint++;
            SavePlayerData(playerData);
        });
        ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);

    }
    public void BuySupport()
    {
        if (playerData.money > 400)
        {
            Time.timeScale = 1;
            isStart = true;
            isPause = false;
            losePanel.SetActive(false);
            playzoneobj.SetActive(true);
            pool.SetActive(true);
            time += 60;
            MoneyDecrease(400);
            loseCount++;
            playerData.hint++;
            SavePlayerData(playerData);
            supportPanel.SetActive(false);
        }
        else
        {
            if (checkBuy) return;
            checkBuy = true;
            outOfMoneyNoti.SetActive(true);
            if (outOfMoneyNoti.GetComponent<RectTransform>().position.y == 0)
            {
                outOfMoneyNoti.GetComponent<RectTransform>()
                    .DOMove(Vector3.down * 2, 1f)
                    .SetDelay(0.5f).OnComplete(() => {
                    outOfMoneyNoti.GetComponent<RectTransform>().position = new Vector3(0, 0, 105.010f);
                    outOfMoneyNoti.SetActive(false);
                    checkBuy = false;
                }); 
            }
        }
    }
    public void Buy60s()
    {
        if (playerData.money >= 150)
        {
            checkBuy = true;
            Time.timeScale = 1;
            isStart = true;
            isPause = false;
            StartCoroutine(MoneyDecrease(150));
            losePanel.SetActive(false);
            playzoneobj.SetActive(true);
            pool.SetActive(true);
            time += 60;
            loseCount++;
        }
        else
        {
            if (checkBuy) return;
            checkBuy = true;
            outOfMoneyNoti.SetActive(true);
            if (outOfMoneyNoti.GetComponent<RectTransform>() != null)
            {
                outOfMoneyNoti.GetComponent<RectTransform>()
                    .DOMove(Vector3.down * 2, 1f)
                    .SetDelay(0.5f)
                    .OnComplete(() => {
                    outOfMoneyNoti.GetComponent<RectTransform>().position = new Vector3(0, 0, 105.010f);
                    outOfMoneyNoti.SetActive(false);
                    checkBuy = false;
                });
            }
        }
    }
    public bool checkBuy = false;
    public GameObject outOfMoneyNoti;
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
        SFXManager.instance.PlayClick();
        UnityEvent eReward = new UnityEvent();
        eReward.AddListener(() =>
        {
            // luồng game sau khi tắt quảng cáo ( tặng thưởng cho user )
            switch (playerData.challenge)
            {
                case PlayerData.Challenge.Level:
                    switch (level)
                    {
                        case Difficult.Easy:
                            StartCoroutine(MoneyIncrease(5));    
                            SavePlayerData(playerData);

                            break;
                        case Difficult.Medium:
                            StartCoroutine(MoneyIncrease(10));
                            SavePlayerData(playerData);
                            break;
                        case Difficult.Hard:
                            StartCoroutine(MoneyIncrease(15));
                            SavePlayerData(playerData);
                            break;
                    }
                    break;
                case PlayerData.Challenge.Daily:
                    StartCoroutine(MoneyIncrease(25));
                    SavePlayerData(playerData);
                    break;
                case PlayerData.Challenge.Easy:
                    StartCoroutine(MoneyIncrease(3));
                    SavePlayerData(playerData);
                    break;
                case PlayerData.Challenge.Medium:
                    StartCoroutine(MoneyIncrease(4));
                    SavePlayerData(playerData);
                    break;
                case PlayerData.Challenge.Hard:
                    StartCoroutine(MoneyIncrease(7));
                    SavePlayerData(playerData);
                    break;
            }
            SavePlayerData(playerData);
            x2CoinBtn.GetComponent<Button>().interactable = false;
            x2CoinBtn.transform.GetChild(1).GetComponent<Button>().interactable = false;
            x2CoinBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Claimed";
            x2CoinBtn.transform.GetChild(2).gameObject.SetActive(false);
            StartCoroutine(MoveMoney(10, GameObject.Find("X2Coin").GetComponent<RectTransform>()));
        });
        ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
    }
    public void Ads45S()
    {
        UnityEvent eReward = new UnityEvent();
        eReward.AddListener(() =>
        {
            // luồng game sau khi tắt quảng cáo ( tặng thưởng cho user )
            Time.timeScale = 1;
            isStart = true;
            isPause = false;
            losePanel.SetActive(false);
            playzoneobj.SetActive(true);
            pool.SetActive(true);
            time += 45;
            loseCount++;
        });
        ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);

    }
    public GameObject coinPrefab;
    private IEnumerator MoveMoney(int count, RectTransform pack)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject coin1 = Instantiate(coinPrefab, GameObject.Find("MoneyMoveParent").transform);
            coin1.GetComponent<RectTransform>().position = pack.position;
            Vector3[] waypoints = new Vector3[3];
            waypoints[0] = pack.position;
            waypoints[2] = GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position;
            waypoints[1] = new Vector3(Random.Range(pack.position.x, GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position.x),
                                        Random.Range(pack.position.y, GameObject.Find("MoneyImg_Win").GetComponent<RectTransform>().position.y), 0);
            if (coin1.transform != null)
            {
                coin1.transform.DOPath(waypoints, 0.5f, PathType.CatmullRom)
                         .SetEase(Ease.OutQuad)
                         .OnComplete(() => Destroy(coin1));
            }
        }

    }
    public IEnumerator MoneyIncrease(int value)
    {
        float delay = 0.1f;
        while (value > 0)
        {
            playerData.money++;
            value--;
            delay *= 0.2f;
            yield return new WaitForSeconds(delay);
        }
        SavePlayerData(playerData);
    }    
    public IEnumerator MoneyDecrease(int value)
    {
        float delay = 0.1f;
        while (value > 0)
        {
            playerData.money--;
            value--;
            delay *= 0.2f;
            yield return new WaitForSeconds(delay);
        }
        SavePlayerData(playerData);
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
    public bool[] monthGift;
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
        daily = new bool[40];
        for (int i = 0; i < 40; i++)
        {
            daily[i] = false;
        }
        monthGift = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            monthGift[i] = false;
        }
    }
}