using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerGiraffe : MonoBehaviour
{
    public static GameManagerGiraffe Instance;
    [Header("Timer Settings")]
    public float timeLimit = 30f;          // ���� �ð�
    public TextMeshProUGUI timerText;
    public Button retryButton;// UI�� ǥ���� Ÿ�̸� �ؽ�Ʈ

    [Header("Images to Grow")]
    public RectTransform image1;
    public RectTransform image2;
    public RectTransform image3;

    [Header("Target Image (for Distance Check)")]
    public RectTransform targetImage;

    [Header("Growth Speeds")]
    [Tooltip("�⺻ ���� �ӵ�(�ʴ� �ȼ� ������)")]
    public float growthSpeed1 = 50f;
    public float growthSpeed2 = 60f;
    public float growthSpeed3 = 70f;

    [Header("Double Speed Range")]
    [Tooltip("Ÿ�� �̹������� �Ÿ�(2D) <= xRange �� ���� �ӵ� 2��")]
    public float xRange = 50f;

    [Header("Max Length Difference Ratio")]
    [Tooltip("3�� �̹��� ����(Height) ���̰� min������ 0.2�� �̻� �Ǹ� GameOver")]
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
        if (isGameOver) return;

        // 1) Ÿ�̸� ���� & üũ
        UpdateTimer();

        // 2) �� �̹������� ���� ���� �ӵ� ���
        float speed1 = GetCurrentSpeed(image1, growthSpeed1);
        float speed2 = GetCurrentSpeed(image2, growthSpeed2);
        float speed3 = GetCurrentSpeed(image3, growthSpeed3);

        // 3) ���� ����(Height) �ø���
        GrowImageHeight(image1, speed1);
        GrowImageHeight(image2, speed2);
        GrowImageHeight(image3, speed3);

        // 4) 3�� �̹��� �� ���� ���� Ȯ�� -> 0.2�� �̻��̸� GameOver
        CheckLengthDifference();
    }

    /// <summary>
    /// Ÿ�̸� ����. 0 ���Ϸ� �������� Clear ó��
    /// </summary>
    private void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // �Ҽ��� �ø��ؼ� ǥ��
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
    /// Ÿ�� �̹����� 2D �Ÿ� �� -> ������ 2�� �ӵ�, �ƴϸ� �⺻ �ӵ�
    /// </summary>
    private float GetCurrentSpeed(RectTransform img, float baseSpeed)
    {
        if (img == null || targetImage == null) return baseSpeed;

        // �� RectTransform�� anchoredPosition(2D) �Ÿ�
        float distance = Vector2.Distance(img.anchoredPosition, targetImage.anchoredPosition);
        if (distance <= xRange)
        {
            // ���� �������� �����ٸ� ���� �ӵ� 2��
            return baseSpeed * 2f;
        }
        else
        {
            return baseSpeed;
        }
    }

    /// <summary>
    /// RectTransform�� Height(sizeDelta.y)�� �� ������ speed��ŭ ����
    /// </summary>
    private void GrowImageHeight(RectTransform img, float speed)
    {
        if (img == null) return;

        Vector2 size = img.sizeDelta;
        size.y += speed * Time.deltaTime;
        img.sizeDelta = size;
    }

    /// <summary>
    /// 3�� �̹��� �� ���� ���̰� min���� * maxDiffRatio �̻��̸� GameOver
    /// </summary>
    private void CheckLengthDifference()
    {
        if (!image1 || !image2 || !image3) return;

        float h1 = image1.sizeDelta.y;
        float h2 = image2.sizeDelta.y;
        float h3 = image3.sizeDelta.y;

        float minH = Mathf.Min(h1, h2, h3);
        float maxH = Mathf.Max(h1, h2, h3);

        // ��: maxH - minH >= minH * 0.2f  -> ���̰� 20% �̻�
        if ((maxH - minH) >= minH * maxDiffRatio)
        {
            // Game Over
            GameOver();
        }
    }

    /// <summary>
    /// ���ӿ���: �ܼ� ��� �� �Ͻ����� (�г� ���X)
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        retryButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���� �ð��� �� �Ǹ� Clear ó��
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
    /// (�ʿ� ��) �� �ε� ����
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
