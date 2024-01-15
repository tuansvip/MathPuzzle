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

    public GameObject camera, menu, level, highScore, levelBtnPrefab, lvContainer;

    bool isChooseLevel;
    bool isChooseHighScore;
    public bool isStart;
    private string savePath;
    PlayerData playerData;




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
            }
            else
            {
                GameObject btn = Instantiate(levelBtnPrefab, lvContainer.transform);
                btn.GetComponent<LevelBtn>().value = i;
                btn.GetComponent<LevelBtn>().isUnlocked = false;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            }
        }
        ResizeParent();
    }
    void ResizeParent()
    {
        lvContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(lvContainer.GetComponent<RectTransform>().sizeDelta.x, 20 + (0.5f*20));
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
            string json = JsonUtility.ToJson(new PlayerData(0, 0, 0, 1, 1));
            File.WriteAllText(savePath, json);
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
    public void LevelSelected()
    {
        SFXManager.instance.PlayClick();
        isStart = true;
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("easy");

    }    


    private IEnumerator StartHard()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("hard");
    }

    private void Update()
    {
        if (isChooseLevel)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(20, 0, -10), 0.1f);
        } else if (isChooseHighScore)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(20, -20, -10), 0.1f);
        }
        if (!isChooseLevel && !isChooseHighScore)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(20, 20, -10), 0.1f);
        }
        if (isStart)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(0, 0, -10), 0.1f);
        }
    }

}
