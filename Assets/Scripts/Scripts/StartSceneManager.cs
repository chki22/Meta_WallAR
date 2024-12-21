using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton; // Unity �����Ϳ��� ��ư�� ����
    public Button infoButton;  // Unity �����Ϳ��� ��ư�� ����

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ �߰�
        startButton.onClick.AddListener(() => LoadScene("InitStory"));
        infoButton.onClick.AddListener(() => LoadScene("InfoScene"));
    }

    void LoadScene(string sceneName)
    {
        // Ư�� ������ �̵�
        SceneManager.LoadScene(sceneName);
    }
}
