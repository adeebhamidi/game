using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Canvas gameCanvas;
    public Text scoreText;
    public Text bestScoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button quitButton;
    
    [Header("Game Over UI")]
    public Text finalScoreText;
    public Text newBestText;
    
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
        
        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // Initialize UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        if (newBestText != null)
            newBestText.gameObject.SetActive(false);
    }
    
    void Update()
    {
        // Update score display if available
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.GetCurrentScore().ToString();
        }
        
        if (bestScoreText != null && gameManager != null)
        {
            bestScoreText.text = "Best: " + gameManager.GetBestScore().ToString();
        }
    }
    
    public void ShowGameOver(int finalScore, bool isNewBest)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + finalScore.ToString();
            
            if (newBestText != null && isNewBest)
                newBestText.gameObject.SetActive(true);
        }
    }
    
    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            
            if (newBestText != null)
                newBestText.gameObject.SetActive(false);
        }
    }
    
    void RestartGame()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
            HideGameOver();
        }
    }
    
    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Method to create UI elements programmatically if needed
    public void CreateBasicUI()
    {
        // This method can be expanded to create UI elements at runtime
        // if no UI Canvas is set up in the scene
        
        if (gameCanvas == null)
        {
            GameObject canvasObj = new GameObject("GameCanvas");
            gameCanvas = canvasObj.AddComponent<Canvas>();
            gameCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Add CanvasScaler for responsive UI
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Add GraphicRaycaster for UI interaction
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        CreateScoreTexts();
        CreateGameOverPanel();
    }
    
    void CreateScoreTexts()
    {
        // Create score text
        if (scoreText == null)
        {
            GameObject scoreObj = new GameObject("ScoreText");
            scoreObj.transform.SetParent(gameCanvas.transform);
            
            scoreText = scoreObj.AddComponent<Text>();
            scoreText.text = "Score: 0";
            scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            
            RectTransform scoreRect = scoreText.GetComponent<RectTransform>();
            scoreRect.anchorMin = new Vector2(0, 1);
            scoreRect.anchorMax = new Vector2(0, 1);
            scoreRect.anchoredPosition = new Vector2(100, -50);
        }
        
        // Create best score text
        if (bestScoreText == null)
        {
            GameObject bestObj = new GameObject("BestScoreText");
            bestObj.transform.SetParent(gameCanvas.transform);
            
            bestScoreText = bestObj.AddComponent<Text>();
            bestScoreText.text = "Best: 0";
            bestScoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            bestScoreText.fontSize = 24;
            bestScoreText.color = Color.yellow;
            
            RectTransform bestRect = bestScoreText.GetComponent<RectTransform>();
            bestRect.anchorMin = new Vector2(0, 1);
            bestRect.anchorMax = new Vector2(0, 1);
            bestRect.anchoredPosition = new Vector2(100, -100);
        }
    }
    
    void CreateGameOverPanel()
    {
        if (gameOverPanel == null)
        {
            // Create game over panel
            gameOverPanel = new GameObject("GameOverPanel");
            gameOverPanel.transform.SetParent(gameCanvas.transform);
            
            Image panelImage = gameOverPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Create restart button
            GameObject buttonObj = new GameObject("RestartButton");
            buttonObj.transform.SetParent(gameOverPanel.transform);
            
            restartButton = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.green;
            
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(200, 60);
            buttonRect.anchoredPosition = Vector2.zero;
            
            // Add button text
            GameObject buttonTextObj = new GameObject("ButtonText");
            buttonTextObj.transform.SetParent(buttonObj.transform);
            
            Text buttonText = buttonTextObj.AddComponent<Text>();
            buttonText.text = "Restart";
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 18;
            buttonText.color = Color.white;
            buttonText.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = buttonText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            gameOverPanel.SetActive(false);
        }
    }
}