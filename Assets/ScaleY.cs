using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    public Transform targetObject; // Ư�� ��ü (���� ��ü)
    public List<Transform> objectsToScale; // Y�� �������� ������ ��ü ����Ʈ
    public float[] baseScaleSpeeds; // �� ��ü�� �⺻ Y�� ������ ���� �ӵ�
    public float boostMultiplier = 2.0f; // X���� 200 �̳��� �� ���� �ӵ� ����
    public float proximityThreshold = 200.0f; // X�� ���� ����

    void Update()
    {
        for (int i = 0; i < objectsToScale.Count; i++)
        {
            Transform obj = objectsToScale[i];

            // X�� ���̸� ���
            float distanceX = Mathf.Abs(obj.position.x - targetObject.position.x);

            // �⺻ ������ ���� �ӵ�
            float scaleSpeed = baseScaleSpeeds[i];

            // X�� ���̰� proximityThreshold �̳��̸� ���� �ӵ��� boostMultiplier ����
            if (distanceX <= proximityThreshold)
            {
                scaleSpeed *= boostMultiplier;
            }

            // Y�� ������ ����
            obj.localScale += new Vector3(0, scaleSpeed * Time.deltaTime, 0);
        }
    }
}
