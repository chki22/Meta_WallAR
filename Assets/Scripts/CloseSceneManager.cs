using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CloseSceneManager : MonoBehaviour
{
    public Button closeButton; // �ݱ� ��ư

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() => LoadScene("TitleScene"));
    }

    void LoadScene(string sceneName)
    {
        // Ư�� ������ �̵�
        SceneManager.LoadScene(sceneName);
    }
}
