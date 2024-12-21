using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public RectTransform spawnArea; // 생성 영역 (Panel)
    public RectTransform destroyArea; // 파괴 영역 (Panel) 추가
    public float spawnInterval = 1.5f; // 생성 간격

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval); // 일정 간격으로 오브젝트 생성
    }

    void SpawnObject()
    {
        if (objectPrefab == null || spawnArea == null || destroyArea == null) return;

        // SpawnArea 내부에서 랜덤한 위치 계산
        float randomX = Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax);
        float randomY = spawnArea.rect.yMax;
        Vector3 spawnPosition = spawnArea.TransformPoint(new Vector3(randomX, randomY, 0));

        // 오브젝트 생성
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, spawnArea.transform);


        // FallingObject에 DestroyArea 설정
        FallingObject fallingObject = newObject.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.destroyArea = destroyArea; // DestroyArea를 자동으로 할당
        }
    }
}
