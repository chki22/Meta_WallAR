using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    public Transform targetObject; // 특정 물체 (기준 물체)
    public List<Transform> objectsToScale; // Y축 스케일이 증가할 물체 리스트
    public float[] baseScaleSpeeds; // 각 물체의 기본 Y축 스케일 증가 속도
    public float boostMultiplier = 2.0f; // X값이 200 이내일 때 증가 속도 배율
    public float proximityThreshold = 200.0f; // X값 차이 기준

    void Update()
    {
        for (int i = 0; i < objectsToScale.Count; i++)
        {
            Transform obj = objectsToScale[i];

            // X값 차이를 계산
            float distanceX = Mathf.Abs(obj.position.x - targetObject.position.x);

            // 기본 스케일 증가 속도
            float scaleSpeed = baseScaleSpeeds[i];

            // X값 차이가 proximityThreshold 이내이면 증가 속도에 boostMultiplier 적용
            if (distanceX <= proximityThreshold)
            {
                scaleSpeed *= boostMultiplier;
            }

            // Y축 스케일 증가
            obj.localScale += new Vector3(0, scaleSpeed * Time.deltaTime, 0);
        }
    }
}
