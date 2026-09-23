using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("SceneLoader");
                go.AddComponent<SceneLoader>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName) => SceneManager.LoadScene(sceneName);

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}