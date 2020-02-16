using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPuzzleController : MonoBehaviour
{

    public void SelectPuzzle()
    {
        string[] name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.Split();
        int index = int.Parse(name[1]);

        if (GameManager.instance != null)
        {
            GameManager.instance.SetPuzzleIndex(index);
        }
        SceneManager.LoadScene("GamePlay");

    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}