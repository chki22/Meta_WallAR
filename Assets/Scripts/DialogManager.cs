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
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialog()
    {
        // ��ȭ ���� ó��
        Debug.Log("��ȭ ����");
        nextButton.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "InitStory")
        {
            SceneManager.LoadScene("DualImageTracking");
        }

        else if (SceneManager.GetActiveScene().name == "PreCycleStory")
        {
            SceneManager.LoadScene("BicycleGame");
        }

        else if (SceneManager.GetActiveScene().name == "AfterCycleStory")
        {
            SceneManager.LoadScene("DualImageTracking");
        }

        else if (SceneManager.GetActiveScene().name == "PreCatStory")
        {
            SceneManager.LoadScene("CatDropGame");
        }

        else if (SceneManager.GetActiveScene().name == "AfterCatStory")
        {
            SceneManager.LoadScene("DualImageTracking");
        }

        else if (SceneManager.GetActiveScene().name == "PrePlantStory")
        {
            SceneManager.LoadScene("HolePlantGame");
        }

        else if (SceneManager.GetActiveScene().name == "AfterPlantStory")
        {
            SceneManager.LoadScene("DualImageTracking");
        }

        else if (SceneManager.GetActiveScene().name == "PreGiraffeStory")
        {
            SceneManager.LoadScene("GiraffeGame");
        }

        else if (SceneManager.GetActiveScene().name == "AfterGiraffeStory")
        {
            SceneManager.LoadScene("FinalStory");
        }

        else if (SceneManager.GetActiveScene().name == "FinalStory")
        {
            SceneManager.LoadScene("DualImageTracking");
        }

    }
}
