using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByteCollector.UI
{
    public class SplashScreenLoader : MonoBehaviour
    {
        [SerializeField] private float waitTime = 3f;
        [SerializeField] private string nextSceneName = "01_MainMenu";

        private void Start()
        {
            StartCoroutine(LoadNextSceneRoutine());
        }

        private IEnumerator LoadNextSceneRoutine()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
