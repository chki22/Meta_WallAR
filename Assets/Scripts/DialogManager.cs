using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우

public class DialogManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogText; // 대화를 표시할 텍스트
    [SerializeField] private Button nextButton;         // Next 버튼
    [SerializeField] private GameObject dialogPanel;    // 대화 패널 (선택적으로 숨기기/보이기)

    [Header("Dialog Settings")]
    [SerializeField] private string[] dialogLines;      // Inspector에서 설정할 대화 리스트

    private int currentLineIndex = 0; // 현재 대화 인덱스

    void Start()
    {
        // 초기화
        if (dialogLines == null || dialogLines.Length == 0)
        {
            Debug.LogError("DialogManager: 대화 내용이 설정되지 않았습니다.");
            return;
        }

        // 대화 패널 및 버튼 설정
        dialogPanel.SetActive(true); // 대화 패널 보이기
        dialogText.text = dialogLines[currentLineIndex]; // 첫 번째 대화 표시
    }

    public void DisplayNextDialog()
    {
        currentLineIndex++;

        // 대화가 끝났을 경우 처리
        if (currentLineIndex >= dialogLines.Length)
        {
            Debug.Log("대화가 끝났습니다.");
            dialogPanel.SetActive(false); // 대화 패널 숨기기
        }
        else
        {
            // 다음 대화 표시
            dialogText.text = dialogLines[currentLineIndex];
        }
    }
}
