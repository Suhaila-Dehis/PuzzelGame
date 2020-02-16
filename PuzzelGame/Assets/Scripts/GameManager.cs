using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject[] puzzlePieces;
    private Sprite[] puzzleImages;

    PuzzlePiece[,] Matrix = new PuzzlePiece[GameVariables.MaxRows, GameVariables.MaxColumns];

    private Vector3 screenPositionToAnimate;
    private PuzzlePiece pieceToAnimate;
    private int toAnimateRow, toAnimateColumn;
    float animSpeed = 10;
    int puzzleIndex;

    GameState gameState;

    private void OnLevelWasLoaded(int level)
    {
        // called each time we change a level
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            if (puzzleIndex > 0)
            {
                LoadPuzzle();
                GameStarted();
            }
        }
        else
        {
            if (puzzleIndex != -1)
            {
                puzzleIndex = -1;
            }

            if (puzzlePieces != null)
            {
                puzzlePieces = null;
            }

            if (gameState != GameState.End)
            {
                gameState = GameState.End;
            }
        }
    }


    void CheckInput()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                string[] parts = hit.collider.gameObject.name.Split('-');
                int rowPart = int.Parse(parts[1]);
                int columnPart = int.Parse(parts[2]);

                int rowFound = -1;
                int columnFound = -1;

                for (int row = 0; row < GameVariables.MaxRows; row++)
                {
                    if (rowFound != -1)
                        break;

                    for (int column = 0; column < GameVariables.MaxColumns; column++)
                    {
                        if (columnFound != -1)
                            break;

                        if (Matrix[row, column] == null)
                            continue;

                        if (Matrix[row, column].OriginalRow == rowPart && Matrix[row, column].OriginalColumn == columnPart)
                        {
                            // puzzle piece found
                            rowFound = row;
                            columnFound = column;
                        }// if we found row and column
                    }// for column
                }// for row

                bool pieceFound = false;
                if (rowFound > 0 && Matrix[rowFound - 1, columnFound] == null)
                {
                    pieceFound = true;
                    toAnimateRow = rowFound - 1;
                    toAnimateColumn = columnFound;

                }
                else if (columnFound > 0 && Matrix[rowFound, columnFound - 1] == null)
                {
                    pieceFound = true;
                    toAnimateRow = rowFound;
                    toAnimateColumn = columnFound - 1;

                }
                else if (rowFound < GameVariables.MaxRows - 1 && Matrix[rowFound + 1, columnFound] == null)
                {
                    pieceFound = true;
                    toAnimateRow = rowFound + 1;
                    toAnimateColumn = columnFound;
                }
                else if (columnFound < GameVariables.MaxColumns - 1 && Matrix[rowFound, columnFound + 1] == null)
                {
                    pieceFound = true;
                    toAnimateRow = rowFound;
                    toAnimateColumn = columnFound = 1;
                }

                // animate the puzzle
                if (pieceFound)
                {
                    screenPositionToAnimate = GetScreenCoordinatesFromViewPort(toAnimateRow, toAnimateColumn);

                    pieceToAnimate = Matrix[rowFound, columnFound];
                    gameState = GameState.Animating;

                }
            }
        }
    } 
        

    void AnimateMovement(PuzzlePiece toMove,float time)
    {
        toMove.gameObject.transform.position = Vector2.MoveTowards(toMove.gameObject.transform.position, screenPositionToAnimate, animSpeed * time);
    }

    void CheckIfAnimationEnded()
    {
        if (Vector2.Distance(pieceToAnimate.gameObject.transform.position, screenPositionToAnimate) < 0.1f)
        {
            Swap(pieceToAnimate.CurrentRow, pieceToAnimate.CurrentColumn, toAnimateRow, toAnimateColumn);

            gameState = GameState.Playing;
            CheckForVictor();
        }
    }

    void CheckForVictor()
    {
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                if (Matrix[row, column] == null)
                    continue;

                if (Matrix[row, column].CurrentRow != Matrix[row, column].OriginalRow || Matrix[row, column].CurrentColumn != Matrix[row, column].OriginalColumn)
                {
                    return;
                }
            }
        }
        gameState = GameState.End;
    }

    void GameStarted()
    {
        int index = Random.Range(0, GameVariables.MaxSize);
        puzzlePieces[index].SetActive(false); // the empty puzzle Piece

        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                if (puzzlePieces[row * GameVariables.MaxColumns + column].activeInHierarchy)
                {
                    Vector3 point = GetScreenCoordinatesFromViewPort(row, column);
                    puzzlePieces[row * GameVariables.MaxColumns + column].transform.position = point;


                    Matrix[row, column] = new PuzzlePiece();
                    Matrix[row, column].gameObject = puzzlePieces[row * GameVariables.MaxColumns + column];
                    Matrix[row, column].OriginalRow = row;
                    Matrix[row, column].OriginalColumn = column;

                }
                else
                {
                    Matrix[row, column] = null;
                }
            }
        }

        Shuffle();
        gameState = GameState.Playing;
    }

    void Shuffle()
    {
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                if (Matrix[row, column] == null)                
                    continue;

                int randomRow = Random.Range(0, GameVariables.MaxRows);
                int randomColumn = Random.Range(0, GameVariables.MaxColumns);
                Swap(row, column, randomRow, randomColumn);
            }
        }
    }


    void Swap(int row, int column, int randomRow, int randomColumn)
    {
        PuzzlePiece temp = Matrix[row, column];
        Matrix[row, column] = Matrix[randomRow, randomColumn];
        Matrix[randomRow, randomColumn] = temp;

        if (Matrix[row, column] != null)
        {
            Matrix[row, column].gameObject.transform.position = GetScreenCoordinatesFromViewPort(row, column);
            Matrix[row, column].CurrentRow = row;
            Matrix[row, column].CurrentColumn = column;
        }

        Matrix[randomRow, randomColumn].gameObject.transform.position = GetScreenCoordinatesFromViewPort(randomRow, randomColumn);
        Matrix[randomRow, randomColumn].CurrentRow = randomRow;
        Matrix[randomRow, randomColumn].CurrentColumn = randomColumn;

    }


    Vector3 GetScreenCoordinatesFromViewPort(int row,int column)
    {
        Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.225f * row, 1 - 0.235f * column), 0);
        point.z = 0;
        return point;
    }

    void LoadPuzzle()
    {
        puzzleImages = Resources.LoadAll<Sprite>("Sprites/BG " + puzzleIndex);
        puzzlePieces = GameObject.Find("Puzzle Holder").GetComponent<PuzzleHolder>().puzzlePieces;

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            puzzlePieces[i].GetComponent<SpriteRenderer>().sprite = puzzleImages[i];
        }

    }

    private void Awake()
    {
        MakeInstace();
    }

    private void Start()
    {
        puzzleIndex = -1;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            switch (gameState)
            {
                case GameState.Playing:
                    CheckInput();
                    break;

                case GameState.Animating:
                    AnimateMovement(pieceToAnimate, Time.deltaTime);
                    CheckIfAnimationEnded();
                    break;

                case GameState.End:
                    Debug.Log("Game Over");
                    break;
                default:
                    break;
            }
        }
    }

    void MakeInstace()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPuzzleIndex(int puzzleIndex)
    {
        this.puzzleIndex = puzzleIndex;
    }
}