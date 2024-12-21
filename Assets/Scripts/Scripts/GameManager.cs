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
        public int PlayingTime = 0; // Main Scene에서 나타난 여부
        public int[] Cleared = new int[4]; // CCTV 씬에서 나타난 여부
        public int[] Score = new int[4];   // 각 레벨에서의 점수
    }

    // 플레이어 데이터베이스
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
    /// 새로운 플레이어 추가
    /// </summary>
    /// <param name="playerName">플레이어 이름</param>
    public void AddPlayer(string playerName)
    {
        // 중복된 이름 확인
        if (playerDatabase.Exists(p => p.PlayerName == playerName))
        {
            Debug.LogWarning($"플레이어 '{playerName}'는 이미 존재합니다.");
            return;
        }

        PlayerData newPlayer = new PlayerData
        {
            PlayerName = playerName
        };

        playerDatabase.Add(newPlayer);
        Debug.Log($"플레이어 '{playerName}'가 추가되었습니다.");
    }

    /// <summary>
    /// 플레이어 데이터 업데이트
    /// </summary>
    /// <param name="playerName">플레이어 이름</param>
    /// <param name="clearedLevel">클리어된 레벨 번호</param>
    /// <param name="score">레벨에서 얻은 점수</param>
    public void UpdatePlayerData(string playerName, int clearedLevel, int score)
    {
        PlayerData player = playerDatabase.Find(p => p.PlayerName == playerName);
        if (player == null)
        {
            Debug.LogWarning($"플레이어 '{playerName}'를 찾을 수 없습니다.");
            return;
        }

        if (clearedLevel < 0 || clearedLevel >= player.Cleared.Length)
        {
            Debug.LogWarning("유효하지 않은 레벨 번호입니다.");
            return;
        }

        // 데이터 업데이트
        player.Cleared[clearedLevel] = 1; // 클리어 표시
        player.Score[clearedLevel] = score;

        Debug.Log($"플레이어 '{playerName}'의 레벨 {clearedLevel} 업데이트 완료. 점수: {score}");
    }

    /// <summary>
    /// 플레이어 데이터베이스 출력
    /// </summary>
    public void PrintPlayerDatabase()
    {
        Debug.Log("플레이어 데이터베이스 출력:");
        foreach (var player in playerDatabase)
        {
            Debug.Log($"플레이어: {player.PlayerName}, Cleared: {string.Join(",", player.Cleared)}, Score: {string.Join(",", player.Score)}");
        }
    }
}
