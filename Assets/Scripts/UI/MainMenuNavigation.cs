using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    // Carga la escena de juego principal
    public void GoToGameplay()
    {
        SceneManager.LoadScene("02_Gameplay");
    }

    // Carga la escena de créditos
    public void GoToCredits()
    {
        SceneManager.LoadScene("03_Credits");
    }

    // Carga la escena de opciones
    public void GoToOptions()
    {
        SceneManager.LoadScene("04_Options");
    }

    // Cierra el juego (funciona en la build compilada)
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}