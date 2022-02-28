using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image outlineImage;

    private Vector2Int boardPosition = Vector2Int.zero;
    private Board board;
    private RectTransform rectTransform;
    private BasePiece currentPiece;
    private Color color = Color.clear;
    
    public Image OutlineImage => outlineImage;
    public Vector2Int BoardPosition => boardPosition;
    public Board Board => board;
    public RectTransform RectTransform => rectTransform;
    public BasePiece CurrentPiece
    {
        get => currentPiece;
        set => currentPiece = value;
    }
    public Color Color
    {
        get => color;
        set => color = value;
    }

    public void Setup(Vector2Int boardPosition, Board board)
    {
        this.boardPosition = boardPosition;
        this.board = board;

        rectTransform = GetComponent<RectTransform>();
    }

}
