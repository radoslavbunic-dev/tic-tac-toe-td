using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Action<TicTacToeCell> OnBoardClicked;

    [SerializeField] TicTacToeCell[] cells;
    public SkinTemplate Skin { get; private set; }

    void OnEnable()
    {
        GameState.OnStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(IState fromState, IState toState)
    {
        if (toState is MovePlayedState movePlayedState)
        {
            SpawnMark(movePlayedState.Cell, movePlayedState.Player.Mark);
        }
        else if (toState is GameOverState gameOverState)
        {
            if (gameOverState.GameOverData.WinningLine != null)
            {
                ShowWinLine(gameOverState.GameOverData.WinningLine);
            }
        }
    }

    public void Init(SkinTemplate skin)
    {
        Skin = skin;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].Init(i);
            }
        }
    }

    void OnMouseDown()
    {
        if (!GetPointerWorldPointOnBoard(out Vector3 worldPoint))
        {
            return;
        }

        int bestIndex = FindNearestCellIndex(worldPoint);
        TicTacToeCell clickedCell = cells[bestIndex];
        if (clickedCell == null || !clickedCell.IsValidToPlaceMark())
        {
            return;
        }

        OnBoardClicked?.Invoke(clickedCell);
    }

    bool GetPointerWorldPointOnBoard(out Vector3 worldPoint)
    {
        worldPoint = default;
        Camera cam = Camera.main;
        if (cam == null)
        {
            return false;
        }

        float planeZ = transform.position.z;
        float distanceFromCameraPlane = Mathf.Abs(cam.transform.position.z - planeZ);
        Vector3 screen = Input.mousePosition;
        screen.z = distanceFromCameraPlane;
        worldPoint = cam.ScreenToWorldPoint(screen);
        return true;
    }

    int FindNearestCellIndex(Vector3 pointerWorld)
    {
        int bestIndex = -1;
        float bestSqr = float.PositiveInfinity;
        float minDistance = Constants.MinSquareDistance;

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == null)
            {
                continue;
            }

            float sqr = SqrDistanceXY(pointerWorld, cells[i].transform.position);
            if (bestIndex < 0)
            {
                bestIndex = i;
                bestSqr = sqr;
                continue;
            }

            if (sqr < bestSqr - minDistance)
            {
                bestSqr = sqr;
                bestIndex = i;
            }
            else if (Mathf.Abs(sqr - bestSqr) <= minDistance && i < bestIndex)
            {
                bestIndex = i;
            }
        }

        return bestIndex < 0 ? 0 : bestIndex;
    }

    static float SqrDistanceXY(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return dx * dx + dy * dy;
    }

    public Transform GetCellTransform(int index)
    {
        if (cells == null || index < 0 || index >= cells.Length || cells[index] == null)
        {
            return null;
        }

        return cells[index].transform;
    }


    void SpawnMark(TicTacToeCell cell, TicTacToeMark mark)
    {
        GameObject prefab = Skin.GetMarkPrefab(mark);
        if (prefab == null)
        {
            return;
        }
        cell.CreateMark(prefab, mark);
    }

    void ShowWinLine(int[] winLine)
    {
        if (winLine == null || winLine.Length < 3 || Skin == null)
        {
            return;
        }

        GameObject prefab = Skin.WinLinePrefab;
        if (prefab == null)
        {
            return;
        }

        Transform startCell = GetCellTransform(winLine[0]);
        Transform endCell = GetCellTransform(winLine[2]);
        if (startCell == null || endCell == null)
        {
            return;
        }

        GameObject instance = Instantiate(prefab, transform);
        WinLine line = instance.GetComponent<WinLine>();
        if (line != null)
        {
            line.Play(startCell.position, endCell.position);
        }

        GameEvents.PlaySFX(AudioClipsId.WinGame);
    }
}
