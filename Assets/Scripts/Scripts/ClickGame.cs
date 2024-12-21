using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGame : MonoBehaviour
{
    public float speedWhenNotClicked = 200f; // Ŭ������ ���� �� x�� �̵���
    public float clickOffset = -110f; // Ŭ�� �� �̵��� x���� ������ ��
    private bool isClicked = false; // Ŭ�� ���� Ȯ�ο�

    void Update()
    {
        // Ŭ�� ���� Ȯ��
        if (Input.GetMouseButton(0)) // ���콺 ���� Ŭ��(����Ͽ����� ��ġ�� ����)
        {
            isClicked = true;
        }
        else
        {
            isClicked = false;
        }

        // �̵� ó��
        HandleMovement();
    }

    void HandleMovement()
    {
        if (isClicked)
        {
            // Ŭ�� �� x ��ǥ�� ��� ����
            transform.position = new Vector3(transform.position.x + clickOffset, transform.position.y, transform.position.z);
        }
        else
        {
            // Ŭ������ ���� �� x ��ǥ�� ���������� ����
            transform.Translate(Vector3.right * speedWhenNotClicked * Time.deltaTime);
        }
    }
}
