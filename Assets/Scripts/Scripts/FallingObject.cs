using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 300f; // 오브젝트 하강 속도
    public RectTransform destroyArea; // 파괴 영역 (Panel)

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        Vector3 destroyAreaWorldPosition = destroyArea.position;
        float destroyAreaTopY = destroyAreaWorldPosition.y + (destroyArea.rect.height * 0.5f);

        if (transform.position.y < destroyAreaTopY)
        {
            GameManagerCat.Instance.OnObjectMissed();
            Destroy(gameObject);
        }
    }

    public void OnObjectClicked()
    {
        Debug.Log("Object clicked!");
        GameManagerCat.Instance.OnObjectHit(); // 점수 증가
        Destroy(gameObject); // 오브젝트 삭제
    }
}
