using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePiece : EventTrigger
{ 
    protected internal Color color = Color.clear;
    protected string gameMode;
    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();
    protected Cell currentCell;
    
    private Cell originalCell;
    private Cell targetCell;
    private PieceManager pieceManager;
   
    
    public virtual void Setup(Color teamColor, Color32 spriteColor, PieceManager pieceManager, string gameMode)
    {
        this.pieceManager = pieceManager;
        this.gameMode = gameMode;
        color = teamColor;
        GetComponent<Image>().color = spriteColor;
    }
    
    public void Place(Cell cell)
    {
        currentCell = cell;
        originalCell = cell;
        currentCell.CurrentPiece = this;

        transform.position = cell.transform.position;
        gameObject.SetActive(true);
    }
    
    public void Reset()
    {
        Place(originalCell);
    }

    protected virtual void CheckPathing() { }

    private void ShowCells()
    {
        foreach (var cell in highlightedCells)
            cell.OutlineImage.enabled = true;
    }

    private void ClearCells()
    {
        foreach (var cell in highlightedCells)
            cell.OutlineImage.enabled = false;

        highlightedCells.Clear();
    }
    
    private void Move()
    {
        currentCell.CurrentPiece = null;
        currentCell = targetCell;
        currentCell.CurrentPiece = this;

        transform.position = currentCell.transform.position;
        targetCell = null;
    }
    
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CheckPathing();
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;
        
        foreach (var cell in highlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.RectTransform, Input.mousePosition))
            {
                targetCell = cell;
                break;
            }

            targetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ClearCells();
        
        if (!targetCell)
        {
            transform.position = currentCell.gameObject.transform.position;
            return;
        }
        
        Move();
        pieceManager.SwitchSides(color);
    }
}
