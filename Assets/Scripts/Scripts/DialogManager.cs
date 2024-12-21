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

    void OnEnable()
    {
        // 씬 로드 이벤트 연결
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 씬 로드 이벤트 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");
        InitializeDialogManager();
    }

    void InitializeDialogManager()
    {
        currentDialogIndex = 0;

        // 버튼 활성화
        nextButton.gameObject.SetActive(true);

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
        else if (SceneManager.GetActiveScene().name == "AfterGiraffeStory1" && currentDialogIndex >= 7 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("이미지가 비활성화되었습니다.");
        }
        else if (SceneManager.GetActiveScene().name == "AfterGiraffeStory2" && currentDialogIndex >= 0 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("이미지가 비활성화되었습니다.");
        }
        else if (SceneManager.GetActiveScene().name == "FinalStory" && currentDialogIndex >= 1 && currentDialogIndex <= 20  && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(true);
            Debug.Log("이미지가 활성화되었습니다.");
        }
        else if (SceneManager.GetActiveScene().name == "FinalStory" && currentDialogIndex >= 21 && yeonhoImage != null)
        {
            yeonhoImage.gameObject.SetActive(false);
            Debug.Log("이미지가 비활성화되었습니다.");
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
        // 대화 종료 처리
        Debug.Log("대화 종료");
        nextButton.gameObject.SetActive(false);

        // 씬 전환 로직
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
