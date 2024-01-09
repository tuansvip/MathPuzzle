using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMath : MonoBehaviour
{
    public enum Directions
    {
        up,
        down,
        left,
        right
    }
    public GameObject opPrefab, numberPrefab, blankPrefab;
    GameObject[,] grid;
    string[,] gridModel;
    int[] posX = new int[5];
    int[] posY = new int[5];
    public Directions dir;
    int mcount = 1;
    private void Awake()
    {
        dir = Directions.right;
        grid = new GameObject[8, 8];
        gridModel = new string[8, 8];
        GenerateModel();
    }

    private void GenerateModel()
    {
        for (int i = 0; i < 500; i++)
        {
            for (int j = 0; j < 500; j++)
            {
                gridModel[i, j] = "empty";
            }
        }
        int x = 0, y = 0;
        int count = 0;
        do
        {
            if (gridModel[x, y] == "empty")
            {
                switch (count)
                {
                    case 0:
                        int asX = x, asY = y;
                        switch (dir)
                        {
                            case Directions.up:
                                asY += 4;
                                break;
                            case Directions.down:
                                asY -= 4;
                                break;
                            case Directions.left:
                                asX -= 4;
                                break;
                            case Directions.right:
                                asX += 4;
                                break;
                        }
                        if (asX < 0 || asX > 500 || asY < 0 || asY > 500)
                        {
                            return;
                        }
                        if (gridModel[asX, asY] != "empty")
                        {
                            gridModel[x, y] = Random.Range(1, 100).ToString();
                            posX[count] = x;
                            posY[count] = y;
                            mcount++;
                        }
                        break;
                    case 1:
                        gridModel[x, y] = RanOp();
                        posX[count] = x;
                        posY[count] = y;
                        break;
                    case 2:
                        gridModel[x, y] = Random.Range(1, 100).ToString();
                        posX[count] = x;
                        posY[count] = y;
                        mcount++;
                        break;
                }
            }
        } while (mcount <= 5);
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
