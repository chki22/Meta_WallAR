using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject[] hidingObjects; // 숨는 오브젝트 배열
    public float appearDuration = 1.0f; // 오브젝트가 나타나는 시간
    public float gameTime = 30.0f; // 게임 시간
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trapClickText; // 상태 메시지를 표시할 Text (옵션)
    public Button retryButton; // 재시작 버튼

    private int score = 0;
    private float remainingTime;
    private bool gameActive = true;
    private int trapClickCount = 3; // 함정을 클릭 가능한 횟수

    bool isWin = false;

    void Start()
    {
        // Score Text: 왼쪽 아래 배치
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // 왼쪽 아래 기준
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // 화면 경계에서 10px 떨어짐

        // Lives Text: 오른쪽 아래 배치
        trapClickText.rectTransform.anchorMin = new Vector2(1, 0); // 오른쪽 아래
        trapClickText.rectTransform.anchorMax = new Vector2(1, 0); // 오른쪽 아래
        trapClickText.rectTransform.pivot = new Vector2(1, 0);     // 오른쪽 아래 기준
        trapClickText.rectTransform.anchoredPosition = new Vector2(-10, 10); // 화면 경계에서 10px 떨어짐

        // 화면 중앙 상단에 배치
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 1.0f); // 화면의 중앙 상단
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 1.0f); // 화면의 중앙 상단
        timerText.rectTransform.pivot = new Vector2(0.5f, 1.0f); // 중심 기준을 상단으로 설정
        timerText.rectTransform.anchoredPosition = new Vector2(0, -50); // 상단에서 아래로 50px 이동

        // Retry Button 초기 설정 (비활성화 및 화면 중앙 배치)
        retryButton.gameObject.SetActive(false); // 초기 상태에서 비활성화
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // 타이머 아래에 배치

        retryButton.onClick.AddListener(RestartGame); // 버튼 클릭 이벤트 추가

        remainingTime = gameTime;
        UpdateUI();
        StartCoroutine(SpawnHidingObjects());
    }

    void Update()
    {
        if (gameActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                EndGame();
            }
            UpdateUI();
        }
    }
    IEnumerator SpawnHidingObjects()
    {
        while (gameActive)
        {
            int randomIndex = Random.Range(0, hidingObjects.Length);
            GameObject obj = hidingObjects[randomIndex];

            // 함정인지 여부를 설정
            var hidingObj = obj.GetComponent<HidingObject>();
            hidingObj.isTrap = Random.value < 0.2f; // 20% 확률로 함정 설정
            hidingObj.UpdateColor(); // 색상 업데이트

            // 오브젝트 나타나기
            obj.SetActive(true);
            yield return new WaitForSeconds(appearDuration);

            // 오브젝트 숨기기
            obj.SetActive(false);
            yield return new WaitForSeconds(0.5f); // 다음 오브젝트 등장 전 대기
        }
    }


    public void OnObjectClicked(GameObject obj)
    {
        if (!gameActive) return;

        var hidingObj = obj.GetComponent<HidingObject>();

        if (hidingObj.isTrap)
        {
            // 함정을 클릭한 경우
            if (score >= 2)
            {
                score -= 2;
            }
            trapClickCount--;
            if (trapClickCount <= 0)
            {
                isWin = false;
                EndGame();
                return;
            }
        }
        else
        {
            // 일반 오브젝트를 클릭한 경우
            score++;
        }

        obj.SetActive(false); // 클릭한 오브젝트 숨김
        UpdateUI();

        if (score >= 10)
        {
            isWin = true;
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        trapClickText.text = "Lives: " + trapClickCount;
    }

    void EndGame()
    {
        gameActive = false;

        if (isWin)
        {
            Debug.Log("You Win!");
        }
        else
        {
            // Retry Button 활성화
            retryButton.gameObject.SetActive(true);
            Debug.Log("Game Over!");
        }
    }
    private void RestartGame()
    {
        SceneManager.LoadScene("HolePlantGame");
    }
}



