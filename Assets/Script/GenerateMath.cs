using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
    GameObject[,] grid;
    List<GameObject> cellAnswer;
    List<GameObject> cellBlank;
    string[,] gridModel;
    int[,] values;
    public int[] posX = new int[5];
    public int[] posY = new int[5];
    public Directions dir;
    public Transform startPoint;
    public Transform startAnsPoint;
    public Transform spawn;
    public Transform spawnAns;
    bool up = true, down = true, left = true, right = true;
    bool canChangeDir = true;
    public int size;
    private void Awake()
    {
        cellAnswer = new List<GameObject>();
        cellBlank = new List<GameObject>();
        dir = Directions.right;
        grid = new GameObject[8, 8];
        gridModel = new string[8, 8];
        values = new int[8, 8];
        GenerateModel();
        for (int y = 7; y >= 0; y--)
        {
            Debug.Log(gridModel[0, y] + "\t" + gridModel[1, y] + "\t" + gridModel[2, y] + "\t" + gridModel[3, y] + "\t" + gridModel[4, y] + "\t" + gridModel[5, y] + "\t" + gridModel[6, y] + "\t" + gridModel[7, y]);
        }
        GenerateGrid();
        SuffleAnswers();
    }

    private void SuffleAnswers()
    {
        for (int i = 0; i < cellAnswer.Count; i++)
        {
            int ran = Random.Range(0, cellAnswer.Count);
            Vector3 temp = cellAnswer[i].transform.position;
            cellAnswer[i].transform.position = cellAnswer[ran].transform.position;
            cellAnswer[ran].transform.position = temp;
        }
    }

    private void GenerateGrid()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                
                Vector3 spawnPoint = new Vector3(i + startPoint.position.x, j + startPoint.position.y, 0);
                switch (gridModel[i, j])
                {
                    case "+":
                        grid[i,j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);
                        grid[i, j].GetComponent<Op>().op = Op.Operations.plus;
                        break;
                    case "-":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);

                        grid[i, j].GetComponent<Op>().op = Op.Operations.minus;
                        break;
                    case "*":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);

                        grid[i, j].GetComponent<Op>().op = Op.Operations.multiply;
                        break;
                    case "/":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);

                        grid[i, j].GetComponent<Op>().op = Op.Operations.divide;
                        break;
                    case "=":
                        grid[i, j] = Instantiate(opPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);

                        grid[i, j].GetComponent<Op>().op = Op.Operations.equal;
                        break;
                    case "empty":
                        continue;
                    case "blank":
                        grid[i, j] = Instantiate(blankPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);
                        grid[i, j].GetComponent<Blank>().value = values[i, j];
                        Vector3 ansPoint = new Vector3(startAnsPoint.position.x + count * 1.1f, startAnsPoint.position.x - count / 8 * 1.1f);
                        GameObject cell = new GameObject("Ans" + count);
                        cell.transform.SetParent(spawnAns);
                        cell = Instantiate(answerPrefab, ansPoint, Quaternion.identity);
                        cell.GetComponent<Number>().value = values[i, j];
                        cellAnswer.Add(cell);
                        cellBlank.Add(grid[i, j]);
                        count++;
                        break;
                    default:
                        grid[i, j] = Instantiate(numberPrefab, spawnPoint, Quaternion.identity);
                        grid[i, j].transform.SetParent(spawn);
                        grid[i, j].GetComponent<Number>().value = values[i, j];
                        break;
                }
            }
        }
    }

    private void GenerateModel()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridModel[i, j] = "empty";
            }
        }
        int x = 0, y = 0;

        //generate model
        do 
        {
            Debug.Log("---Dir: " + dir);
            Debug.Log("x: " + x + "; y: " + y);
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
                        Debug.Log("*ranPos: " + ranPos);
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
                            
                            Debug.Log("*ranDir: " + ranDir);
                            switch (ranDir)
                            {
                                case 0:
                                    if (x - 4 < 0 || gridModel[x - 1, y] != "empty" || gridModel[x - 3, y] != "empty")
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
                                    if (x + 4 > 7 || gridModel[x + 1, y] != "empty" || gridModel[x + 3, y] != "empty")
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
                        Debug.Log("*ranPos: " + ranPos);
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
                            Debug.Log("*ranDir: " + ranDir);
                            switch (ranDir)
                            {
                                case 0:
                                    if (x - 4 < 0 || gridModel[x - 1, y] != "empty" || gridModel[x - 3, y] != "empty")
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
                                    if (x + 4 > 7 || gridModel[x + 1, y] != "empty" || gridModel[x + 3, y] != "empty")
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
                        Debug.Log("*ranPos: " + ranPos);
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
                            Debug.Log("*ranDir: " + ranDir);    
                            switch (ranDir)
                            {
                                case 0:
                                    if (y - 4 < 0 || gridModel[x, y - 1] != "empty" || gridModel[x, y - 3] != "empty")
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
                                    if (y + 4 > 7 || gridModel[x, y + 1] != "empty" || gridModel[x, y + 3] != "empty")
                                    {
                                        check = false;

                                        up = false;
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
                        Debug.Log("*ranPos: " + ranPos);
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
                            Debug.Log("*ranDir: " + ranDir);
                            switch (ranDir)
                            {
                                case 0:
                                    Debug.Log("y: " + y);
                                    if (y - 4 < 0 || gridModel[x, y -1] != "empty" || gridModel[x, y - 3] != "empty")
                                    {
                                        down = false;
                                        check = false;

                                        break;
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
                                    if (y + 4 > 7 || gridModel[x, y + 1] != "empty" || gridModel[x, y + 3] != "empty")
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
                            Debug.Log("Check: " + check);
                            Debug.Log("canChangeDir: " + canChangeDir);
                            if (check)
                            {
                                canChangeDir = true;
                                break;
                            }

                        } while (canChangeDir);
                        break;
                }
            } while(canChangeDir);
            Debug.Log(gridModel[posX[0], posY[0]] + "\t" + gridModel[posX[1], posY[1]] + "\t" + gridModel[posX[2], posY[2]] + gridModel[posX[3], posY[3]] + "\t" + gridModel[posX[4], posY[4]]);
            Debug.Log("up: " + up + " down: " + down + " left: " + left + " right: " + right);
        } while (canChangeDir);
    }

    private void GenerateValue()
    {

        gridModel[posX[1], posY[1]] = RanOp();
        gridModel[posX[3], posY[3]] = "=";
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[4], posY[4]] == "empty")
        {
            values[posX[0], posY[0]] = Random.Range(1, 10);
            values[posX[2], posY[2]] = Random.Range(1, 10);
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[4], posY[4]] = (values[posX[0], posY[0]] + values[posX[2], posY[2]]);
                    break;
                case "-":
                    values[posX[4], posY[4]] = (values[posX[0], posY[0]] - values[posX[2], posY[2]]);
                    break;
                case "*":
                   values[posX[4], posY[4]] = (values[posX[0], posY[0]] * values[posX[2], posY[2]]);
                    break;
                case "/":
                    if (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0)
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);

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
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[2], posY[2]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[2], posY[2]] = Random.Range(1, 10);
                    values[posX[0], posY[0]] = (values[posX[4], posY[4]] - values[posX[2], posY[2]]);
                    break;
                case "-":
                    values[posX[2], posY[2]] = Random.Range(1, 10);
                    values[posX[0], posY[0]] = values[posX[4], posY[4]] + values[posX[2], posY[2]];
                    break;
                case "*":
                    do
                    {
                        values[posX[2], posY[2]] = Random.Range(1, 10);
                    } while (values[posX[4], posY[4]] % values[posX[2], posY[2]] != 0);
                    values[posX[0], posY[0]] = values[posX[4], posY[4]] / values[posX[2], posY[2]] ;
                    break;
                case "/":
                    values[posX[2], posY[2]] = Random.Range(2, 6);
                    values[posX[0], posY[0]] = values[posX[4], posY[4]]  * values[posX[2], posY[2]] ;
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
            int isBlank = Random.Range(0, 3);

            Debug.Log("isBlank: " + isBlank);
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
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty" && gridModel[posX[4], posY[4]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    gridModel[posX[4], posY[4]] = Random.Range(1, 10).ToString();
                    gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) - int.Parse(gridModel[posX[2], posY[2]])).ToString();
                    break;
                case "-":
                    gridModel[posX[4], posY[4]] = Random.Range(1, 10).ToString();
                    gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) + int.Parse(gridModel[posX[2], posY[2]])).ToString();
                    break;
                case "*":
                    do
                    {
                        gridModel[posX[4], posY[4]] = Random.Range(1, 10).ToString();
                    } while (int.Parse(gridModel[posX[4], posY[4]]) % int.Parse(gridModel[posX[2], posY[2]]) != 0);
                    gridModel[posX[0], posY[0]] = (int.Parse(gridModel[posX[4], posY[4]]) / int.Parse(gridModel[posX[2], posY[2]])).ToString();
                    break;
                case "/":
                    gridModel[posX[4], posY[4]] = Random.Range(2, 6).ToString();
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);
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
        }
        else
        if (gridModel[posX[2], posY[2]] == "empty" && gridModel[posX[4], posY[4]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[2], posY[2]] = Random.Range(1, 10);
                    values[posX[4], posY[4]] = values[posX[0], posY[0]] + values[posX[2], posY[2]];
                    break;
                case "-":
                    values[posX[2], posY[2]] = Random.Range(1, 10);
                    values[posX[4], posY[4]] = values[posX[0], posY[0]]  - values[posX[2], posY[2]];
                    break;
                case "*":
                    values[posX[2], posY[2]] = Random.Range(2, 6);
                    values[posX[4], posY[4]] = values[posX[0], posY[0]] * values[posX[2], posY[2]]   ;
                    break;
                case "/":
                    do
                    {
                        values[posX[2], posY[2]] = Random.Range(1, 10);
                    } while (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0);
                    values[posX[4], posY[4]] = values[posX[0], posY[0]]  / values[posX[2], posY[2]] ;
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);

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
        }
        else
        if (gridModel[posX[0], posY[0]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[0], posY[0]] = values[posX[4], posY[4]] - values[posX[2], posY[2]];
                    break;
                case "-":
                    values[posX[0], posY[0]] = values[posX[2], posY[2]] + values[posX[4], posY[4]];
                    break;
                case "*":
                    if (values[posX[4], posY[4]] % values[posX[2], posY[2]] != 0)
                    {
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] - values[posX[2], posY[2]];
                        gridModel[posX[1], posY[1]] = "+";
                    }
                    else
                    {
                        values[posX[0], posY[0]] = values[posX[4], posY[4]] / values[posX[2], posY[2]];
                    }
                    break;
                case "/":
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);

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
        }
        else
        if (gridModel[posX[2], posY[2]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                    break;
                case "-":
                    values[posX[2], posY[2]] = values[posX[0], posY[0]] - values[posX[4], posY[4]];
                    break;
                case "*":
                    if (values[posX[4], posY[4]] % values[posX[0], posY[0]] != 0)
                    {
                        values[posX[2], posY[2]] = values[posX[4], posY[4]] - values[posX[0], posY[0]];
                        gridModel[posX[1], posY[1]] = "+";
                    }
                    else
                    {
                        values[posX[2], posY[2]] = values[posX[4], posY[4]] / values[posX[0], posY[0]];
                    }
                    break;
                case "/":
                    if (values[posX[0], posY[0]] % values[posX[4], posY[4]] != 0)
                    {
                        values[posX[2], posY[2]] = values[posX[4], posY[4]] + values[posX[0], posY[0]];
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);

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
        }
        else
        if (gridModel[posX[4], posY[4]] == "empty")
        {
            switch (gridModel[posX[1], posY[1]])
            {
                case "+":
                    values[posX[4], posY[4]] = values[posX[0], posY[0]] + values[posX[2], posY[2]];
                    break;
                case "-":
                    values[posX[4], posY[4]] = values[posX[0], posY[0]] - values[posX[2], posY[2]];
                    break;
                case "*":
                    values[posX[4], posY[4]] = values[posX[0], posY[0]] * values[posX[2], posY[2]];
                    break;
                case "/":
                    if (values[posX[0], posY[0]] % values[posX[2], posY[2]] != 0 || values[posX[2], posY[2]]==0)
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
            int isBlank = Random.Range(0, 3);
            Debug.Log("isBlank: " + isBlank);

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
        }
    }

    private string RanOp()
    {
        int op = Random.Range(0, 4);
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
        return "+";
    }


}
