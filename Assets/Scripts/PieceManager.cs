using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
   [SerializeField] private GameObject piecePrefab;
    
    private bool isGameAlive = true;
    
    private List<BasePiece> whitePieces;
    private List<BasePiece> blackPieces;
    private List<Cell> cellPos;
    private bool PvP;

    private string[] pieceOrder = new string[9]
    {
        "P", "P", "P", 
        "P", "P", "P", 
        "P", "P", "P"
    };

    private Dictionary<string, Type> pieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
    };
    
    public void Setup(Board board, string gameMode, bool PvP)
    {
        this.PvP = PvP;
        cellPos = new List<Cell>();
        
        whitePieces = CreatePieces(Color.white, new Color32(236, 236, 236, 255), gameMode);
        blackPieces = CreatePieces(Color.black, new Color32(46, 49, 49, 255), gameMode);

        PlacePieces(5,2, 1, 0, Color.white,  whitePieces, board);
        PlacePieces(0,5, 6, 7, Color.black, blackPieces, board);

        SwitchSides(Color.black);
    }
    
    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, string gameMode)
    {
        var newPieces = new List<BasePiece>();
        
        foreach (var key in pieceOrder)
        {
            var pieceType = pieceLibrary[key];
            var newPiece = CreatePiece(pieceType);
            newPiece.Setup(teamColor, spriteColor, this, gameMode);
            newPieces.Add(newPiece);
        }
        
        return newPieces;
    }
    
    private BasePiece CreatePiece(Type pieceType)
    {
        var newPieceObject = Instantiate(piecePrefab);
        newPieceObject.transform.SetParent(transform);

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        var newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);

        return newPiece;
    }
    
    private void PlacePieces(int index, int firstRow, int secontRow, int thirdRow, Color color, List<BasePiece> pieces, Board board)
    {
        for (var i = 0; i < 3; i++)
        { 
            pieces[i].Place(board.cells[i + index, firstRow]);
            var cell = board.cells[i + index, firstRow];
            cell.Color = color;
            cellPos.Add(cell);
            
            pieces[i + 3].Place(board.cells[i + index, secontRow]);
            var cell1 = board.cells[i + index, secontRow];
            cell1.Color = color;
            cellPos.Add(cell1);
            
            pieces[i + 6].Place(board.cells[i + index, thirdRow]);
            var cell2 = board.cells[i + index, thirdRow];
            cell2.Color = color;
            cellPos.Add(cell2);
        }
    }
    
    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (var piece in allPieces)
            piece.enabled = value;
    }
    
    public void SwitchSides(Color color)
    {
        IsGameOver();
        if (!isGameAlive)
        {
            ResetPieces();
            isGameAlive = true;
            color = Color.black;
        }

        var isBlackTurn = color == Color.white ? true : false;

        SetInteractive(whitePieces, !isBlackTurn);
        SetInteractive(blackPieces, isBlackTurn);
    }
    
    private void ResetPieces()
    {
        foreach (var piece in whitePieces)
            piece.Reset();

        foreach (var piece in blackPieces)
            piece.Reset();
    }

    private void IsGameOver()
    {
        var w = 0;
        var b = 0;
        foreach (var cell in cellPos)
        {
            var cellState = CellState.None;
            cellState = cell.Board.ValidateCell(cell.BoardPosition.x, cell.BoardPosition.y, cell.Color);

            if (cellState == CellState.Enemy)
            {
                if (cell.Color == Color.white)
                {
                    w++;
                    if (w >= 9)
                    {
                        Debug.Log("Победа черных!");
                        ResetPieces();
                        isGameAlive = true;
                    }
                }

                if (cell.Color == Color.black)
                {
                    b++;
                    if (b >= 9)
                    {
                        Debug.Log("Победа белых!");
                        ResetPieces();
                        isGameAlive = true;
                    }
                }
            }
        }
    }
}
