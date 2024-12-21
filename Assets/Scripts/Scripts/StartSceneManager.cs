using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton; // Unity 에디터에서 버튼을 연결
    public Button infoButton;  // Unity 에디터에서 버튼을 연결

    void Start()
    {
        // 버튼 클릭 이벤트 추가
        startButton.onClick.AddListener(() => LoadScene("InitStory"));
        infoButton.onClick.AddListener(() => LoadScene("InfoScene"));
    }

    void LoadScene(string sceneName)
    {
        // 특정 씬으로 이동
        SceneManager.LoadScene(sceneName);
    }
}
