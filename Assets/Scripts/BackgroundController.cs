using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed = 3.0f; // 이동 속도
    private float repositionThreshold = 1060f; // 위치 변경 기준 값

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
        // 초과된 값을 계산하여 정확히 -1060으로 이동
        float excess = transform.position.x - repositionThreshold;
        transform.position = new Vector3(-repositionThreshold + excess, transform.position.y, transform.position.z);
    }
}
