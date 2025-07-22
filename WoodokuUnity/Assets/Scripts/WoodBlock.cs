using UnityEngine;

public enum BlockType
{
    Single,
    Line2,
    Line3,
    Line4,
    Line5,
    LShape,
    TShape,
    Square2x2,
    Square3x3,
    Corner
}

public class WoodBlock : MonoBehaviour
{
    [Header("Block Settings")]
    public BlockType blockType;
    public Color blockColor = new Color(0.6f, 0.3f, 0.1f); // Wood color
    
    private Vector2Int[] shape;
    private SpriteRenderer[] blockCells;
    private bool isDragging = false;
    private bool isPlaced = false;
    private Vector3 originalPosition;
    private Camera mainCamera;
    private GridManager gridManager;
    private BlockSpawner blockSpawner;
    
    void Start()
    {
        mainCamera = Camera.main;
        gridManager = FindObjectOfType<GridManager>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
        originalPosition = transform.position;
        
        InitializeShape();
        CreateVisualBlock();
    }
    
    void InitializeShape()
    {
        switch (blockType)
        {
            case BlockType.Single:
                shape = new Vector2Int[] { new Vector2Int(0, 0) };
                break;
            case BlockType.Line2:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0) };
                break;
            case BlockType.Line3:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) };
                break;
            case BlockType.Line4:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) };
                break;
            case BlockType.Line5:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(4, 0) };
                break;
            case BlockType.LShape:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 0) };
                break;
            case BlockType.TShape:
                shape = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2) };
                break;
            case BlockType.Square2x2:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1) };
                break;
            case BlockType.Square3x3:
                shape = new Vector2Int[] { 
                    new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
                    new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
                    new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2)
                };
                break;
            case BlockType.Corner:
                shape = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0) };
                break;
        }
    }
    
    void CreateVisualBlock()
    {
        blockCells = new SpriteRenderer[shape.Length];
        
        for (int i = 0; i < shape.Length; i++)
        {
            GameObject cellObj = new GameObject("BlockCell_" + i);
            cellObj.transform.SetParent(transform);
            cellObj.transform.localPosition = new Vector3(shape[i].x, -shape[i].y, 0);
            
            SpriteRenderer renderer = cellObj.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateSquareSprite();
            renderer.color = blockColor;
            renderer.sortingOrder = 10;
            
            blockCells[i] = renderer;
        }
        
        // Add collider for the entire block
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        Vector2 size = GetBlockBounds();
        collider.size = size;
        collider.offset = new Vector2(size.x / 2f - 0.5f, -size.y / 2f + 0.5f);
    }
    
    Vector2 GetBlockBounds()
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;
        
        foreach (Vector2Int cell in shape)
        {
            minX = Mathf.Min(minX, cell.x);
            maxX = Mathf.Max(maxX, cell.x);
            minY = Mathf.Min(minY, cell.y);
            maxY = Mathf.Max(maxY, cell.y);
        }
        
        return new Vector2(maxX - minX + 1, maxY - minY + 1);
    }
    
    public Vector2Int[] GetShape()
    {
        return shape;
    }
    
    public int GetBlockSize()
    {
        return shape.Length;
    }
    
    void OnMouseDown()
    {
        if (isPlaced) return;
        
        isDragging = true;
        // Bring block to front
        foreach (var cell in blockCells)
        {
            cell.sortingOrder = 20;
        }
    }
    
    void OnMouseDrag()
    {
        if (!isDragging || isPlaced) return;
        
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
        
        // Show preview on grid
        if (gridManager != null)
        {
            Vector2Int gridPos = gridManager.GetGridPosition(mousePos);
            gridManager.ShowBlockPreview(this, gridPos.x, gridPos.y, true);
        }
    }
    
    void OnMouseUp()
    {
        if (!isDragging || isPlaced) return;
        
        isDragging = false;
        
        // Clear preview
        if (gridManager != null)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = gridManager.GetGridPosition(mousePos);
            gridManager.ShowBlockPreview(this, gridPos.x, gridPos.y, false);
            
            // Try to place block
            if (gridManager.CanPlaceBlock(this, gridPos.x, gridPos.y))
            {
                PlaceBlock(gridPos.x, gridPos.y);
            }
            else
            {
                // Return to original position
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }
    
    void PlaceBlock(int row, int col)
    {
        isPlaced = true;
        gridManager.PlaceBlock(this, row, col);
        
        // Hide the draggable block
        gameObject.SetActive(false);
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBlockPlaced(shape.Length);
        }
        
        // Notify block spawner
        if (blockSpawner != null)
        {
            blockSpawner.OnBlockPlaced(this);
        }
    }
    
    void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        
        // Reset sorting order
        foreach (var cell in blockCells)
        {
            cell.sortingOrder = 10;
        }
    }
    
    public void ResetBlock()
    {
        isPlaced = false;
        isDragging = false;
        gameObject.SetActive(true);
        ReturnToOriginalPosition();
    }
    
    private Sprite CreateSquareSprite()
    {
        // Create a simple 1x1 white square texture
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);
    }
}