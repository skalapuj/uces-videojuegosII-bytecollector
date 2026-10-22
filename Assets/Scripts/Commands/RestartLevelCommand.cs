using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.Commands
{
    public class RestartLevelCommand : ICommand
    {
        public void Execute()
        {
            Time.timeScale = 1f; // Aseguramos restaurar el tiempo antes de recargar
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}