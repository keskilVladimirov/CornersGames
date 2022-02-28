using UnityEngine;
using UnityEngine.UI;


public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}
public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    protected internal Cell[,] cells = new Cell[8, 8];
    
    public void Create()
    {
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                var cell = Instantiate(cellPrefab, transform);

                var rectTransform = cell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                cells[x, y] = cell.GetComponent<Cell>();
                cells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        for (var x = 0; x < 8; x += 2)
        {
            for (var y = 0; y < 8; y++)
            {
                var offset = (y % 2 != 0) ? 0 : 1;
                var finalX = x + offset;
                
                cells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }
    
    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        var targetCell = cells[targetX, targetY];

        if (targetCell.CurrentPiece != null)
        {
            if (checkingPiece.color == targetCell.CurrentPiece.color)
                return CellState.Friendly;

            if (checkingPiece.color != targetCell.CurrentPiece.color)
                return CellState.Enemy;
        }

        return CellState.Free;
    }
    
    public CellState ValidateCell(int targetX, int targetY, Color color)
    {
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        var targetCell = cells[targetX, targetY];

        if (targetCell.CurrentPiece != null)
        {
            if (color == targetCell.CurrentPiece.color)
                return CellState.Friendly;

            if (color != targetCell.CurrentPiece.color)
                return CellState.Enemy;
        }

        return CellState.Free;
    }
}
