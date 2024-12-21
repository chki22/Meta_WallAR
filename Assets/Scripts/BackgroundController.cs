using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("Scrolling Speed")]
    public float speed = 3.0f; // ���������� �̵��� �ӵ�

    [Header("Screen Width (Note 20 Ultra)")]
    public float screenWidth = 1440f; // ���� ȭ�� �ʺ�

    private float spriteWidth; // ��� ��������Ʈ�� ���� �ʺ� (���� ����)

    void Start()
    {
        // ��������Ʈ ���� ��ǥ������ ���� �ʺ�
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        Move();

        // "���� ��" = (�߽� X) - (��������Ʈ �ʺ�/2)
        float leftEdgeX = transform.position.x - (spriteWidth / 2f);

        // ���� ���� 1440 �̻��̸� �� ������ -1440�� ����
        if (leftEdgeX >= screenWidth)
        {
            // ���ο� "�߽� X" = -1440 + (��������Ʈ �ʺ�/2)
            float newCenterX = -screenWidth + (spriteWidth / 2f);
            transform.position = new Vector3(newCenterX, transform.position.y, transform.position.z);
        }
    }

    // ����� ���������� �̵� (speed�� ����� ���������� ����)
    void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
}
