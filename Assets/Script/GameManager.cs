using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GenerateMath playzone;

    public int length;

    public bool[,] check;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
        if (checkWin())
        {
            Victory();
        }
    }

    private void Victory()
    {
        Debug.Log("Victory");
        
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
}
