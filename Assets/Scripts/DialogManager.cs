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
        public string name; // 대화 이름
        [TextArea]
        public string text; // 대화 내용
    }

    public List<Dialog> dialogList; // 대화 리스트
    public TextMeshProUGUI nameText; // 이름 텍스트를 표시할 Text
    public TextMeshProUGUI dialogText; // 대화 내용을 표시할 Text
    public Button nextButton; // 다음 대화 버튼

    public Image yeonhoImage; // 특정 dialogIndex에서 활성화할 Image

    private int currentDialogIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public float typingSpeed = 0.05f; // 텍스트 출력 속도

    void Start()
    {
        // 버튼 클릭 이벤트 추가
        nextButton.onClick.AddListener(DisplayNextDialog);

        // 첫 번째 대화 표시
        if (dialogList.Count > 0)
        {
            DisplayDialog();
        }
    }

    void DisplayNextDialog()
    {
        if (isTyping)
        {
            // 현재 애니메이션 중이라면 즉시 전체 텍스트 표시
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogText.text = dialogList[currentDialogIndex].text;
            isTyping = false;
            return;
        }

        // 다음 대화로 이동
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

        // 이름 및 대화 내용 설정
        nameText.text = currentDialog.name;

        // 텍스트 출력 애니메이션 시작
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(currentDialog.text));

        // 특정 dialogIndex에서 이미지 활성화
        if (SceneManager.GetActiveScene().name == "InitStory" && currentDialogIndex >= 6 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(true);
            Debug.Log("이미지가 활성화되었습니다.");
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
        // 대화 종료 처리
        Debug.Log("대화 종료");
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
