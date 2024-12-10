using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϴ� ���

public class DialogManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogText; // ��ȭ�� ǥ���� �ؽ�Ʈ
    [SerializeField] private Button nextButton;         // Next ��ư
    [SerializeField] private GameObject dialogPanel;    // ��ȭ �г� (���������� �����/���̱�)

    [Header("Dialog Settings")]
    [SerializeField] private string[] dialogLines;      // Inspector���� ������ ��ȭ ����Ʈ

    private int currentLineIndex = 0; // ���� ��ȭ �ε���

    void Start()
    {
        // �ʱ�ȭ
        if (dialogLines == null || dialogLines.Length == 0)
        {
            Debug.LogError("DialogManager: ��ȭ ������ �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��ȭ �г� �� ��ư ����
        dialogPanel.SetActive(true); // ��ȭ �г� ���̱�
        dialogText.text = dialogLines[currentLineIndex]; // ù ��° ��ȭ ǥ��
    }

    public void DisplayNextDialog()
    {
        currentLineIndex++;

        // ��ȭ�� ������ ��� ó��
        if (currentLineIndex >= dialogLines.Length)
        {
            Debug.Log("��ȭ�� �������ϴ�.");
            dialogPanel.SetActive(false); // ��ȭ �г� �����
        }
        else
        {
            // ���� ��ȭ ǥ��
            dialogText.text = dialogLines[currentLineIndex];
        }
    }
}
