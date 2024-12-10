using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed = 3.0f; // �̵� �ӵ�
    private float repositionThreshold = 1060f; // ��ġ ���� ���� ��

    void Update()
    {
        Move();

        // Ư�� ��ġ�� �����ϰų� �ʰ��ϸ� Reposition ����
        if (transform.position.x >= repositionThreshold)
        {
            Reposition();
        }
    }

    void Move()
    {
        // ���������� �̵�
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void Reposition()
    {
        // �ʰ��� ���� ����Ͽ� ��Ȯ�� -1060���� �̵�
        float excess = transform.position.x - repositionThreshold;
        transform.position = new Vector3(-repositionThreshold + excess, transform.position.y, transform.position.z);
    }
}
