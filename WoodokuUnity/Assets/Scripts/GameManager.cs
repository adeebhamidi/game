using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Board")]
    public GridManager gridManager;
    public BlockSpawner blockSpawner;
    
    [Header("UI Elements")]
    public Text scoreText;
    public Text bestScoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    
    [Header("Game Settings")]
    public int pointsPerBlock = 10;
    public int bonusPointsPerLine = 100;
    public int bonusPointsPerBox = 200;
    
    private int currentScore = 0;
    private int bestScore = 0;
    private bool isGameOver = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        LoadBestScore();
        InitializeGame();
        restartButton.onClick.AddListener(RestartGame);
    }
    
    void InitializeGame()
    {
        currentScore = 0;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        UpdateScoreUI();
        
        gridManager.InitializeGrid();
        blockSpawner.SpawnNewBlocks();
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;
        
        currentScore += points;
        UpdateScoreUI();
        
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveBestScore();
        }
    }
    
    public void OnBlockPlaced(int blocksPlaced)
    {
        AddScore(blocksPlaced * pointsPerBlock);
        CheckForCompletedLines();
        
        if (!blockSpawner.HasValidMoves())
        {
            GameOver();
        }
        else
        {
            blockSpawner.SpawnNewBlocks();
        }
    }
    
    void CheckForCompletedLines()
    {
        List<int> completedRows = new List<int>();
        List<int> completedCols = new List<int>();
        List<Vector2Int> completedBoxes = new List<Vector2Int>();
        
        // Check for completed rows
        for (int row = 0; row < 9; row++)
        {
            if (gridManager.IsRowComplete(row))
            {
                completedRows.Add(row);
            }
        }
        
        // Check for completed columns
        for (int col = 0; col < 9; col++)
        {
            if (gridManager.IsColumnComplete(col))
            {
                completedCols.Add(col);
            }
        }
        
        // Check for completed 3x3 boxes
        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                if (gridManager.IsBoxComplete(boxRow, boxCol))
                {
                    completedBoxes.Add(new Vector2Int(boxRow, boxCol));
                }
            }
        }
        
        // Clear completed lines and add bonus points
        if (completedRows.Count > 0 || completedCols.Count > 0 || completedBoxes.Count > 0)
        {
            gridManager.ClearCompletedLines(completedRows, completedCols, completedBoxes);
            
            int bonusPoints = (completedRows.Count + completedCols.Count) * bonusPointsPerLine + 
                            completedBoxes.Count * bonusPointsPerBox;
            AddScore(bonusPoints);
        }
    }
    
    void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }
    
    public void RestartGame()
    {
        InitializeGame();
    }
    
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore.ToString();
        bestScoreText.text = "Best: " + bestScore.ToString();
    }
    
    void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }
    
    void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    
    // Public methods for UI access
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public int GetBestScore()
    {
        return bestScore;
    }
}