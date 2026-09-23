using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    private void Awake() =>
        GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.Quit());
}