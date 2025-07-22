using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Setup")]
    public GameObject cellPrefab;
    public Transform gridParent;
    public float cellSize = 1f;
    public float cellSpacing = 0.1f;
    
    [Header("Colors")]
    public Color emptyCellColor = Color.white;
    public Color filledCellColor = Color.brown;
    public Color highlightColor = Color.yellow;
    public Color previewColor = new Color(1f, 1f, 0f, 0.5f);
    
    private GridCell[,] gridCells = new GridCell[9, 9];
    private bool[,] gridState = new bool[9, 9];
    
    public void InitializeGrid()
    {
        ClearGrid();
        CreateGridVisuals();
    }
    
    void CreateGridVisuals()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                Vector3 position = new Vector3(
                    col * (cellSize + cellSpacing),
                    -row * (cellSize + cellSpacing),
                    0
                );
                
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, gridParent);
                GridCell cell = cellObj.GetComponent<GridCell>();
                if (cell == null)
                {
                    cell = cellObj.AddComponent<GridCell>();
                }
                
                cell.Initialize(row, col, this);
                gridCells[row, col] = cell;
                UpdateCellVisual(row, col);
            }
        }
        
        // Center the grid
        Vector3 gridCenter = new Vector3(
            (8 * (cellSize + cellSpacing)) / 2f,
            -(8 * (cellSize + cellSpacing)) / 2f,
            0
        );
        gridParent.position = -gridCenter;
    }
    
    public bool CanPlaceBlock(WoodBlock block, int startRow, int startCol)
    {
        Vector2Int[] blockShape = block.GetShape();
        
        foreach (Vector2Int offset in blockShape)
        {
            int row = startRow + offset.y;
            int col = startCol + offset.x;
            
            if (row < 0 || row >= 9 || col < 0 || col >= 9 || gridState[row, col])
            {
                return false;
            }
        }
        
        return true;
    }
    
    public void PlaceBlock(WoodBlock block, int startRow, int startCol)
    {
        Vector2Int[] blockShape = block.GetShape();
        
        foreach (Vector2Int offset in blockShape)
        {
            int row = startRow + offset.y;
            int col = startCol + offset.x;
            
            gridState[row, col] = true;
            UpdateCellVisual(row, col);
        }
    }
    
    public void ShowBlockPreview(WoodBlock block, int startRow, int startCol, bool show)
    {
        if (block == null) return;
        
        Vector2Int[] blockShape = block.GetShape();
        
        foreach (Vector2Int offset in blockShape)
        {
            int row = startRow + offset.y;
            int col = startCol + offset.x;
            
            if (row >= 0 && row < 9 && col >= 0 && col < 9 && !gridState[row, col])
            {
                if (show)
                {
                    gridCells[row, col].SetPreviewColor(previewColor);
                }
                else
                {
                    gridCells[row, col].ClearPreview();
                }
            }
        }
    }
    
    public bool IsRowComplete(int row)
    {
        for (int col = 0; col < 9; col++)
        {
            if (!gridState[row, col])
                return false;
        }
        return true;
    }
    
    public bool IsColumnComplete(int col)
    {
        for (int row = 0; row < 9; row++)
        {
            if (!gridState[row, col])
                return false;
        }
        return true;
    }
    
    public bool IsBoxComplete(int boxRow, int boxCol)
    {
        int startRow = boxRow * 3;
        int startCol = boxCol * 3;
        
        for (int row = startRow; row < startRow + 3; row++)
        {
            for (int col = startCol; col < startCol + 3; col++)
            {
                if (!gridState[row, col])
                    return false;
            }
        }
        return true;
    }
    
    public void ClearCompletedLines(List<int> rows, List<int> cols, List<Vector2Int> boxes)
    {
        // Clear completed rows
        foreach (int row in rows)
        {
            for (int col = 0; col < 9; col++)
            {
                gridState[row, col] = false;
            }
        }
        
        // Clear completed columns
        foreach (int col in cols)
        {
            for (int row = 0; row < 9; row++)
            {
                gridState[row, col] = false;
            }
        }
        
        // Clear completed boxes
        foreach (Vector2Int box in boxes)
        {
            int startRow = box.x * 3;
            int startCol = box.y * 3;
            
            for (int row = startRow; row < startRow + 3; row++)
            {
                for (int col = startCol; col < startCol + 3; col++)
                {
                    gridState[row, col] = false;
                }
            }
        }
        
        // Update visuals
        UpdateAllCellVisuals();
    }
    
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector3 localPos = gridParent.InverseTransformPoint(worldPosition);
        
        int col = Mathf.RoundToInt(localPos.x / (cellSize + cellSpacing));
        int row = Mathf.RoundToInt(-localPos.y / (cellSize + cellSpacing));
        
        return new Vector2Int(row, col);
    }
    
    void ClearGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                gridState[row, col] = false;
            }
        }
    }
    
    void UpdateCellVisual(int row, int col)
    {
        if (gridCells[row, col] != null)
        {
            Color cellColor = gridState[row, col] ? filledCellColor : emptyCellColor;
            gridCells[row, col].SetColor(cellColor);
        }
    }
    
    void UpdateAllCellVisuals()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                UpdateCellVisual(row, col);
            }
        }
    }
}