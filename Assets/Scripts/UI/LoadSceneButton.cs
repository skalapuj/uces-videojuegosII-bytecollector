using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Awake() =>
        GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.Load(sceneName));
}