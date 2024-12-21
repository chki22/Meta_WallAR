using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialog
    {
        public string name; // ��ȭ �̸�
        [TextArea]
        public string text; // ��ȭ ����
    }

    public List<Dialog> dialogList; // ��ȭ ����Ʈ
    public TextMeshProUGUI nameText; // �̸� �ؽ�Ʈ�� ǥ���� Text
    public TextMeshProUGUI dialogText; // ��ȭ ������ ǥ���� Text
    public Button nextButton; // ���� ��ȭ ��ư

    public Image yeonhoImage; // Ư�� dialogIndex���� Ȱ��ȭ�� Image

    private int currentDialogIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public float typingSpeed = 0.05f; // �ؽ�Ʈ ��� �ӵ�

    void OnEnable()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ �߰�
        nextButton.onClick.AddListener(DisplayNextDialog);

        // ù ��° ��ȭ ǥ��
        if (dialogList.Count > 0)
        {
            DisplayDialog();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");
        InitializeDialogManager();
    }

    void InitializeDialogManager()
    {
        currentDialogIndex = 0;

        // ��ư Ȱ��ȭ
        nextButton.gameObject.SetActive(true);

        // ù ��° ��ȭ ǥ��
        if (dialogList.Count > 0)
        {
            DisplayDialog();
        }
    }

    void DisplayNextDialog()
    {
        if (isTyping)
        {
            // ���� �ִϸ��̼� ���̶�� ��� ��ü �ؽ�Ʈ ǥ��
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogText.text = dialogList[currentDialogIndex].text;
            isTyping = false;
            return;
        }

        // ���� ��ȭ�� �̵�
        currentDialogIndex++;
        if (currentDialogIndex < dialogList.Count)
        {
            DisplayDialog();
        }
        else
        {
            EndDialog();
        }
    }

    void DisplayDialog()
    {
        Dialog currentDialog = dialogList[currentDialogIndex];

        // �̸� �� ��ȭ ���� ����
        nameText.text = currentDialog.name;

        // �ؽ�Ʈ ��� �ִϸ��̼� ����
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(currentDialog.text));

        // Ư�� dialogIndex���� �̹��� Ȱ��ȭ
        if (SceneManager.GetActiveScene().name == "InitStory" && currentDialogIndex >= 6 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(true);
            Debug.Log("�̹����� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else if (SceneManager.GetActiveScene().name == "AfterGiraffeStory1" && currentDialogIndex >= 7 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("�̹����� ��Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else if (SceneManager.GetActiveScene().name == "AfterGiraffeStory2" && currentDialogIndex >= 0 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("�̹����� ��Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else if (SceneManager.GetActiveScene().name == "FinalStory" && currentDialogIndex >= 1 && currentDialogIndex <= 20  && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(true);
            Debug.Log("�̹����� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else if (SceneManager.GetActiveScene().name == "FinalStory" && currentDialogIndex >= 21 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("�̹����� ��Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else if (yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
        }
    }

    IEnumerator TypeText(string text)
    {
        Debug.Log($"Typing text: {text}");
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        Debug.Log("Typing completed.");
    }

    void EndDialog()
    {
        // ��ȭ ���� ó��
        Debug.Log("��ȭ ����");
        nextButton.gameObject.SetActive(false);

        // �� ��ȯ ����
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "InitStory":
                SceneManager.LoadScene("DualImageTracking");
                break;
            case "PreCycleStory":
                SceneManager.LoadScene("BicycleGame");
                break;
            case "AfterCycleStory":
                SceneManager.LoadScene("DualImageTracking");
                break;
            case "PreCatStory":
                SceneManager.LoadScene("CatDropGame");
                break;
            case "AfterCatStory":
                SceneManager.LoadScene("DualImageTracking");
                break;
            case "PrePlantStory":
                SceneManager.LoadScene("HolePlantGame");
                break;
            case "AfterPlantStory":
                SceneManager.LoadScene("DualImageTracking");
                break;
            case "PreGiraffeStory":
                SceneManager.LoadScene("GiraffeGame");
                break;
            case "AfterGiraffeStory1":
                SceneManager.LoadScene("AfterGiraffeStory2");
                break;
            case "AfterGiraffeStory2":
                SceneManager.LoadScene("FinalStory");
                break;
            case "FinalStory":
                SceneManager.LoadScene("TitleScene");
                break;
            default:
                Debug.LogWarning("Unknown scene name");
                break;
        }
    }
}
