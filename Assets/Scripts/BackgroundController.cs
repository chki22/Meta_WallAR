using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed = 3.0f; // �̵� �ӵ�
    private float repositionThreshold;

    void Start()
    {
        // ȭ���� ������ ���� �������� repositionThreshold ���
        repositionThreshold = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

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
        // �ʰ��� ���� ����Ͽ� ��Ȯ�� ���� ������ �̵�
        float excess = transform.position.x - repositionThreshold;

        float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - GetComponent<SpriteRenderer>().bounds.size.x / 2;
        transform.position = new Vector3(leftLimit + excess, transform.position.y, transform.position.z);
    }
}
