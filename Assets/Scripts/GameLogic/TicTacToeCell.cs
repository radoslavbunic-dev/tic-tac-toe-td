using UnityEngine;

public class TicTacToeCell : MonoBehaviour
{
    public int Index { get; private set; }
    public TicTacToeMark Mark { get; private set; }

    public void Init(int index)
    {
        Index = index;
        Mark = TicTacToeMark.None;
    }

    public void CreateMark(GameObject prefab, TicTacToeMark mark)
    {
        Mark = mark;
        Instantiate(prefab, transform);
    }

    public bool IsValidToPlaceMark()
    {
        return Mark == TicTacToeMark.None;
    }
}
