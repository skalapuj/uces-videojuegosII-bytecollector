using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Pantalla de Game Over con Reintentar y Volver al menú.
/// IMPORTANTE: este script va en un objeto que SIEMPRE esté activo (ej: el HUD).
/// El panel que se muestra/oculta es 'panelRoot', un objeto distinto.
/// Los botones se conectan por código (mismo criterio que LoadSceneButton en el menú).
/// </summary>
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private string menuSceneName = "01_MainMenu";

    private void Awake()
    {
        panelRoot.SetActive(false);
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(GoToMenu);
    }

    private void OnEnable()
    {
        gameManager.GameEnded += Show;
    }

    private void OnDisable()
    {
        if (gameManager != null) gameManager.GameEnded -= Show;
    }

    private void Show(int finalScore)
    {
        finalScoreText.text = $"PUNTAJE FINAL: {finalScore}";
        panelRoot.SetActive(true);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
