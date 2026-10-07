using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.UI
{
    public class GameplayNavigation : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "01_MainMenu";

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f; // Restaurar la velocidad del juego al salir de la escena
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}