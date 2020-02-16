using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece 
{

    public int CurrentRow { get; set; }
    public int CurrentColumn { get; set; }

    public int OriginalRow { get; set; }
    public int OriginalColumn { get; set; }

    public  GameObject gameObject { get; set; }

}