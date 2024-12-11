using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerCat : MonoBehaviour
{
    public static GameManagerCat Instance;

    public int maxMissCount = 3; // 최대 실패 횟수
    public TextMeshProUGUI scoreText; // 점수 표시
    public TextMeshProUGUI livesText; // 남은 실패 횟수 표시
    public TextMeshProUGUI timerText; // 타이머 표시
    public float timeLimit = 30f; // 제한 시간 (초)
    public Button retryButton; // 재시작 버튼

    private int score = 0;
    private int missCount = 0;
    private float remainingTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingTime = timeLimit; // 초기 시간 설정

        // Score Text: 왼쪽 아래 배치
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // 왼쪽 아래 기준
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // 화면 경계에서 10px 떨어짐

        // Lives Text: 오른쪽 아래 배치
        livesText.rectTransform.anchorMin = new Vector2(1, 0); // 오른쪽 아래
        livesText.rectTransform.anchorMax = new Vector2(1, 0); // 오른쪽 아래
        livesText.rectTransform.pivot = new Vector2(1, 0);     // 오른쪽 아래 기준
        livesText.rectTransform.anchoredPosition = new Vector2(-10, 10); // 화면 경계에서 10px 떨어짐

        // Timer Text: 화면 중앙 배치
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 0.5f); // 중심 기준
        timerText.rectTransform.anchoredPosition = new Vector2(0, 0); // 중앙에 위치

        // Retry Button 초기 설정 (비활성화 및 화면 중앙 배치)
        retryButton.gameObject.SetActive(false); // 초기 상태에서 비활성화
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120); // 타이머 아래에 배치

        retryButton.onClick.AddListener(RestartGame); // 버튼 클릭 이벤트 추가

        UpdateUI();
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void OnObjectHit()
    {
        score++;
        UpdateUI();
    }

    public void OnObjectMissed()
    {
        missCount++;
        if (missCount >= maxMissCount)
        {
            timerText.text = "Game Over!";
            GameOver();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {maxMissCount - missCount}";
    }

    private void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = $"{Mathf.CeilToInt(remainingTime)}";

            if (remainingTime <= 0)
            {
                timerText.text = "Clear!";
                Clear();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // 게임 일시정지

        // Retry Button 활성화
        retryButton.gameObject.SetActive(true);
    }

    private void Clear()
    {
        Debug.Log("Clear!");
        SceneManager.LoadScene("AfterCatStory");
        Time.timeScale = 0f;
    }

    private void StartTimer()
    {
        Time.timeScale = 1f; // 타이머 시작
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("CatDropGame");
    }
}
