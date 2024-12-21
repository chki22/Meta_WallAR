using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName;
        public int PlayingTime = 0; // Main Scene���� ��Ÿ�� ����
        public int[] Cleared = new int[4]; // CCTV ������ ��Ÿ�� ����
        public int[] Score = new int[4];   // �� ���������� ����
    }

    // �÷��̾� �����ͺ��̽�
    private List<PlayerData> playerDatabase = new List<PlayerData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���ο� �÷��̾� �߰�
    /// </summary>
    /// <param name="playerName">�÷��̾� �̸�</param>
    public void AddPlayer(string playerName)
    {
        // �ߺ��� �̸� Ȯ��
        if (playerDatabase.Exists(p => p.PlayerName == playerName))
        {
            Debug.LogWarning($"�÷��̾� '{playerName}'�� �̹� �����մϴ�.");
            return;
        }

        PlayerData newPlayer = new PlayerData
        {
            PlayerName = playerName
        };

        playerDatabase.Add(newPlayer);
        Debug.Log($"�÷��̾� '{playerName}'�� �߰��Ǿ����ϴ�.");
    }

    /// <summary>
    /// �÷��̾� ������ ������Ʈ
    /// </summary>
    /// <param name="playerName">�÷��̾� �̸�</param>
    /// <param name="clearedLevel">Ŭ����� ���� ��ȣ</param>
    /// <param name="score">�������� ���� ����</param>
    public void UpdatePlayerData(string playerName, int clearedLevel, int score)
    {
        PlayerData player = playerDatabase.Find(p => p.PlayerName == playerName);
        if (player == null)
        {
            Debug.LogWarning($"�÷��̾� '{playerName}'�� ã�� �� �����ϴ�.");
            return;
        }

        if (clearedLevel < 0 || clearedLevel >= player.Cleared.Length)
        {
            Debug.LogWarning("��ȿ���� ���� ���� ��ȣ�Դϴ�.");
            return;
        }

        // ������ ������Ʈ
        player.Cleared[clearedLevel] = 1; // Ŭ���� ǥ��
        player.Score[clearedLevel] = score;

        Debug.Log($"�÷��̾� '{playerName}'�� ���� {clearedLevel} ������Ʈ �Ϸ�. ����: {score}");
    }

    /// <summary>
    /// �÷��̾� �����ͺ��̽� ���
    /// </summary>
    public void PrintPlayerDatabase()
    {
        Debug.Log("�÷��̾� �����ͺ��̽� ���:");
        foreach (var player in playerDatabase)
        {
            Debug.Log($"�÷��̾�: {player.PlayerName}, Cleared: {string.Join(",", player.Cleared)}, Score: {string.Join(",", player.Score)}");
        }
    }
}
