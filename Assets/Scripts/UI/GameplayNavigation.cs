using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.UI
{
    public class GameplayNavigation : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "01_MainMenu";

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}