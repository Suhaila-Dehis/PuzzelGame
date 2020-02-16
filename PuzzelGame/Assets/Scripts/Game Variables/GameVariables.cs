using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables 
{
    public static int MaxRows = 4;
    public static int MaxColumns = 4;
    public static int MaxSize = MaxRows * MaxColumns;


}


public enum GameState
{
    Playing,
    Animating,
    End
}
