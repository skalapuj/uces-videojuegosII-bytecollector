using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Paneles de Navegación")]
        [SerializeField] private GameObject panelMain;
        [SerializeField] private GameObject panelOptions;
        [SerializeField] private GameObject panelCredits;

        [Header("Configuración de Escena")]
        [SerializeField] private string gameplaySceneName = "03_Gameplay";

        // Start is called before the first frame update
        void Start()
        {
            // Estado inicial garantizado: Menú principal visible, submenús ocultos
            ShowMainMenu();
        }

        #region Métodos de Navegación de Escenas

        public void PlayGame()
        {
            SceneManager.LoadScene(gameplaySceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion

        #region Métodos de Transición de Paneles

        public void ShowMainMenu()
        {
            SetPanelState(main: true, options: false, credits: false);
        }

        public void ShowOptions()
        {
            SetPanelState(main: false, options: true, credits: false);
        }

        public void ShowCredits()
        {
            SetPanelState(main: false, options: false, credits: true);
        }

        private void SetPanelState(bool main, bool options, bool credits)
        {
            if (panelMain != null) panelMain.SetActive(main);
            if (panelOptions != null) panelOptions.SetActive(options);
            if (panelCredits != null) panelCredits.SetActive(credits);
        }

        #endregion

        // Update is called once per frame
        void Update()
        {

        }
    }
}
