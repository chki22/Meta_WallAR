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
    [Tooltip("��Ʈ21 �̹����� UI RectTransform���� ����")]
    public RectTransform targetImage;

    [Header("Out of Bounds Settings")]
    [Tooltip("2�� �̻� ȭ�� �ۿ� ������ GameOver")]
    public float outOfBoundsDuration = 2f; // 2��
    private float outOfBoundsTimer = 0f;

    // ��Ʈ 20 ��Ʈ�� �ػ�(1440 x 3088) �������� ȭ�� ��踦 ���� ����
    // �ʿ��� ��� Screen.width/Screen.height�� ��ü ����
    private float screenLeft = 0f;
    private float screenRight = 1440f;
    private float screenBottom = 0f;
    private float screenTop = 3088f;

    // ȭ�� �� ������ ���� ������ (�ʹ� �����ϰ� ������ �ʵ���)
    private float margin = 50f;

    private float remainingTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingTime = timeLimit;

        // Ÿ�̸� �ؽ�Ʈ �߾� ��ġ
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchoredPosition = new Vector2(0, 0);

        // ����� ��ư �߾� ��ġ (�ʱ⿡ ��Ȱ��ȭ)
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
    /// ������(�̹���)�� ȭ�� ������ ���� ��,
    /// - 2�� �̻� �ۿ� ������ GameOver
    /// - �ٽ� ������ ���ƿ��� Ÿ�̸� �ʱ�ȭ
    /// </summary>
    private void CheckOutOfBoundsAndCountdown()
    {
        if (targetImage == null) return;

        Vector3[] corners = new Vector3[4];
        targetImage.GetWorldCorners(corners);

        // corners[0] (���� �Ʒ�), corners[2] (������ ��)
        bool isOutLeft = corners[2].x < (screenLeft - margin);
        bool isOutRight = corners[0].x > (screenRight + margin);
        bool isOutBottom = corners[2].y < (screenBottom - margin);
        bool isOutTop = corners[0].y > (screenTop + margin);

        bool isOutOfBounds = (isOutLeft || isOutRight || isOutBottom || isOutTop);

        if (isOutOfBounds)
        {
            // ȭ�� ���̸� outOfBoundsTimer ����
            outOfBoundsTimer += Time.deltaTime;

            // outOfBoundsDuration(2��) �̻� �����Ǹ� Game Over
            if (outOfBoundsTimer >= outOfBoundsDuration)
            {
                Debug.Log("2�� �̻� ȭ�� �� -> GameOver");
                GameOver();
            }
        }
        else
        {
            // ȭ�� ������ ���ƿ��� �ٽ� Ÿ�̸� �ʱ�ȭ
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
