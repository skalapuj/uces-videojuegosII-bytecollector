using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenLoader : MonoBehaviour
{
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private string nextSceneName = "01_MainMenu";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextSceneRoutine());
    }
    private IEnumerator LoadNextSceneRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
