using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerGiraffe : MonoBehaviour
{
    public static GameManagerGiraffe Instance;
    [Header("Timer Settings")]
    public float timeLimit = 30f;          // 제한 시간
    public TextMeshProUGUI timerText;
    public Button retryButton;// UI에 표시할 타이머 텍스트

    [Header("Images to Grow")]
    public RectTransform image1;
    public RectTransform image2;
    public RectTransform image3;

    [Header("Target Image (for Distance Check)")]
    public RectTransform targetImage;

    [Header("Growth Speeds")]
    [Tooltip("기본 성장 속도(초당 픽셀 증가량)")]
    public float growthSpeed1 = 50f;
    public float growthSpeed2 = 60f;
    public float growthSpeed3 = 70f;

    [Header("Double Speed Range")]
    [Tooltip("타겟 이미지와의 거리(2D) <= xRange 면 성장 속도 2배")]
    public float xRange = 50f;

    [Header("Max Length Difference Ratio")]
    [Tooltip("3개 이미지 길이(Height) 차이가 min길이의 0.2배 이상 되면 GameOver")]
    public float maxDiffRatio = 0.2f;

    private float remainingTime;
    private bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingTime = timeLimit;

        // 타이머 텍스트 중앙 배치
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchoredPosition = new Vector2(0, 0);

        // 재시작 버튼 중앙 배치 (초기에 비활성화)
        retryButton.gameObject.SetActive(false);
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);

        retryButton.onClick.AddListener(RestartGame);

        StartTimer();
    }
    private void Update()
    {
        if (isGameOver) return;

        // 1) 타이머 갱신 & 체크
        UpdateTimer();

        // 2) 각 이미지별로 현재 성장 속도 계산
        float speed1 = GetCurrentSpeed(image1, growthSpeed1);
        float speed2 = GetCurrentSpeed(image2, growthSpeed2);
        float speed3 = GetCurrentSpeed(image3, growthSpeed3);

        // 3) 실제 길이(Height) 늘리기
        GrowImageHeight(image1, speed1);
        GrowImageHeight(image2, speed2);
        GrowImageHeight(image3, speed3);

        // 4) 3개 이미지 간 길이 차이 확인 -> 0.2배 이상이면 GameOver
        CheckLengthDifference();
    }

    /// <summary>
    /// 타이머 갱신. 0 이하로 떨어지면 Clear 처리
    /// </summary>
    private void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // 소수점 올림해서 표시
            if (timerText != null)
                timerText.text = $"{Mathf.CeilToInt(remainingTime)}";

            if (remainingTime <= 0)
            {
                if (timerText != null)
                    timerText.text = "Clear!";
                Clear();
            }
        }
    }

    /// <summary>
    /// 타겟 이미지와 2D 거리 비교 -> 가까우면 2배 속도, 아니면 기본 속도
    /// </summary>
    private float GetCurrentSpeed(RectTransform img, float baseSpeed)
    {
        if (img == null || targetImage == null) return baseSpeed;

        // 두 RectTransform의 anchoredPosition(2D) 거리
        float distance = Vector2.Distance(img.anchoredPosition, targetImage.anchoredPosition);
        if (distance <= xRange)
        {
            // 일정 범위보다 가깝다면 성장 속도 2배
            return baseSpeed * 2f;
        }
        else
        {
            return baseSpeed;
        }
    }

    /// <summary>
    /// RectTransform의 Height(sizeDelta.y)를 매 프레임 speed만큼 증가
    /// </summary>
    private void GrowImageHeight(RectTransform img, float speed)
    {
        if (img == null) return;

        Vector2 size = img.sizeDelta;
        size.y += speed * Time.deltaTime;
        img.sizeDelta = size;
    }

    /// <summary>
    /// 3개 이미지 간 길이 차이가 min길이 * maxDiffRatio 이상이면 GameOver
    /// </summary>
    private void CheckLengthDifference()
    {
        if (!image1 || !image2 || !image3) return;

        float h1 = image1.sizeDelta.y;
        float h2 = image2.sizeDelta.y;
        float h3 = image3.sizeDelta.y;

        float minH = Mathf.Min(h1, h2, h3);
        float maxH = Mathf.Max(h1, h2, h3);

        // 예: maxH - minH >= minH * 0.2f  -> 차이가 20% 이상
        if ((maxH - minH) >= minH * maxDiffRatio)
        {
            // Game Over
            GameOver();
        }
    }

    /// <summary>
    /// 게임오버: 콘솔 출력 후 일시정지 (패널 사용X)
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        retryButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// 제한 시간이 다 되면 Clear 처리
    /// </summary>
    private void Clear()
    {
        isGameOver = true;
        Debug.Log("Clear! Timer ended.");
        Time.timeScale = 0f;


        StartCoroutine(LoadSceneAsync("AfterGiraffeStory1"));
   
    }
    private void RestartGame()
    {
        StartCoroutine(LoadSceneAsync("GiraffeGame"));
    }
    private void StartTimer()
    {
        Time.timeScale = 1f;
    }
    /// <summary>
    /// (필요 시) 씬 로드 예시
    /// </summary>
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
