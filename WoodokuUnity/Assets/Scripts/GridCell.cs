using UnityEngine;

public class GridCell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color previewColor;
    private bool isShowingPreview = false;
    
    public int row { get; private set; }
    public int col { get; private set; }
    private GridManager gridManager;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            // Create a simple square sprite
            spriteRenderer.sprite = CreateSquareSprite();
        }
    }
    
    public void Initialize(int row, int col, GridManager manager)
    {
        this.row = row;
        this.col = col;
        this.gridManager = manager;
        
        // Add a collider for mouse interaction
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }
    }
    
    public void SetColor(Color color)
    {
        originalColor = color;
        if (!isShowingPreview)
        {
            spriteRenderer.color = color;
        }
    }
    
    public void SetPreviewColor(Color color)
    {
        previewColor = color;
        isShowingPreview = true;
        spriteRenderer.color = color;
    }
    
    public void ClearPreview()
    {
        isShowingPreview = false;
        spriteRenderer.color = originalColor;
    }
    
    private Sprite CreateSquareSprite()
    {
        // Create a simple 1x1 white square texture
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);
    }
    
    void OnMouseEnter()
    {
        // Add hover effect if needed
    }
    
    void OnMouseExit()
    {
        // Remove hover effect if needed
    }
}