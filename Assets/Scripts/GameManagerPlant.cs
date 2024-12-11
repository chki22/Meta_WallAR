using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManagerPlant : MonoBehaviour
{
    public List<GameObject> upwardGrowingObjects; // 위로 늘어나는 오브젝트 리스트
    public List<GameObject> downwardGrowingObjects; // 아래로 늘어나는 오브젝트 리스트
    public RectTransform destroyArea; // 첫 번째 Destroy Area (상단)
    public RectTransform destroyArea2; // 두 번째 Destroy Area (하단)
    public float growSpeed = 50.0f; // 오브젝트가 늘어나는 속도 (pixels/second)
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI destroyCountText; // Destroy 횟수 표시
    public Button retryButton; // 재시작 버튼

    private int score = 0;
    private float remainingTime = 30.0f;
    private int destroyCount = 3; // 현재 Destroy 횟수
    private bool gameActive = true;
    private bool isWin = false;

    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        retryButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(RestartGame);

        // UI 설정
        SetUITextPositions();

        UpdateUI();
        StartCoroutine(SpawnHidingObjects());
    }

    void SetUITextPositions()
    {
        // Score Text: 왼쪽 아래 배치
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // 왼쪽 아래
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // 왼쪽 아래 기준
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // 화면 경계에서 10px 떨어짐

        // Lives Text: 오른쪽 아래 배치
        destroyCountText.rectTransform.anchorMin = new Vector2(1, 0); // 오른쪽 아래
        destroyCountText.rectTransform.anchorMax = new Vector2(1, 0); // 오른쪽 아래
        destroyCountText.rectTransform.pivot = new Vector2(1, 0);     // 오른쪽 아래 기준
        destroyCountText.rectTransform.anchoredPosition = new Vector2(-10, 10); // 화면 경계에서 10px 떨어짐

        // Timer Text: 화면 중앙 상단 배치
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 1.0f); // 중앙 상단
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 1.0f); // 중앙 상단
        timerText.rectTransform.pivot = new Vector2(0.5f, 1.0f);     // 중심 기준을 상단으로 설정
        timerText.rectTransform.anchoredPosition = new Vector2(0, -10); // 상단에서 아래로 50px 이동

        // Retry Button 초기 설정 (비활성화 및 화면 중앙 배치)
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // 중앙에 배치
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

            // 위로 늘어나는 오브젝트 랜덤 0~1개 선택
            if (upwardGrowingObjects.Count > 0)
            {
                int upwardCount = Random.Range(0, 2); // 0 또는 1
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

            // 아래로 늘어나는 오브젝트 랜덤 0~1개 선택
            if (downwardGrowingObjects.Count > 0)
            {
                int downwardCount = Random.Range(0, 2); // 0 또는 1
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

            // 위와 아래 모두 선택되지 않은 경우 강제로 하나 선택
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

            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f)); // 다음 오브젝트 대기
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
