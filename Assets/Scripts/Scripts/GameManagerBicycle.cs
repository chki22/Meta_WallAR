using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.Management;

public class GameManagerBicycle : MonoBehaviour
{
    public static GameManagerBicycle Instance;

    [Header("Game Settings")]
    public int maxMissCount = 3;
    public float timeLimit = 30f;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public Button retryButton;

    [Header("Target Image (Note21)")]
    [Tooltip("노트21 이미지를 UI RectTransform으로 가정")]
    public RectTransform targetImage;

    [Header("Out of Bounds Settings")]
    [Tooltip("2초 이상 화면 밖에 있으면 GameOver")]
    public float outOfBoundsDuration = 2f; // 2초
    private float outOfBoundsTimer = 0f;

    // 노트 20 울트라 해상도(1440 x 3088) 기준으로 화면 경계를 직접 지정
    // 필요한 경우 Screen.width/Screen.height로 대체 가능
    private float screenLeft = 0f;
    private float screenRight = 1440f;
    private float screenBottom = 0f;
    private float screenTop = 3088f;

    // 화면 밖 판정을 위한 여유값 (너무 예민하게 끊기지 않도록)
    private float margin = 50f;

    private float remainingTime;

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
        UpdateTimer();
        CheckOutOfBoundsAndCountdown();
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

    /// <summary>
    /// 자전거(이미지)가 화면 밖인지 판정 후,
    /// - 2초 이상 밖에 있으면 GameOver
    /// - 다시 안으로 돌아오면 타이머 초기화
    /// </summary>
    private void CheckOutOfBoundsAndCountdown()
    {
        if (targetImage == null) return;

        Vector3[] corners = new Vector3[4];
        targetImage.GetWorldCorners(corners);

        // corners[0] (왼쪽 아래), corners[2] (오른쪽 위)
        bool isOutLeft = corners[2].x < (screenLeft - margin);
        bool isOutRight = corners[0].x > (screenRight + margin);
        bool isOutBottom = corners[2].y < (screenBottom - margin);
        bool isOutTop = corners[0].y > (screenTop + margin);

        bool isOutOfBounds = (isOutLeft || isOutRight || isOutBottom || isOutTop);

        if (isOutOfBounds)
        {
            // 화면 밖이면 outOfBoundsTimer 증가
            outOfBoundsTimer += Time.deltaTime;

            // outOfBoundsDuration(2초) 이상 유지되면 Game Over
            if (outOfBoundsTimer >= outOfBoundsDuration)
            {
                Debug.Log("2초 이상 화면 밖 -> GameOver");
                GameOver();
            }
        }
        else
        {
            // 화면 안으로 돌아오면 다시 타이머 초기화
            outOfBoundsTimer = 0f;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        retryButton.gameObject.SetActive(true);
    }

    private void Clear()
    {
        Debug.Log("Clear!");
        StartCoroutine(LoadSceneAsync("AfterCycleStory"));
        Time.timeScale = 0f;
    }

    private void StartTimer()
    {
        Time.timeScale = 1f;
    }

    private void RestartGame()
    {
        StartCoroutine(LoadSceneAsync("BicycleGame"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }
}
