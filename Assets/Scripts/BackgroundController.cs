using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("Scrolling Speed")]
    public float speed = 3.0f; // 오른쪽으로 이동할 속도

    [Header("Screen Width (Note 20 Ultra)")]
    public float screenWidth = 1440f; // 고정 화면 너비

    private float spriteWidth; // 배경 스프라이트의 실제 너비 (월드 단위)

    void Start()
    {
        // 스프라이트 월드 좌표에서의 실제 너비
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        Move();

        // "왼쪽 끝" = (중심 X) - (스프라이트 너비/2)
        float leftEdgeX = transform.position.x - (spriteWidth / 2f);

        // 왼쪽 끝이 1440 이상이면 → 강제로 -1440에 맞춤
        if (leftEdgeX >= screenWidth)
        {
            // 새로운 "중심 X" = -1440 + (스프라이트 너비/2)
            float newCenterX = -screenWidth + (spriteWidth / 2f);
            transform.position = new Vector3(newCenterX, transform.position.y, transform.position.z);
        }
    }

    // 배경을 오른쪽으로 이동 (speed가 양수면 오른쪽으로 전진)
    void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
}
