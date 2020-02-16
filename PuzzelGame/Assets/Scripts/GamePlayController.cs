using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{

    public void BackToSelectPuzzleMenu()
    {
        SceneManager.LoadScene("SelectPuzzleMenu");
    }
}