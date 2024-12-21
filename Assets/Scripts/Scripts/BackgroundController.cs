using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed = 3.0f; // 이동 속도
    private float repositionThreshold;

    void Start()
    {
        // 화면의 오른쪽 끝을 기준으로 repositionThreshold 계산
        repositionThreshold = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        Move();

        // 특정 위치에 도달하거나 초과하면 Reposition 실행
        if (transform.position.x >= repositionThreshold)
        {
            Reposition();
        }
    }

    void Move()
    {
        // 오른쪽으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void Reposition()
    {
        // 초과된 값을 계산하여 정확히 왼쪽 끝으로 이동
        float excess = transform.position.x - repositionThreshold;

        float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - GetComponent<SpriteRenderer>().bounds.size.x / 2;
        transform.position = new Vector3(leftLimit + excess, transform.position.y, transform.position.z);
    }
}
