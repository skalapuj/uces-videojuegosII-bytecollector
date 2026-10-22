using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.Commands
{
    public class ReturnToMenuCommand : ICommand
    {
        private const string MAIN_MENU_SCENE = "01_MainMenu";

        public void Execute()
        {
            Time.timeScale = 1f; // Restauramos tiempo antes de cambiar de escena
            SceneManager.LoadScene(MAIN_MENU_SCENE);
        }
    }
}