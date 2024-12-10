using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGame : MonoBehaviour
{
    public float speedWhenNotClicked = 200f; // 클릭하지 않을 때 x축 이동량
    public float clickOffset = -110f; // 클릭 시 이동할 x축의 오프셋 값
    private bool isClicked = false; // 클릭 여부 확인용

    void Update()
    {
        // 클릭 여부 확인
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 클릭(모바일에서는 터치로 동작)
        {
            isClicked = true;
        }
        else
        {
            isClicked = false;
        }

        // 이동 처리
        HandleMovement();
    }

    void HandleMovement()
    {
        if (isClicked)
        {
            // 클릭 시 x 좌표를 즉시 감소
            transform.position = new Vector3(transform.position.x + clickOffset, transform.position.y, transform.position.z);
        }
        else
        {
            // 클릭하지 않을 때 x 좌표를 지속적으로 증가
            transform.Translate(Vector3.right * speedWhenNotClicked * Time.deltaTime);
        }
    }
}
