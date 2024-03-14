using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class GenerateMath : MonoBehaviour
{
    public enum Directions
    {
        up,
        down,
        left,
        right
    }
    public GameObject opPrefab, numberPrefab, blankPrefab, answerPrefab;
    public GameObject[,] grid;
    public List<GameObject> cellAnswer;
    List<GameObject> cellBlank;
    public string[,] gridModel;
    public int[] posX = new int[5];
    public int[] posY = new int[5];
    public Directions dir;
    public Transform startPoint;
    public Transform startAnsPoint;
    public Transform spawn;
    public Transform spawnAns;
    public Transform spawnParent;
    public int size;
    public int maxRange;
    public int maxValue;
    public int[,] values;
    bool up = true, down = true, left = true, right = true;
    bool canChangeDir = true;
    int maxHide;
    float ratiocam;

    public void Generate(GameManager.Difficult level)
    {
        switch (GameManager.instance.playerData.challenge)
        {
            case PlayerData.Challenge.Easy:
                size = 8;
                maxRange = 200;
                maxHide = 2;
                break;
            case PlayerData.Challenge.Medium:
                size = 14;
                maxRange = 500;
                maxHide = 3;
                break;
            case PlayerData.Challenge.Hard:
                size = 14;
                maxRange = 500;
                maxHide = 3;
                break;
            case PlayerData.Challenge.Level:
                {
                    switch (level)
                    {
                        case GameManager.Difficult.Easy:
                            if (GameManager.instance.playerData.currentLevel < 50)
                            {
                                size = 5;
                                maxRange = 20 + GameManager.instance.playerData.currentLevel;
                                if (maxRange > 500) maxRange = 500;
                                maxValue = 75 + GameManager.instance.playerData.currentLevel;
                                maxHide = 2;

                            } else
                            {
                                size = 8;
                                maxRange = 20 + GameManager.instance.playerData.currentLevel * 2;
                                if (maxRange > 500) maxRange = 500;
                                maxValue = 200 + GameManager.instance.playerData.currentLevel * 2;
                                maxHide = 2; 
                            }
                            break;
                        case GameManager.Difficult.Medium:
                            if (GameManager.instance.playerData.currentLevel < 50)
                            {
                                size = 10;
                                maxRange = 20 + GameManager.instance.playerData.currentLevel * 2;
                                maxValue = 75 + GameManager.instance.playerData.currentLevel * 2;
                                if (maxRange > 500) maxRange = 500;
                                maxHide = 3;
                            } else
                            {
                                size = 14;
                                maxRange = 20 + GameManager.instance.playerData.currentLevel * 2;
                                maxValue = 200 + GameManager.instance.playerData.currentLevel * 2;
                                if (maxRange > 500) maxRange = 500;
                                maxHide = 3;
                            }
                            break;
                        case GameManager.Difficult.Hard:
                            if (GameManager.instance.playerData.currentLevel < 50)
                            {
                                size = 10;
                                maxRange = 30 + GameManager.instance.playerData.currentLevel * 2;
                                maxValue = 75 + GameManager.instance.playerData.currentLevel * 2;
                                if (maxRange > 500) maxRange = 500;
                                maxHide = 3;
                            } else
                            {
                                size = 14;
                                maxRange = 30 + GameManager.instance.playerData.currentLevel * 2;
                                maxValue = 200 + GameManager.instance.playerData.currentLevel * 2;
                                if (maxRange > 500) maxRange = 500;
                                maxHide = 3;
                            }

                            break;
                    }
                }
                break;
            case PlayerData.Challenge.Daily:
                size = 14;
                maxRange = 500;
                maxHide = 3;

                break;
        }
        cellAnswer = new List<GameObject>();
        cellBlank = new List<GameObject>();
        dir = Directions.right;
        grid = new GameObject[size, size];
        up = true; down = true; left = true; right = true;
        bool canPlay = false;
        if (SceneManager.GetActiveScene().name == "sample")
        {
            gridModel = new string[size, size];
            values = new int[size, size];
            GenerateModel();
            while (!canPlay)
            {
                switch (GameManager.instance.playerData.challenge)
                {
                    case PlayerData.Challenge.Easy:
                        if (NumberOfBlank() < 2 || NumberOfBlank() > 5)
                        {
                            cellAnswer = new List<GameObject>();
                            cellBlank = new List<GameObject>();
                            dir = Directions.right;
                            grid = new GameObject[size, size];
                            gridModel = new string[size, size];
                            values = new int[size, size];
                            up = true; down = true; left = true; right = true;
                            GenerateModel();
                        }
                        else
                        {
                            canPlay = true;
                        }
                        break;
                    case PlayerData.Challenge.Medium:
                        if (NumberOfBlank() < 6 || NumberOfBlank() > 9)
                        {
                            cellAnswer = new List<GameObject>();
                            cellBlank = new List<GameObject>();
                            dir = Directions.right;
                            grid = new GameObject[size, size];
                            gridModel = new string[size, size];
                            values = new int[size, size];
                            up = true; down = true; left = true; right = true;
                            GenerateModel();
                        }
                        else
                        {
                            canPlay = true;
                        }
                        break;
                    case PlayerData.Challenge.Hard:
                        if (NumberOfBlank() < 10)
                        {
                            cellAnswer = new List<GameObject>();
                            cellBlank = new List<GameObject>();
                            dir = Directions.right;
                            grid = new GameObject[size, size];
                            gridModel = new string[size, size];
                            values = new int[size, size];
                            up = true; down = true; left = true; right = true;
                            GenerateModel();
                        }
                        else
                        {
                            canPlay = true;
                        }
                        break;
                    case PlayerData.Challenge.Level:
                        {
                            switch (level)
                            {
                                case GameManager.Difficult.Easy:
                                    if (GameManager.instance.playerData.currentLevel < 50)
                                    {
                                        if (NumberOfBlank() < 2 || NumberOfBlank() > 4)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    else
                                    {
                                        if (NumberOfBlank() < 3 || NumberOfBlank() > 5)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    break;
                                case GameManager.Difficult.Medium:
                                    if (GameManager.instance.playerData.currentLevel < 50)
                                    {
                                        if (NumberOfBlank() < 5 || NumberOfBlank() > 7)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    else
                                    {
                                        if (NumberOfBlank() < 6 || NumberOfBlank() > 9)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    break;
                                case GameManager.Difficult.Hard:
                                    if (GameManager.instance.playerData.currentLevel < 50)
                                    {
                                        if (NumberOfBlank() < 8)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    else
                                    {
                                        if (NumberOfBlank() < 10)
                                        {
                                            cellAnswer = new List<GameObject>();
                                            cellBlank = new List<GameObject>();
                                            dir = Directions.right;
                                            grid = new GameObject[size, size];
                                            gridModel = new string[size, size];
                                            values = new int[size, size];
                                            up = true; down = true; left = true; right = true;
                                            GenerateModel();
                                        }
                                        else
                                        {
                                            canPlay = true;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case PlayerData.Challenge.Daily:
                        if (NumberOfBlank() < 12)
                        {
                            cellAnswer = new List<GameObject>();
                            cellBlank = new List<GameObject>();
                            dir = Directions.right;
                            grid = new GameObject[size, size];
                            gridModel = new string[size, size];
                            values = new int[size, size];
                            up = true; down = true; left = true; right = true;
                            GenerateModel();
                        }
                        else
                        {
                            canPlay = true;
                        }
                        break;
                }
            }

        }
        ratiocam = Camera.main.orthographicSize * 2 * Camera.main.aspect / 10;
        GenerateGrid();
        SuffleAnswers();
        if (GameManager.instance.IsMobile())
        {
            GameManager.instance.ResizeGameplay();
        }
    }

    int NumberOfBlank()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (gridModel[i, j] != "blank")
                {
                    continue;
                }
                count++;
            }
        }
        Debug.Log("Blank: " + count);
        return count;
    }
    public static IList<T> Swap<T>(IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        return list;
    }
    public void SuffleAnswers()
    {
        for (int i = 0; i < cellAnswer.Count; i++)
        {
            int ran = Random.Range(0, cellAnswer.Count);
            if (!cellAnswer[i].GetComponent<Answer>().isOnBlank && !cellAnswer[ran].GetComponent<Answer>().isOnBlank)
            {
                Vector3 tempFs = cellAnswer[i].GetComponent<Answer>().startPosition;
                Vector3 tempTarget = cellAnswer[i].GetComponent<Answer>().targetPosition;
                cellAnswer[i].GetComponent<Answer>().startPosition = cellAnswer[ran].GetComponent<Answer>().startPosition;
                cellAnswer[i].GetComponent<Answer>().targetPosition = cellAnswer[ran].GetComponent<Answer>().targetPosition;
                cellAnswer[ran].GetComponent<Answer>().startPosition = tempFs;
                cellAnswer[ran].GetComponent<Answer>().targetPosition = tempTarget;
                Swap(cellAnswer, i, ran);
            }
        }
    }
    public void SortAnswers()
    {
        for (int i = 0; i < cellAnswer.Count - 1; i++)
        {
            for (int j = i; j < cellAnswer.Count; j++)
            {
                if (cellAnswer[i].GetComponent<Number>().value > cellAnswer[j].GetComponent<Number>().value && !cellAnswer[i].GetComponent<Answer>().isOnBlank && !cellAnswer[j].GetComponent<Answer>().isOnBlank)
                {
                    Vector3 tempFs = cellAnswer[i].GetComponent<Answer>().startPosition;
                    Vector3 tempTarget = cellAnswer[i].GetComponent<Answer>().targetPosition;
                    cellAnswer[i].GetComponent<Answer>().startPosition = cellAnswer[j].GetComponent<Answer>().startPosition;
                    cellAnswer[i].GetComponent<Answer>().SetTarget(cellAnswer[j].GetComponent<Answer>().targetPosition);
                    cellAnswer[j].GetComponent<Answer>().startPosition = tempFs;
                    cellAnswer[j].GetComponent<Answer>().SetTarget(tempTarget);               
                    Swap(cellAnswer, i, j);
                }
                
            }           
        }
        bool check = true;
        for (int i = 0; i < cellAnswer.Count - 1; i++)
        {
            for (int j = i; j < cellAnswer.Count; j++)
            {
                if (cellAnswer[i].GetComponent<Number>().value > cellAnswer[j].GetComponent<Number>().value && !cellAnswer[i].GetComponent<Answer>().isOnBlank && !cellAnswer[j].GetComponent<Answer>().isOnBlank)
                {
                    check = false;
                }

            }
        }
        if (!check)
        {
            SortAnswers();
        }
    }
    public void GenerateGrid()
    {
        int lineCount = 8/size;
        Vector3 center = Vector3.zero;
        float maxX = float.MinValue, minX = float.MaxValue, maxY = float.MinValue, minY = float.MaxValue;
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector3 spawnPoint = new Vector3(i  + startPoint.position.x, j + startPoint.position.y, 0);
                switch (gridModel[i, j])
                {
                    case "+":
                        grid[i,j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity, spawn);
                        grid[i, j].GetComponent<Op>().op = Op.Operations.plus;
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/                        break;
                    case "-":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity, spawn);
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/
                        grid[i, j].GetComponent<Op>().op = Op.Operations.minus;
                        break;
                    case "*":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity, spawn);
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/
                        grid[i, j].GetComponent<Op>().op = Op.Operations.multiply;
                        break;
                    case "/":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity, spawn);
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/
                        grid[i, j].GetComponent<Op>().op = Op.Operations.divide;
                        break;
                    case "=":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity, spawn);
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/
                        grid[i, j].GetComponent<Op>().op = Op.Operations.equal;
                        break;
                    case "empty":
                        continue;
                    case "blank":
                        Debug.Log(blankPrefab);
                        Debug.Log(spawnPoint);
                        Debug.Log(spawn);
                        grid[i, j] = Instantiate(blankPrefab, spawnPoint, Quaternion.identity, spawn);
                        grid[i, j].GetComponent<Blank>().value = values[i, j];
                        grid[i, j].GetComponent<Blank>().x = i;
                        grid[i, j].GetComponent<Blank>().y = j;
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/                        cellBlank.Add(grid[i, j]);
                        

                        Vector3 ansPoint = new Vector3(startAnsPoint.position.x + (count % 7) * 1.1f, startAnsPoint.position.y - (count / 7) * 1.1f);
                        GameObject cell = Instantiate(answerPrefab, ansPoint, Quaternion.identity, spawnAns);
                        cell.transform.localScale = Vector3.one;
                        cell.GetComponent<Number>().value = values[i, j];
                        cellAnswer.Add(cell);
                        count++;
                        break;
                    default:
                        grid[i, j] = Instantiate(numberPrefab, spawnPoint, Quaternion.identity, spawn);
                        grid[i, j].transform.SetParent(spawn);
                        grid[i, j].GetComponent<Number>().value = values[i, j];
/*                        grid[i, j].transform.localScale = Vector3.one * (8f / ((float)size));
*/
                        break;
                }
                if (grid[i,j].transform.position.x > maxX)
                {
                    maxX = grid[i, j].transform.position.x;
                }
                if (grid[i,j].transform.position.y > maxY)
                {
                    maxY = grid[i, j].transform.position.y;
                }
                if (grid[i,j].transform.position.x < minX)
                {
                    minX = grid[i, j].transform.position.x;
                }
                if (grid[i,j].transform.position.y < minY)
                {
                    minY = grid[i, j].transform.position.y;
                }
            }
        }
        center = new Vector3((maxX + minX)  / 2f, (maxY + minY) / 2f, 0);
        
        spawn.position += transform.position -  center;
        if (maxX - minX <= maxY - minY)
        {
            spawnParent.localScale = Vector3.one * (8f / (maxY - minY + 1));
        }
        else
        {
            spawnParent.localScale = Vector3.one * (8f / (maxX - minX + 1));
        }
    }
    public void GenerateModel()
    {
        Debug.Log("GenerateModel");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                gridModel[i, j] = "empty";
            }
        }
        int x, y;
        do
        {
            x = 0; y = Random.Range(0,size);
        } while (y % 2 != 0);

        //generate model
        do 
        {
            int ranPos, ranDir;
            bool check = false;
            do
            {
                switch (dir)
                {
                    case Directions.up:
                        posX[0] = x; posY[0] = y;
                        posX[1] = x; posY[1] = y + 1;
                        posX[2] = x; posY[2] = y + 2;
                        posX[3] = x; posY[3] = y + 3;
                        posX[4] = x; posY[4] = y + 4;
                        GenerateValue();

                        up = false;
                        down = false;
                        left = true;
                        right = true;
                        ranPos = Random.Range(0, 2);
                        switch (ranPos)
                        {
                            case 0:
                                x = posX[2]; y = posY[2];
                                break;
                            case 1:
                                x = posX[4]; y = posY[4];
                                break;
                        }
                        do
                        {
                            check = false;
                            ranDir = Random.Range(0, 2);
                            
                            switch (ranDir)
                            {
                                case 0:
                                    if (x - 4 < 0 || gridModel[x - 1, y] != "empty" || gridModel[x - 3, y] != "empty" )
                                    {
                                        left = false;
                                        check = false;

                                    }
                                    if (x - 5 >= 0 && gridModel[x - 5, y] != "empty")
                                    {
                                        left = false;
                                        check = false;
                                    }
                                    if (x + 1 < size && gridModel[x + 1, y] != "empty")
                                    {
                                        left = false;
                                        check = false;
                                    }
                                    if (left)
                                    {
                                        dir = Directions.left;
                                        up = true;
                                        down = true;
                                        check = true;
                                    }
                                    break;
                                case 1:
                                    if (x + 4 >= size || gridModel[x + 1, y] != "empty" || gridModel[x + 3, y] != "empty")
                                    {
                                        right = false;
                                        check = false;

                                    }
                                    if (x + 5 < size && gridModel[x + 5, y] != "empty")
                                    {
                                        right = false;
                                        check = false;
                                    }
                                    if (x - 1 >= 0 && gridModel[x - 1, y] != "empty")
                                    {
                                        right = false;
                                        check = false;
                                    }
                                    if (right)
                                    {
                                        dir = Directions.right;
                                        up = true;
                                        down = true;
                                        check = true;
                                    }
                                    break;
                            }
                            if (!left && !right && !up && !down)
                            {
                                canChangeDir = false;
                            }
                            if (check)
                            {
                                canChangeDir = true;
                                break;
                            }
                        } while (canChangeDir);
                        break;
                    case Directions.down:
                        posX[0] = x; posY[0] = y;
                        posX[1] = x; posY[1] = y - 1;
                        posX[2] = x; posY[2] = y - 2;
                        posX[3] = x; posY[3] = y - 3;
                        posX[4] = x; posY[4] = y - 4;
                        GenerateValue();
                        up = false;
                        down = false;
                        left = true;
                        right = true;
                        ranPos = Random.Range(0, 2);
                        switch (ranPos)
                        {
                            case 0:
                                x = posX[2]; y = posY[2];
                                break;
                            case 1:
                                x = posX[4]; y = posY[4];
                                break;
                        }
                        do
                        {
                            check = false;

                            ranDir = Random.Range(0, 2);
                            switch (ranDir)
                            {
                                case 0:
                                    if (x - 4 < 0 || gridModel[x - 1, y] != "empty" || gridModel[x - 3, y] != "empty")
                                    {
                                        left = false;
                                        check = false;

                                    }
                                    if (x - 5 >= 0 && gridModel[x - 5, y] != "empty")
                                    {
                                        left = false;
                                        check = false;
                                    }
                                    if (x + 1 < size && gridModel[x + 1, y] != "empty")
                                    {
                                        left = false;
                                        check = false;
                                    }
                                    if (left)
                                    {
                                        dir = Directions.left;
                                        up = true;
                                        down = true;
                                        check = true;
                                    }
                                    break;
                                case 1:
                                    if (x + 4 >= size || gridModel[x + 1, y] != "empty" || gridModel[x + 3, y] != "empty")
                                    {
                                        right = false;
                                        check = false;

                                    }
                                    if (x + 5 < size && gridModel[x + 5, y] != "empty")
                                    {
                                        right = false;
                                        check = false;
                                    }
                                    if (x - 1 >= 0 && gridModel[x - 1, y] != "empty")
                                    {
                                        right = false;
                                        check = false;
                                    }
                                    if (right)
                                    {
                                        dir = Directions.right;
                                        up = true;
                                        down = true;
                                        check = true;
                                    }
                                    break;
                            }
                            if (!left && !right && !up && !down)
                            {
                                canChangeDir = false;
                            }
                            if (check)
                            {
                                canChangeDir = true;
                                break;
                            }
                        } while (canChangeDir);
                        break;
                    case Directions.left:
                        posX[0] = x; posY[0] = y;
                        posX[1] = x - 1; posY[1] = y;
                        posX[2] = x - 2; posY[2] = y;
                        posX[3] = x - 3; posY[3] = y;
                        posX[4] = x - 4; posY[4] = y;
                        GenerateValue();
                        left = false;
                        right = false;
                        up = true;
                        down = true;
                        ranPos = Random.Range(0, 2);
                        switch (ranPos)
                        {
                            case 0:
                                x = posX[2]; y = posY[2];
                                break;
                            case 1:
                                x = posX[4]; y = posY[4];
                                break;
                        }
                        do
                        {
                            check = false;

                            ranDir = Random.Range(0, 2);
                            switch (ranDir)
                            {
                                case 0:
                                    if (y - 4 < 0 || gridModel[x, y - 1] != "empty" || gridModel[x, y - 3] != "empty")
                                    {
                                        down = false;
                                        check = false;

                                    }
                                    if (y - 5 >= 0 && gridModel[x, y - 5] != "empty")
                                    {
                                        down = false;
                                        check = false;
                                    }
                                    if (y + 1 < size && gridModel[x, y + 1] != "empty")
                                    {
                                        down = false;
                                        check = false;
                                    }
                                    if (down)
                                    {
                                        dir = Directions.down;
                                        left = true;
                                        right = true;
                                        check = true;
                                    }
                                    break;
                                case 1:
                                    if (y + 4 >= size || gridModel[x, y + 1] != "empty" || gridModel[x, y + 3] != "empty")
                                    {
                                        check = false;

                                        up = false;
                                    }
                                    if (y + 5 < size && gridModel[x, y + 5] != "empty")
                                    {
                                        up = false;
                                        check = false;
                                    }
                                    if (y - 1 >= 0 && gridModel[x, y - 1] != "empty")
                                    {
                                        up = false;
                                        check = false;
                                    }
                                    if (up)
                                    {
                                        dir = Directions.up;
                                        left = true;
                                        right = true;
                                        check = true;
                                    }
                                    break;
                            }
                            if (!left && !right && !up && !down)
                            {
                                canChangeDir = false;
                            }
                            if (check)
                            {
                                canChangeDir = true;
                                break;
                            }
                        } while (canChangeDir);
                        break;
                    case Directions.right:
                        posX[0] = x; posY[0] = y;
                        posX[1] = x + 1; posY[1] = y;
                        posX[2] = x + 2; posY[2] = y;
                        posX[3] = x + 3; posY[3] = y;
                        posX[4] = x + 4; posY[4] = y;
                        GenerateValue();
                        left = false;
                        right = false;
                        up = true;
                        down = true;
                        ranPos = Random.Range(0, 2);
                        switch (ranPos)
                        {
                            case 0:
                                x = posX[2]; y = posY[2];
                                break;
                            case 1:
                                x = posX[4]; y = posY[4];
                                break;
                        }
                        do
                        {
                            check = false;
                            ranDir = Random.Range(0, 2);
                            switch (ranDir)
                            {
                                case 0:
                                    if (y - 4 < 0 || gridModel[x, y -1] != "empty" || gridModel[x, y - 3] != "empty")
                                    {
                                        down = false;
                                        check = false;
                                    }
                                    if (y - 5 >= 0 && gridModel[x, y - 5] != "empty")
                                    {
                                        down = false;
                                        check = false;
                                    }
                                    if (y + 1 < size && gridModel[x, y + 1] != "empty")
                                    {
                                        down = false;
                                        check = false;
                                    }
                                    if (down)
                                    {
                                        dir = Directions.down;
                                        left = true;
                                        right = true;
                                        check = true;
                                    }
                                    break;
                                case 1:
                                    if (y + 4 >= size || gridModel[x, y + 1] != "empty" || gridModel[x, y + 3] != "empty")
                                    {
                                        up = false;
                                        check = false;

                                    }
                                    if (y + 5 < size - 1 && gridModel[x, y + 5] != "empty")
                                    {
                                        up = false;
                                        check = false;
                                    }
                                    if (y - 1 >= 0 && gridModel[x, y - 1] != "empty")
                                    {
                                        up = false;
                                        check = false;
                                    }
                                    if (up)
                                    {
                                        dir = Directions.up;
                                        left = true;
                                        right = true;
                                        check = true;
                                    }
                                    break;
                            }
                            if (!left && !right && !up && !down)
                            {
                                canChangeDir = false;
                            }
                            if (check)
                            {
                                canChangeDir = true;
                                break;
                            }

                        } while (canChangeDir);
                        break;
                }
            } while(canChangeDir);
        } while (canChangeDir);
    }
    public void GenerateValue()
    {

        if (gridModel[posX[0], posY[0]] != "empty" && gridModel[posX[2], posY[2]] != "empty" && gridModel[posX[4], posY[4]] != "empty")
        {
            return;
        }

        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[4], posY[4]] == "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                values[posX[0], posY[0]] = Random.Range(1, maxRange);
                values[posX[2], posY[2]] = Random.Range(1, maxRange);
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        values[posX[4], posY[4]] = (values[posX[0], posY[0]] + values[posX[2], posY[2]]);
                        break;
                    case "-":
                        if ((values[posX[0], posY[0]] - values[posX[2], posY[2]]) < 0) continue;
                        values[posX[4], posY[4]] = (values[posX[0], posY[0]] - values[posX[2], posY[2]]);
                        break;
                    case "*":
                        if (values[posX[0], posY[0]] * values[posX[2], posY[2]] > maxValue) continue;
                        values[posX[4], posY[4]] = (values[posX[0], posY[0]] * values[posX[2], posY[2]]);
                        break;
                    case "/":
                        if (values[posX[0], posY[0]] == 1 || values[posX[2], posY[2]] == 1) continue;
                        if (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0 && values[posX[2], posY[2]] != 0)
                        {
                            values[posX[4], posY[4]] = (values[posX[0], posY[0]] + values[posX[2], posY[2]]);
                            gridModel[posX[1], posY[1]] = "+";
                        }
                        else
                        {
                            values[posX[4], posY[4]] = values[posX[0], posY[0]] / values[posX[2], posY[2]];
                        }
                        break;
                }
                gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                int isBlank = Random.Range(0, maxHide);

                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
                isdone = true;
            } while (!isdone);
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[4], posY[4]] != "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        values[posX[2], posY[2]] = Random.Range(1, maxRange);
                        if (values[posX[4], posY[4]] - values[posX[2], posY[2]] < 0) continue;
                        values[posX[0], posY[0]] = (values[posX[4], posY[4]] - values[posX[2], posY[2]]);
                        break;
                    case "-":
                        values[posX[2], posY[2]] = Random.Range(1, maxRange);
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] + values[posX[2], posY[2]];
                        break;
                    case "*":
                        if (values[posX[4], posY[4]] == 1 || IsPrime(values[posX[4], posY[4]])) continue;
                        do
                        {
                            values[posX[2], posY[2]] = Random.Range(2, maxRange);
                        } while (values[posX[4], posY[4]] % values[posX[2], posY[2]] != 0);
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] / values[posX[2], posY[2]];
                        break;
                    case "/":
                        values[posX[2], posY[2]] = Random.Range(2, 6);
                       
                        if (values[posX[4], posY[4]] * values[posX[2], posY[2]] > maxValue) continue;
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] * values[posX[2], posY[2]];
                        break;
                }
                if (gridModel[posX[0], posY[0]] == "empty")
                {
                    gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                }
                if (gridModel[posX[2], posY[2]] == "empty")
                {
                    gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                }
                if (gridModel[posX[4], posY[4]] == "empty")
                {
                    gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                }
                int isBlank = Random.Range(0, maxHide);

                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
                isdone = true;
            }while (!isdone);
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[4], posY[4]] == "empty" && gridModel[posX[2], posY[2]] != "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        gridModel[posX[4], posY[4]] = Random.Range(1, maxRange).ToString();
                        if ((int.Parse(gridModel[posX[4], posY[4]]) - int.Parse(gridModel[posX[2], posY[2]])) < 0) continue;
                        gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) - int.Parse(gridModel[posX[2], posY[2]])).ToString();
                        break;
                    case "-":
                        gridModel[posX[4], posY[4]] = Random.Range(1, maxRange).ToString();
                        gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) + int.Parse(gridModel[posX[2], posY[2]])).ToString();
                        break;
                    case "*":
                        if (int.Parse(gridModel[posX[2], posY[2]]) == 1 || IsPrime(int.Parse(gridModel[posX[2], posY[2]]))) continue;
                        do
                        {
                            gridModel[posX[4], posY[4]] = Random.Range(2, maxRange).ToString();
                        } while (int.Parse(gridModel[posX[4], posY[4]]) % int.Parse(gridModel[posX[2], posY[2]]) != 0 || int.Parse(gridModel[posX[4], posY[4]]) == 1);
                        gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) / int.Parse(gridModel[posX[2], posY[2]])).ToString();
                        break;
                    case "/":
                        gridModel[posX[4], posY[4]] = Random.Range(2, 10).ToString();
                        if (int.Parse(gridModel[posX[4], posY[4]]) * int.Parse(gridModel[posX[2], posY[2]]) > maxValue) continue;
                        gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) * int.Parse(gridModel[posX[2], posY[2]])).ToString();
                        break;
                }
                if (gridModel[posX[0], posY[0]] == "empty")
                {
                    gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                }
                if (gridModel[posX[2], posY[2]] == "empty")
                {
                    gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                }
                if (gridModel[posX[4], posY[4]] == "empty")
                {
                    gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                }
                int isBlank = Random.Range(0, maxHide);
                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
            } while (!isdone);  
        }
        else
        if (gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[4], posY[4]] == "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        values[posX[2], posY[2]] = Random.Range(1, maxRange);
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] + values[posX[2], posY[2]];
                        break;
                    case "-":
                        values[posX[2], posY[2]] = Random.Range(1, maxRange);
                        if (values[posX[0], posY[0]] - values[posX[2], posY[2] ] < 0) continue;
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] - values[posX[2], posY[2]];
                        break;
                    case "*":
                        values[posX[2], posY[2]] = Random.Range(2, 10);
                        if (values[posX[0], posY[0]] * values[posX[2], posY[2]] > maxValue) continue;
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] * values[posX[2], posY[2]];
                        break;
                    case "/":
                        if (values[posX[0], posY[0]] == 1 || IsPrime(values[posX[0], posY[0]])) continue;
                        do
                        {
                            values[posX[2], posY[2]] = Random.Range(2, maxRange);
                        } while (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0);
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] / values[posX[2], posY[2]];
                        break;
                }
                if (gridModel[posX[0], posY[0]] == "empty")
                {
                    gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                }
                if (gridModel[posX[2], posY[2]] == "empty")
                {
                    gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                }
                if (gridModel[posX[4], posY[4]] == "empty")
                {
                    gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                }
                int isBlank = Random.Range(0, maxHide);

                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
                isdone = true;
            } while (!isdone);
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[2], posY[2]] != "empty" && gridModel[posX[0], posY[0]] != "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        if (values[posX[4], posY[4]] - values[posX[2], posY[2]] < 0) continue;
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] - values[posX[2], posY[2]];
                        break;
                    case "-":
                        values[posX[0], posY[0]] = values[posX[2], posY[2]] + values[posX[4], posY[4]];
                        break;
                    case "*":
                        if (values[posX[2], posY[2]] == 0 || values[posX[2], posY[2]] == 1 || values[posX[4], posY[4]] == 1 || IsPrime(values[posX[4], posY[4]])) continue;
                        if (values[posX[4], posY[4]] % values[posX[2], posY[2]] != 0)
                        {
                            values[posX[0], posY[0]] = values[posX[4], posY[4]] + values[posX[2], posY[2]];
                            gridModel[posX[1], posY[1]] = "-";
                        }
                        else
                        {
                            values[posX[0], posY[0]] = values[posX[4], posY[4]] / values[posX[2], posY[2]];
                        }
                        break;
                    case "/":
                        if(values[posX[4], posY[4]] * values[posX[2], posY[2]] > maxValue) continue;
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] * values[posX[2], posY[2]];
                        break;
                }
                if (gridModel[posX[0], posY[0]] == "empty")
                {
                    gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                }
                if (gridModel[posX[2], posY[2]] == "empty")
                {
                    gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                }
                if (gridModel[posX[4], posY[4]] == "empty")
                {
                    gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                }
                int isBlank = Random.Range(0, maxHide);

                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
                isdone = true;
            } while (!isdone);
        }
        else
        if (gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[0], posY[0]] != "empty" && gridModel[posX[4], posY[4]] != "empty")
        {
            bool isdone = false;
            if (values[posX[0], posY[0]] >= values[posX[4], posY[4]])
            {
                gridModel[posX[3], posY[3]] = "=";
                do
                {
                    gridModel[posX[1], posY[1]] = RanOp();
                    switch (gridModel[posX[1], posY[1]])
                    {
                        case "+":
                            if (values[posX[4], posY[4]] - values[posX[0], posY[0]] < 0) continue;
                            values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                            break;
                        case "-":
                            if (values[posX[0], posY[0]] - values[posX[4], posY[4]] < 0) continue;
                            values[posX[2], posY[2]] = values[posX[0], posY[0]] - values[posX[4], posY[4]];
                            break;
                        case "*":
                            if (values[posX[0], posY[0]] == 0 || values[posX[0], posY[0]] == 1 || values[posX[4], posY[4]] == 1 || IsPrime(values[posX[4], posY[4]])) continue;

                            if (values[posX[4], posY[4]] % values[posX[0], posY[0]] != 0)
                            {
                                if (values[posX[4], posY[4]] - values[posX[0], posY[0]] < 0) continue;
                                values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                                gridModel[posX[1], posY[1]] = "+";
                                
                            }
                            else
                            {
                                values[posX[2], posY[2]] = values[posX[4], posY[4]] / values[posX[0], posY[0]];
                            }
                            break;
                        case "/":
                            if (values[posX[4], posY[4]] == 0 || values[posX[4], posY[4]] == 1 || values[posX[0], posY[0]] == 1 || IsPrime(values[posX[0], posY[0]])) continue;

                            if (values[posX[0], posY[0]] % values[posX[4], posY[4]] != 0)
                            {
                                if (values[posX[4], posY[4]] - values[posX[0], posY[0]] < 0) continue;
                                values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                                gridModel[posX[1], posY[1]] = "+";
                            }
                            else
                            {
                                values[posX[2], posY[2]] = values[posX[0], posY[0]] / values[posX[4], posY[4]];
                            }
                            break;
                    }
                    if (gridModel[posX[0], posY[0]] == "empty")
                    {
                        gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                    }
                    if (gridModel[posX[2], posY[2]] == "empty")
                    {
                        gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                    }
                    if (gridModel[posX[4], posY[4]] == "empty")
                    {
                        gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                    }
                    int isBlank = Random.Range(0, maxHide);

                    switch (isBlank)
                    {
                        case 0:
                            gridModel[posX[0], posY[0]] = "blank";
                            break;
                        case 1:
                            gridModel[posX[2], posY[2]] = "blank";
                            break;
                        case 2:
                            gridModel[posX[4], posY[4]] = "blank";
                            break;
                    }
                    isdone = true;
                } while (!isdone);
            }
            else
            {
                gridModel[posX[1], posY[1]] = "=";
                do
                {
                    gridModel[posX[3], posY[3]] = RanOp();
                    switch (gridModel[posX[3], posY[3]])
                    {
                        case "+":
                            if (values[posX[0], posY[0]] - values[posX[4], posY[4]] < 0) continue;
                            values[posX[2], posY[2]] = values[posX[0], posY[0]] - values[posX[4], posY[4]];
                            break;
                        case "-":
                            if (values[posX[4], posY[4]] - values[posX[0], posY[0]] < 0) continue;
                            values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                            break;
                        case "*":
                            if (values[posX[0], posY[0]] % values[posX[4], posY[4]] != 0)
                            {
                                if (values[posX[0], posY[0]] - values[posX[4], posY[4]] < 0) continue;
                                values[posX[2], posY[2]] = values[posX[0], posY[0]] - values[posX[4], posY[4]];
                                gridModel[posX[1], posY[1]] = "+";

                            }
                            else
                            {
                                values[posX[2], posY[2]] = values[posX[0], posY[0]] / values[posX[4], posY[4]];
                            }
                            break;
                        case "/":
                            if (values[posX[0], posY[0]] == 0) continue;
                            if (values[posX[4], posY[4]] % values[posX[0], posY[0]] != 0)
                            {
                                if (values[posX[0], posY[0]] - values[posX[4], posY[4]] < 0) continue;
                                values[posX[2], posY[2]] = values[posX[0], posY[0]] - values[posX[4], posY[4]];
                                gridModel[posX[1], posY[1]] = "+";
                            }
                            else
                            {
                                values[posX[2], posY[2]] = values[posX[4], posY[4]] / values[posX[0], posY[0]];
                            }
                            break;
                    }
                    if (gridModel[posX[0], posY[0]] == "empty")
                    {
                        gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                    }
                    if (gridModel[posX[2], posY[2]] == "empty")
                    {
                        gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                    }
                    if (gridModel[posX[4], posY[4]] == "empty")
                    {
                        gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                    }
                    int isBlank = Random.Range(0, maxHide);

                    switch (isBlank)
                    {
                        case 0:
                            gridModel[posX[0], posY[0]] = "blank";
                            break;
                        case 1:
                            gridModel[posX[2], posY[2]] = "blank";
                            break;
                        case 2:
                            gridModel[posX[4], posY[4]] = "blank";
                            break;
                    }
                    isdone = true;
                } while (!isdone);
            }
        }
        else
        if (gridModel[posX[4], posY[4]] == "empty" && gridModel[posX[2], posY[2]] != "empty" && gridModel[posX[0], posY[0]] != "empty")
        {
            gridModel[posX[3], posY[3]] = "=";
            bool isdone = false;
            do
            {
                gridModel[posX[1], posY[1]] = RanOp();
                switch (gridModel[posX[1], posY[1]])
                {
                    case "+":
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] + values[posX[2], posY[2]];
                        break;
                    case "-":
                        if (values[posX[0], posY[0]] - values[posX[2], posY[2]] < 0) continue;
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] - values[posX[2], posY[2]];
                        break;
                    case "*":
                        if (values[posX[0], posY[0]] * values[posX[2], posY[2]] > maxValue) continue;
                        values[posX[4], posY[4]] = values[posX[0], posY[0]] * values[posX[2], posY[2]];
                        break;
                    case "/":
                        if (values[posX[2], posY[2]] == 1 || values[posX[2], posY[2]] == 0 || values[posX[0], posY[0]] == 1 || IsPrime(values[posX[0], posY[0]])) continue;
                        if (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0 || values[posX[2], posY[2]] == 0)
                        {
                            values[posX[4], posY[4]] = values[posX[0], posY[0]] + values[posX[2], posY[2]];
                            gridModel[posX[1], posY[1]] = "+";
                        }
                        else
                        {
                            values[posX[4], posY[4]] = values[posX[0], posY[0]] / values[posX[2], posY[2]];
                        }
                        break;
                }
                if (gridModel[posX[0], posY[0]] == "empty")
                {
                    gridModel[posX[0], posY[0]] = values[posX[0], posY[0]].ToString();
                }
                if (gridModel[posX[2], posY[2]] == "empty")
                {
                    gridModel[posX[2], posY[2]] = values[posX[2], posY[2]].ToString();
                }
                if (gridModel[posX[4], posY[4]] == "empty")
                {
                    gridModel[posX[4], posY[4]] = values[posX[4], posY[4]].ToString();
                }
                int isBlank = Random.Range(0, maxHide);

                switch (isBlank)
                {
                    case 0:
                        gridModel[posX[0], posY[0]] = "blank";
                        break;
                    case 1:
                        gridModel[posX[2], posY[2]] = "blank";
                        break;
                    case 2:
                        gridModel[posX[4], posY[4]] = "blank";
                        break;
                }
                isdone = true;
            } while (!isdone);
        }

    }
    public string RanOp()
    {
        switch (GameManager.instance.level)
        {
            case GameManager.Difficult.Easy:
                int op = Random.Range(0, 2);
                switch (op)
                {
                    case 0:
                        return "+";
                    case 1:
                        return "-";
                }
                break;
            case GameManager.Difficult.Medium:
                op = Random.Range(0, 2);
                switch (op)
                {
                    case 0:
                        return "+";
                    case 1:
                        return "-";

                }
                break;
            case GameManager.Difficult.Hard:
                op = Random.Range(0, 4);
                switch (op)
                {
                    case 0:
                        return "+";
                    case 1:
                        return "-";
                    case 2:
                        return "*";
                    case 3:
                        return "/";
                }
                break;
        }
        return "+";
    }
    static bool IsPrime(int number)
    {
        if (number <= 1)
            return false;
        if (number <= 3)
            return true;

        if (number % 2 == 0 || number % 3 == 0)
            return false;

        for (int i = 5; i * i <= number; i += 6)
        {
            if (number % i == 0 || number % (i + 2) == 0)
                return false;
        }

        return true;
    }

}



