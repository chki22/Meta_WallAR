using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CloseSceneManager : MonoBehaviour
{
    public Button closeButton; // 닫기 버튼

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() => LoadScene("TitleScene"));
    }

    void LoadScene(string sceneName)
    {
        // 특정 씬으로 이동
        SceneManager.LoadScene(sceneName);
    }
}
