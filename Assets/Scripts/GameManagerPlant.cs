using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManagerPlant : MonoBehaviour
{
    public List<GameObject> upwardGrowingObjects; // ���� �þ�� ������Ʈ ����Ʈ
    public List<GameObject> downwardGrowingObjects; // �Ʒ��� �þ�� ������Ʈ ����Ʈ
    public RectTransform destroyArea; // ù ��° Destroy Area (���)
    public RectTransform destroyArea2; // �� ��° Destroy Area (�ϴ�)
    public float growSpeed = 50.0f; // ������Ʈ�� �þ�� �ӵ� (pixels/second)
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI destroyCountText; // Destroy Ƚ�� ǥ��
    public Button retryButton; // ����� ��ư

    private int score = 0;
    private float remainingTime = 30.0f;
    private int destroyCount = 3; // ���� Destroy Ƚ��
    private bool gameActive = true;
    private bool isWin = false;

    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        retryButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(RestartGame);

        // UI ����
        SetUITextPositions();

        UpdateUI();
        StartCoroutine(SpawnHidingObjects());
    }

    void SetUITextPositions()
    {
        // Score Text: ���� �Ʒ� ��ġ
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // ���� �Ʒ� ����
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // ȭ�� ��迡�� 10px ������

        // Lives Text: ������ �Ʒ� ��ġ
        destroyCountText.rectTransform.anchorMin = new Vector2(1, 0); // ������ �Ʒ�
        destroyCountText.rectTransform.anchorMax = new Vector2(1, 0); // ������ �Ʒ�
        destroyCountText.rectTransform.pivot = new Vector2(1, 0);     // ������ �Ʒ� ����
        destroyCountText.rectTransform.anchoredPosition = new Vector2(-10, 10); // ȭ�� ��迡�� 10px ������

        // Timer Text: ȭ�� �߾� ��� ��ġ
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 1.0f); // �߾� ���
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 1.0f); // �߾� ���
        timerText.rectTransform.pivot = new Vector2(0.5f, 1.0f);     // �߽� ������ ������� ����
        timerText.rectTransform.anchoredPosition = new Vector2(0, -10); // ��ܿ��� �Ʒ��� 50px �̵�

        // Retry Button �ʱ� ���� (��Ȱ��ȭ �� ȭ�� �߾� ��ġ)
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // �߾ӿ� ��ġ
    }

    void Update()
    {
        if (gameActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                isWin = true;
                EndGame();
            }
            UpdateUI();
        }
    }

    IEnumerator SpawnHidingObjects()
    {
        while (gameActive)
        {
            bool upwardObjectSelected = false;
            bool downwardObjectSelected = false;

            // ���� �þ�� ������Ʈ ���� 0~1�� ����
            if (upwardGrowingObjects.Count > 0)
            {
                int upwardCount = Random.Range(0, 2); // 0 �Ǵ� 1
                for (int i = 0; i < upwardCount; i++)
                {
                    int randomIndex = Random.Range(0, upwardGrowingObjects.Count);
                    GameObject obj = upwardGrowingObjects[randomIndex];

                    if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                    {
                        StopCoroutine(activeCoroutines[obj]);
                    }

                    activeCoroutines[obj] = StartCoroutine(GrowObject(obj, true));
                    upwardObjectSelected = true;
                }
            }

            // �Ʒ��� �þ�� ������Ʈ ���� 0~1�� ����
            if (downwardGrowingObjects.Count > 0)
            {
                int downwardCount = Random.Range(0, 2); // 0 �Ǵ� 1
                for (int i = 0; i < downwardCount; i++)
                {
                    int randomIndex = Random.Range(0, downwardGrowingObjects.Count);
                    GameObject obj = downwardGrowingObjects[randomIndex];

                    if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                    {
                        StopCoroutine(activeCoroutines[obj]);
                    }

                    activeCoroutines[obj] = StartCoroutine(GrowObject(obj, false));
                    downwardObjectSelected = true;
                }
            }

            // ���� �Ʒ� ��� ���õ��� ���� ��� ������ �ϳ� ����
            if (!upwardObjectSelected && !downwardObjectSelected)
            {
                if (upwardGrowingObjects.Count > 0)
                {
                    int randomIndex = Random.Range(0, upwardGrowingObjects.Count);
                    GameObject obj = upwardGrowingObjects[randomIndex];

                    if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                    {
                        StopCoroutine(activeCoroutines[obj]);
                    }

                    activeCoroutines[obj] = StartCoroutine(GrowObject(obj, true));
                }
                else if (downwardGrowingObjects.Count > 0)
                {
                    int randomIndex = Random.Range(0, downwardGrowingObjects.Count);
                    GameObject obj = downwardGrowingObjects[randomIndex];

                    if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                    {
                        StopCoroutine(activeCoroutines[obj]);
                    }

                    activeCoroutines[obj] = StartCoroutine(GrowObject(obj, false));
                }
            }

            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f)); // ���� ������Ʈ ���
        }
    }


    IEnumerator GrowObject(GameObject obj, bool growUpward)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        while (gameActive)
        {
            if (growUpward)
            {
                rectTransform.sizeDelta += new Vector2(0, growSpeed * Time.deltaTime);
            }
            else
            {
                rectTransform.sizeDelta += new Vector2(0, growSpeed * Time.deltaTime);
            }

            if (IsOverlappingDestroyArea(rectTransform, growUpward))
            {
                destroyCount--;
                rectTransform.sizeDelta = new Vector2(282.1547f, 281.5374f);

                if (!growUpward)
                {
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
                }

                activeCoroutines[obj] = null;

                if (destroyCount <= 0)
                {
                    isWin = false;
                    EndGame();
                }
                yield break;
            }

            yield return null;
        }
    }

    public void OnObjectClicked(GameObject obj)
    {
        if (!gameActive) return;

        score++;

        if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
        {
            StopCoroutine(activeCoroutines[obj]);
            activeCoroutines[obj] = null;
        }

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(282.1547f, 281.5374f);

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = $"{Mathf.CeilToInt(remainingTime)}";
        destroyCountText.text = "Lives: " + destroyCount;
    }

    void EndGame()
    {
        gameActive = false;

        if (isWin)
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene("AfterPlantStory");
        }
        else
        {
            retryButton.gameObject.SetActive(true);
            Debug.Log("Game Over!");
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool IsOverlappingDestroyArea(RectTransform rectTransform, bool growUpward)
    {
        float destroyArea1Bottom = destroyArea.position.y;
        float destroyArea2Top = destroyArea2.position.y;

        float objectTop;
        float objectBottom;

        if (growUpward)
        {
            objectBottom = rectTransform.position.y;
            objectTop = rectTransform.position.y + rectTransform.sizeDelta.y;
        }
        else
        {
            objectTop = rectTransform.position.y;
            objectBottom = rectTransform.position.y - rectTransform.sizeDelta.y;
        }

        if (objectTop >= destroyArea1Bottom)
        {
            return true;
        }

        if (objectBottom <= destroyArea2Top)
        {
            return true;
        }

        return false;
    }
}
