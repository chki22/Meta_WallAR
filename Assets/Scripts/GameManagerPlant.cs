using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

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
        // UI 배치
        scoreText.rectTransform.anchorMin = new Vector2(0, 0);
        scoreText.rectTransform.anchorMax = new Vector2(0, 0);
        scoreText.rectTransform.pivot = new Vector2(0, 0);
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10);

        trapClickText.rectTransform.anchorMin = new Vector2(1, 0);
        trapClickText.rectTransform.anchorMax = new Vector2(1, 0);
        trapClickText.rectTransform.pivot = new Vector2(1, 0);
        trapClickText.rectTransform.anchoredPosition = new Vector2(-10, 10);

        timerText.rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 1.0f);
        timerText.rectTransform.anchoredPosition = new Vector2(0, -50);

        retryButton.gameObject.SetActive(false);
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        retryButton.onClick.AddListener(RestartGame);

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
            // 랜덤으로 1~2개의 오브젝트 활성화
            int numberOfObjectsToShow = Random.Range(1, 3); // 1~2개의 오브젝트
            List<GameObject> selectedObjects = new List<GameObject>();

            for (int i = 0; i < numberOfObjectsToShow; i++)
            {
                GameObject obj;
                do
                {
                    obj = hidingObjects[Random.Range(0, hidingObjects.Length)];
                } while (selectedObjects.Contains(obj)); // 중복 방지

                selectedObjects.Add(obj);
                var hidingObj = obj.GetComponent<HidingObject>();
                hidingObj.isTrap = Random.value < 0.2f; // 20% 확률로 함정 설정
                hidingObj.UpdateColor(); // 색상 업데이트
                obj.SetActive(true); // 활성화
            }

            // 오브젝트 유지 시간
            yield return new WaitForSeconds(appearDuration);

            // 모든 오브젝트 비활성화
            foreach (var obj in selectedObjects)
            {
                obj.SetActive(false);
            }

            // 다음 라운드 대기 시간
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnObjectClicked(GameObject obj)
    {
        if (!gameActive) return;

        var hidingObj = obj.GetComponent<HidingObject>();

        if (hidingObj.isTrap)
        {
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
            score++;
        }

        obj.SetActive(false);
        UpdateUI();

        if (score >= 10)
        {
            isWin = true;
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = $"{Mathf.CeilToInt(remainingTime)}";
        trapClickText.text = "Lives: " + trapClickCount;
    }

    void EndGame()
    {
        if (isWin)
        {
            SceneManager.LoadScene("DualImageTracking");
            Debug.Log("You Win!");
        }
        else
        {
            retryButton.gameObject.SetActive(true);
            Debug.Log("Game Over!");
        }

        gameActive = false;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("HolePlantGame");
    }
}
