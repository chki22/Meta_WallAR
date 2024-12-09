using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject[] hidingObjects; // ���� ������Ʈ �迭
    public float appearDuration = 1.0f; // ������Ʈ�� ��Ÿ���� �ð�
    public float gameTime = 30.0f; // ���� �ð�
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trapClickText; // ���� �޽����� ǥ���� Text (�ɼ�)
    public Button retryButton; // ����� ��ư

    private int score = 0;
    private float remainingTime;
    private bool gameActive = true;
    private int trapClickCount = 3; // ������ Ŭ�� ������ Ƚ��

    bool isWin = false;

    void Start()
    {
        // UI ��ġ
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
            // �������� 1~2���� ������Ʈ Ȱ��ȭ
            int numberOfObjectsToShow = Random.Range(1, 3); // 1~2���� ������Ʈ
            List<GameObject> selectedObjects = new List<GameObject>();

            for (int i = 0; i < numberOfObjectsToShow; i++)
            {
                GameObject obj;
                do
                {
                    obj = hidingObjects[Random.Range(0, hidingObjects.Length)];
                } while (selectedObjects.Contains(obj)); // �ߺ� ����

                selectedObjects.Add(obj);
                var hidingObj = obj.GetComponent<HidingObject>();
                hidingObj.isTrap = Random.value < 0.2f; // 20% Ȯ���� ���� ����
                hidingObj.UpdateColor(); // ���� ������Ʈ
                obj.SetActive(true); // Ȱ��ȭ
            }

            // ������Ʈ ���� �ð�
            yield return new WaitForSeconds(appearDuration);

            // ��� ������Ʈ ��Ȱ��ȭ
            foreach (var obj in selectedObjects)
            {
                obj.SetActive(false);
            }

            // ���� ���� ��� �ð�
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
