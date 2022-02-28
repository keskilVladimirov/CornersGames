using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private List<Cell> listCell;
    
    public override void Setup(Color teamColor, Color32 spriteColor, PieceManager pieceManager, string gameMode)
    {
        base.Setup(teamColor, spriteColor, pieceManager, gameMode);
        this.gameMode = gameMode;

        listCell = new List<Cell>();
        movement = new Vector3Int(1, 1, 1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Pawn");
    }

    protected override void CheckPathing()
    {
        var currentX = currentCell.BoardPosition.x;
        var currentY = currentCell.BoardPosition.y;

        listCell.Clear();
        CreateCellPath(currentX, currentY, true);
    }
    
    private void CreateCellPath(int currentX, int currentY, bool firstCell)
    {
        if (gameMode == "DiagonalMode")
        {
            var checkTopLeft = MatchesState(currentX - 1, currentY + 1, false, firstCell);
            if (!checkTopLeft)
                MatchesState(currentX - 2, currentY + 2, true, firstCell);

            var checkTopRight = MatchesState(currentX + 1, currentY + 1, false, firstCell);
            if (!checkTopRight)
                MatchesState(currentX + 2, currentY + 2, true, firstCell);

            var checkBottomLeft = MatchesState(currentX - 1, currentY - 1, false, firstCell);
            if (!checkBottomLeft)
                MatchesState(currentX - 2, currentY - 2, true, firstCell);

            var checkBottomRight = MatchesState(currentX + 1, currentY - 1, false, firstCell);
            if (!checkBottomRight)
                MatchesState(currentX + 2, currentY - 2, true, firstCell);
            

            for (var x = -1; x <= 1; x++)
            {
                if (x == 0) continue;
                var checkX = currentX + x;
                MatchesState(checkX, currentY, false, firstCell);
            }
            for (var y = -1; y <= 1; y++)
            {
                if (y == 0) continue;
                var checkY = currentY + y;
                MatchesState(currentX, checkY, false, firstCell);
            }
        }

        if (gameMode == "VerHorMode")
        {
            for (var x = -1; x <= 1; x++)
            {
                if (x == 0) continue;
                var checkX = currentX + x;
                 
                var check = MatchesState(checkX, currentY, false, firstCell);
                if (!check)
                {
                    var checkX1 = currentX + (x * 2);
                    MatchesState(checkX1, currentY, true, firstCell);
                }
            }
            
            for (var y = -1; y <= 1; y++)
            {
                if (y == 0) continue;
                var checkY = currentY + y;
                 
                var check = MatchesState(currentX, checkY, false, firstCell);
                if (!check)
                {
                    var checkY1 = currentY + (y * 2);
                    MatchesState(currentX, checkY1, true, firstCell);
                }
                    
            }
        }

        if (gameMode == "NoMode")
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    
                    var checkX = currentX + x;
                    var checkY = currentY + y;
                    MatchesState(checkX, checkY, false, firstCell);
                }
            }
        }
    }
    
    private bool MatchesState(int targetX, int targetY, bool recurs, bool firstCell)
    {
        var cellState = CellState.None;
        cellState = currentCell.Board.ValidateCell(targetX, targetY, this);

        if (cellState == CellState.Free)
        {
            if (recurs)
            {
                highlightedCells.Add(currentCell.Board.cells[targetX, targetY]);
                var tempTargetCell = currentCell.Board.cells[targetX, targetY];
                if(listCell.Contains(tempTargetCell)) 
                    return true;

                listCell.Add(tempTargetCell);
                CreateCellPath(tempTargetCell.BoardPosition.x,  tempTargetCell.BoardPosition.y, false);
            }
            else
            {
                if (firstCell)
                {
                    highlightedCells.Add(currentCell.Board.cells[targetX, targetY]);
                    return true;
                }
                return true;
            }
        }
        
        return false;
    }
}


