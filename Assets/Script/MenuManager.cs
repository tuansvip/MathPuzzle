using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ToggleSwitch;
using Path = System.IO.Path;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Panels")]
    public GameObject selectChalengePanel;
    public GameObject homePanel, shopPanel, dailyPanel;
    public GameObject settingsPanel;
    public GameObject settingsPanel_Child;
    public GameObject menuBar;

    [Header("GameObjects")]
    public GameObject canvas;
    public GameObject musicSwitch;
    public GameObject soundSwitch;
    public GameObject vibrationSwitch;


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
    public bool adsDaily = false;
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
        playerData.day = 0;
        if (playerData.month != DateTime.Now.Month)
        {
            playerData.month = DateTime.Now.Month;
            playerData.daily = new bool[40];
            for (int i = 0; i < 40; i++)
            {
                playerData.daily[i] = false;
            }
            for(int i = 0; i < 5; i++)
            {
                playerData.monthGift[i] = false;
            }
            SavePlayerData(playerData);
        }

        
        musicSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isMusicOn;
        soundSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isSoundOn;
        vibrationSwitch.GetComponent<ToggleSwitch>().isOn = playerData.isVibrateOn;
        musicSwitch.GetComponent<ToggleSwitch>().Init();
        soundSwitch.GetComponent<ToggleSwitch>().Init();
        vibrationSwitch.GetComponent<ToggleSwitch>().Init();
        shopPanel.GetComponent<RectTransform>().localPosition = Vector3.left * 10000;
        dailyPanel.GetComponent<RectTransform>().localPosition = Vector3.right * 10000;
        if (playerData.noAds)
        {
            GameObject.Find("Pack1").GetComponent<Button>().interactable = false;
            GameObject.Find("Pack1 Text").GetComponent<Text>().text = "Purchased";
        }
        if (playerData.firstPurchased)
        {
            GameObject.Find("Pack4").GetComponent<Button>().interactable = false;
            GameObject.Find("Pack4 Text").GetComponent<Text>().text = "Purchased";
        }


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
        SetBanner(true);
        ACEPlay.Bridge.BridgeController.instance.ShowBanner();

        
    }

    public void SetBanner(bool check)
    {
        if (check)
        {
            menuBar.GetComponent<RectTransform>().DOMove(menuBar.GetComponent<RectTransform>().position + Vector3.up * 200, 0.25f)
                .OnComplete(() => {
                    MenuBarBtn.instance.yOrigin = MenuBarBtn.instance.homeBtn.transform.position.y;
                    MenuBarBtn.instance.SelectHome();
                });
        }
        else
        {
            MenuBarBtn.instance.yOrigin = MenuBarBtn.instance.homeBtn.transform.position.y;
            MenuBarBtn.instance.SelectHome();
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
                    SceneManager.LoadScene("challenge");
                }

            break;
            case 2:
                if (adsDaily)
                {
                    UnityEvent eReward = new UnityEvent();
                    eReward.AddListener(() =>
                    {
                        // luồng game sau khi tắt quảng cáo ( tặng thưởng cho user )
                        if (playerData.day <= 0 || playerData.daily[playerData.day]) return;
                        playerData.challenge = PlayerData.Challenge.Daily;
                        SavePlayerData(playerData);
                        SceneManager.LoadScene("challenge");
                    });
                    ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
                }
                else
                {
                    if (playerData.day <= 0 || playerData.daily[playerData.day]) return;
                    playerData.challenge = PlayerData.Challenge.Daily;
                    SavePlayerData(playerData);
                    SceneManager.LoadScene("challenge");
                }
                break;
            case 3:
                playerData.challenge = PlayerData.Challenge.Easy;
                SavePlayerData(playerData);
                SceneManager.LoadScene("challenge");
                break;
            case 4:
                playerData.challenge = PlayerData.Challenge.Medium;
                SavePlayerData(playerData);
                SceneManager.LoadScene("challenge");
                break;
            case 5:
                playerData.challenge = PlayerData.Challenge.Hard;
                SavePlayerData(playerData);
                SceneManager.LoadScene("challenge");
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
        playerData.isVibrateOn = vibrationSwitch.GetComponent<ToggleSwitch>().isOn;
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
                btnLeft.interactable = false;
                break;
            case 5:
                selectedLevel = 4;
                btnRight.interactable = true;
                break;
            default: break;
        }
    }
    public void BtnRightClick()
    {
        switch(selectedLevel){
            case 3:
                selectedLevel = 4;
                btnLeft.interactable = true;
                break;
            case 4:
                selectedLevel = 5;
                btnRight.interactable = false;
                break;
            default: break;
        }
    }

    public void PackNoAds()
    {
        ACEPlay.Bridge.BridgeController.instance.CanShowAds = false;
        playerData.noAds = true;
        SavePlayerData(playerData);
        if (playerData.noAds)
        {
            GameObject.Find("Pack1").GetComponent<Button>().interactable = false;
            GameObject.Find("Pack1 Text").GetComponent<Text>().text = "Purchased";
        }
        if (playerData.firstPurchased)
        {
            GameObject.Find("Pack4").GetComponent<Button>().interactable = false;
            GameObject.Find("Pack4 Text").GetComponent<Text>().text = "Purchased";
        }
    }
    public void Pack100()
    {


        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            StartCoroutine(MoneyIncrease(100));
            SavePlayerData(playerData);
            StartCoroutine(MoveMoney(5, GameObject.Find("Pack2").GetComponent<RectTransform>()));
        });
        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct("100coins", e);

    }
    public void Pack350()
    {


        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua

            StartCoroutine(MoneyIncrease(350));
            SavePlayerData(playerData);
            StartCoroutine(MoveMoney(15, GameObject.Find("Pack3").GetComponent<RectTransform>()));
        });
        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct("350coins", e);
    }
    public void PackSale()
    {


        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua
            StartCoroutine(MoneyIncrease(125));
            playerData.hint += 3;
            playerData.firstPurchased = true;
            SavePlayerData(playerData);

            StartCoroutine(MoveMoney(10, GameObject.Find("Pack4").GetComponent<RectTransform>()));
            StartCoroutine(MoveHint(3, GameObject.Find("Pack4").GetComponent<RectTransform>()));
        });
        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct("packSale", e);
        if (playerData.firstPurchased)
        {
            GameObject.Find("Pack4").GetComponent<Button>().interactable = false;
            GameObject.Find("Pack4 Text").GetComponent<Text>().text = "Purchased";
        }
    }

    public IEnumerator MoveHint(int count, RectTransform pack)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject coin1 = Instantiate(hintPrefab, canvas.transform);
            coin1.GetComponent<RectTransform>().position = pack.position;
            Vector3[] waypoints = new Vector3[3];
            waypoints[0] = pack.position;
            waypoints[2] = GameObject.Find("MoneyImg").GetComponent<RectTransform>().position;
            waypoints[1] = new Vector3(Random.Range(pack.position.x, GameObject.Find("MoneyImg").GetComponent<RectTransform>().position.x),
                                        Random.Range(pack.position.y, GameObject.Find("MoneyImg").GetComponent<RectTransform>().position.y), 0);

            coin1.transform.DOPath(waypoints, 0.5f, PathType.CatmullRom)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() => Destroy(coin1));
        }
    }

    public IEnumerator MoveMoney(int count, RectTransform pack)
    {
        for (int i=1; i <= count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject hint = Instantiate(coinPrefab, canvas.transform);
            hint.GetComponent<RectTransform>().position = pack.position;
            Vector3[] waypoints = new Vector3[3];
            waypoints[0] = pack.position;
            waypoints[2] = GameObject.Find("MoneyImg").GetComponent<RectTransform>().position;
            waypoints[1] = new Vector3(Random.Range(pack.position.x, GameObject.Find("MoneyImg").GetComponent<RectTransform>().position.x),
                                        Random.Range(pack.position.y, GameObject.Find("MoneyImg").GetComponent<RectTransform>().position.y), 0);

            hint.transform.DOPath(waypoints, 0.5f, PathType.CatmullRom)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() => Destroy(hint));
        }

    }
    public GameObject coinPrefab, hintPrefab;

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
    public void MoreGame()
    {
        ACEPlay.Bridge.BridgeController.instance.Moregames();
    }
    public void RestorePurchase()
    {
        ACEPlay.Bridge.BridgeController.instance.RestorePurchase();
    }
    public void Rate()
    {
        ACEPlay.Bridge.BridgeController.instance.RateGame();
    }
}
