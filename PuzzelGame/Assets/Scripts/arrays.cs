using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrays : MonoBehaviour
{
    public static int Row = 6;
    public static int Column = 5;

    private int[,] number = new int[Row, Column];


    private void Start()
    {
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Column; column++)
            {
                number[row, column] = Random.Range(10, 50);
            }
        }

        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Column; column++)
            {
                Debug.Log("the number is "+number[row,column]);
            }
        }

    }
}