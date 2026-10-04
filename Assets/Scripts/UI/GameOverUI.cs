using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.UI
{
    public class GameOverUI : MonoBehaviour
    {
        public void RestartGame()
        {
            Time.timeScale = 1f; // Restaurar velocidad del tiempo antes de recargar
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("01_MainMenu");
        }
    }
}